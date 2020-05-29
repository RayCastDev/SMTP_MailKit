using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Model;
using NotificationService.Service;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ISmtpProvider _smtpProvider;

        public MessageController(ISmtpProvider smtpProvider)
        {
            _smtpProvider = smtpProvider;
        }

        [Route("~/api/message/test")]
        [HttpGet]
        public IEnumerable<string> Test()
        {
            return new[] { "Пожилой", "Жмых" };
        }

        [Route("~/api/message/send")]
        [HttpPost]
        public async Task<IActionResult> Send([FromForm]EmailMessage emailMessage)
        {
            if (emailMessage == null)
            {
                return BadRequest();
            }
            await _smtpProvider.SendEmailAsync(emailMessage);
            return Ok(new {status = true, message = $"The message has been sent to {emailMessage.RecipientAddress}" });
        }

    }
}