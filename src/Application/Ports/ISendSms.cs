using System.Threading.Tasks;
using Domain;

namespace Application.Ports
{
    public interface ISendSms
    {
        Task Send(UsPhoneNumber phoneNumber, SmsMessage message);
    }
}
