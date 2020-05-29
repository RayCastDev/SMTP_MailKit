using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace NotificationService.Model
{
    public class EmailMessage
    {
        public string SenderName { get; set; }
        public string RecipientAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
