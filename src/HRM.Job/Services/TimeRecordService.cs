using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Logging;
using HRM.Job.Constants;
using HRM.Job.Criteria;
using HRM.Job.Extensions;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;
using Newtonsoft.Json;

namespace HRM.Job.Services;

public class TimeRecordService(
    List<TimeRecordApiSetting> apiSettings,
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
        var toDate = DateTimeOffset.Now.DateTime.ToClientTimeZone(8);
        var fromDate = toDate.AddHours(-12);
        var url = new Uri($"{setting.Url}?fromDate={fromDate:yyyy-MM-dd HH:mm:ss}&toDate={toDate:yyyy-MM-dd HH:mm:ss}");
        var httpClient = clientFactory.CreateClient();

        if (!string.IsNullOrWhiteSpace(setting.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);
        }

        try
        {
            await logger.InfoAsync($"Sync time record: {url}");

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var records = JsonConvert.DeserializeObject<IList<TimeRecordAPI>>(rawData);
                if (records != null && records.Any())
                {
                    await ImportTimeRecordAsync(fromDate, toDate, records);
                }
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                await logger.ErrorAsync($"Sync time record, StatusCode: {response.StatusCode}\r\nMessage: {message}");
            }
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
        }
    }

    private async Task ImportTimeRecordAsync(DateTime fromDate, DateTime toDate, IList<TimeRecordAPI> records)
    {
        var recordsByCompany = records.GroupBy(x => x.Company);
        foreach (var record in recordsByCompany)
        {
            var company = TimeRecordApiConstant.CompanyDict.GetValueOrDefault(record.Key);
            if (company == default)
            {
                continue;
            }

            var (companyId, clientTimeZone) = company;
            var timeRecords = record.ToList();
            foreach (var timeRecord in timeRecords)
            {
                timeRecord.RecordTime = timeRecord.RecordTime.ToServerTimeZone(clientTimeZone);
            }

            await logger.InfoAsync($"Import time record: {companyId} - {JsonConvert.SerializeObject(timeRecords)}");

            const short batchSize = 20;
            var totalBatch = (int)Math.Ceiling((double)timeRecords.Count / batchSize);
            for (var i = 0; i < totalBatch; i++)
            {
                var batchData = timeRecords.Skip(i * batchSize).Take(batchSize).ToList();

                var command = new ImportTimeRecordCommand
                {
                    CompanyId = companyId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    TimeRecordXml = batchData.ToXML().ToString()
                };

                var thread = new Thread(() => baseService.ExecuteNonQueryAsync(command));
                thread.Start();
            }
        }
    }
}