using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vonage.Messaging;
using Vonage.Request;

namespace WebApp.RegressionTests
{
    public sealed class LoggerSmsClient : ISmsClient
    {
        private readonly ILogger<LoggerSmsClient> _logger;

        public LoggerSmsClient(ILogger<LoggerSmsClient> logger)
        {
            _logger = logger;
        }

        public Task<SendSmsResponse> SendAnSmsAsync(string from, string to, string text, SmsType type = SmsType.text, Credentials creds = null)
        {
            _logger.LogInformation($"SMS sent from {from} to {to} with text: {text}");
            return Task.FromResult(new SendSmsResponse());
        }

        public Task<SendSmsResponse> SendAnSmsAsync(SendSmsRequest request, Credentials creds = null) =>
            throw new System.NotImplementedException();

        public SendSmsResponse SendAnSms(SendSmsRequest request, Credentials creds = null) =>
            throw new System.NotImplementedException();

        public SendSmsResponse SendAnSms(string @from, string to, string text, SmsType type = SmsType.text, Credentials creds = null) =>
            throw new System.NotImplementedException();
    }
}
