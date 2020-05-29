namespace NotificationService.Configuration
{
    public interface ISmtpSettings
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
        bool SmtpUseSsl { get; set; }

    }
}
