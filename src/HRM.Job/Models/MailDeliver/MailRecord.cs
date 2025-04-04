using MimeKit;

namespace HRM.Job.Models.MailDeliver;

public class MailRecord
{
    public long MailId { get; set; }

    public MailSetting MailSetting { get; set; }

    public int Priority { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string MailTo { get; set; }

    public string MailCc { get; set; }

    public string MailBcc { get; set; }

    public AttachmentCollection Attachments { get; set; } = [];
}