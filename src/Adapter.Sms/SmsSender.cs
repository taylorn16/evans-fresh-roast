using System.Threading.Tasks;
using Application.Ports;
using Domain;
using Vonage.Messaging;

namespace Adapter.Sms
{
    internal sealed class SmsSender : ISendSms
    {
        private readonly ISmsClient _vonageSmsClient;

        public SmsSender(ISmsClient vonageSmsClient)
        {
            _vonageSmsClient = vonageSmsClient;
        }

        public async Task Send(UsPhoneNumber phoneNumber, SmsMessage message)
        {
            await _vonageSmsClient.SendAnSmsAsync("12406106727", phoneNumber, message);
        }
    }
}
