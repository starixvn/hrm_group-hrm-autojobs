using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HRM.Job.Constants;
using HRM.Job.Criteria.MailDeliver;
using HRM.Job.Models;
using HRM.Job.Models.MailDeliver;
using HRM.Job.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;

namespace HRM.Job.Services;

public class BaseMailService(
    IBaseService baseService) : IBaseMailService
{
    public async Task<MailConfig> GetMailConfigAsync(short companyId)
    {
        var criteria = new GetSystemConfigsByKeyCriteria
        {
            CompanyId = companyId,
            Key = JobSettingConstants.MailSetting
        };

        var result = await baseService.GetDataEntityAsync<string>(criteria);
        var data = result == null ? null : JsonConvert.DeserializeObject<MailConfig>(result);

        return data;
    }


    public async Task<bool> SendEmailAsync(MailRecord email)
    {
        var isSuccessful = false;
        if (email != null && IsValidEmail(email.MailTo))
        {
            await InsertMailRecordAsync(email);
            isSuccessful = await SendByMailKitAsync(email);
        }

        return isSuccessful;
    }

    private async Task<bool> SendByMailKitAsync(MailRecord email)
    {
        var message = GetMessageContent(email);
        var isSuccessful = false;

        using var mailClient = new SmtpClient();
        mailClient.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
        await mailClient.ConnectAsync(email.MailSetting.Server, email.MailSetting.Port, email.MailSetting.IsAuth);
        if (email.MailSetting.IsAuth)
        {
            await mailClient.AuthenticateAsync(email.MailSetting.Username, email.MailSetting.Password);
        }

        try
        {
            await mailClient.SendAsync(message);
            isSuccessful = true;
            await UpdateMailStatusAsync(email, true);
        }
        catch (Exception ex)
        {
            await HandleMailErrorAsync(email, ex.Message, ex.StackTrace);
        }

        await mailClient.DisconnectAsync(true);

        return isSuccessful;
    }

    private static void GetMailAddressFromString(string mailString, InternetAddressList mails)
    {
        if (!string.IsNullOrWhiteSpace(mailString))
        {
            var emails = RemoveDoubleComma(mailString);
            var mailArrays = emails.Split(';');
            foreach (var mail in mailArrays)
            {
                if (!string.IsNullOrEmpty(mail))
                {
                    mails.Add(new MailboxAddress(string.Empty, mail));
                }
            }
        }
    }

    private static string RemoveDoubleComma(string mails)
    {
        var emails = mails;
        while (emails.Contains(",,"))
        {
            emails = emails.Replace(",,", ",");
            emails = emails.Replace(",", ";");
        }

        while (emails.Contains(";;"))
        {
            emails = emails.Replace(";;", ";");
        }

        return emails.TrimEnd(',', ';');
    }

    private static MimeMessage GetMessageContent(MailRecord email)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(string.Empty, email.MailSetting.Username));
        GetMailAddressFromString(email.MailTo, message.To);
        GetMailAddressFromString(email.MailCc, message.Cc);
        GetMailAddressFromString(email.MailBcc, message.Bcc);

        message.Subject = email.Subject;
        message.Priority = (MessagePriority)email.Priority;

        var builder = new BodyBuilder
        {
            HtmlBody = email.Body
        };
        foreach (var attachment in email.Attachments)
        {
            builder.Attachments.Add(attachment);
        }

        message.Body = builder.ToMessageBody();

        return message;
    }

    public async Task<MailSetting> GetMailSettingByIdAsync(int appId)
    {
        var criteria = new GetMailSettingByIdCriteria
        {
            SettingId = appId
        };
        return await baseService.GetDataEntityAsync<MailSetting>(criteria);
    }

    private async Task InsertMailRecordAsync(MailRecord email)
    {
        var criteria = new InsertMailRecordCommand
        {
            AppId = email.MailSetting.AppId,
            Subject = email.Subject,
            Body = email.Body,
            Attachments = string.Empty,
            MailTo = email.MailTo,
            MailCc = email.MailCc,
            MailBcc = email.MailBcc,
            MailDate = DateTimeOffset.Now.DateTime
        };

        await baseService.ExecuteNonQueryAsync(criteria);
        email.MailId = criteria.MailId;
    }

    private async Task HandleMailErrorAsync(MailRecord email, string errorMessage, string stackTrace)
    {
        await InsertMailErrorAsync(email, errorMessage, stackTrace);
        await UpdateMailStatusAsync(email, false);
    }

    private async Task InsertMailErrorAsync(MailRecord email, string errorMessage, string stackTrace)
    {
        var command = new InsertMailErrorCommand
        {
            AppId = email.MailSetting.AppId,
            MailId = email.MailId,
            Message = errorMessage,
            StackTrace = stackTrace,
            ErrorTime = DateTimeOffset.Now.DateTime
        };

        await baseService.ExecuteNonQueryAsync(command);
    }

    private async Task UpdateMailStatusAsync(MailRecord email, bool isSuccessful)
    {
        var command = new UpdateMailStatusCommand
        {
            MailId = email.MailId,
            Status = isSuccessful,
            MailUpdatedDate = DateTimeOffset.Now.DateTime
        };

        await baseService.ExecuteNonQueryAsync(command);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        return regex.IsMatch(email);
    }

    private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        => true;
}