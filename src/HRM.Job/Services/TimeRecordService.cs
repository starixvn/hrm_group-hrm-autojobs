using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HRM.Job.Criteria;
using HRM.Job.Services.Interfaces;
using Newtonsoft.Json;
using Fanex.Logging;
using System.Net.Http;
using HRM.Job.Models;
using System.Linq;
using System.Threading;
using HRM.Job.Extensions;

namespace HRM.Job.Services;

public class TimeRecordService(
    IList<TimeRecordApiSetting> apiSettings,
    IHttpClientFactory clientFactory,
    ILogger logger,
    IBaseService baseService) : ITimeRecordService
{
    public async Task ImportTimeRecordAsync()
    {
        foreach (var setting in apiSettings)
        {
            var thread = new Thread(() => GetTimeRecordAsync(setting));
            thread.Start();
        }
    }

    public async Task GetTimeRecordAsync(TimeRecordApiSetting setting)
    {
        var toDate = DateTimeOffset.Now.DateTime;
        var fromDate = toDate.Date;
        var url = new Uri($"{setting.Url}?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd mm:HH:ss}");
        var httpClient = clientFactory.CreateClient();

        if (!string.IsNullOrWhiteSpace(setting.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);
        }

        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var rawData = await response.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<IList<TimeRecordAPI>>(rawData);
            if (records != null && records.Any())
            {
                var command = new ImportTimeRecordCommand
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    TimeRecordXml = records.ToXML().ToString()
                };
                await baseService.ExecuteNonQueryAsync(command);
            }
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            await logger.ErrorAsync($"Sync time record, StatusCode: {response.StatusCode}\r\nMessage: {message}");
        }
    }
}