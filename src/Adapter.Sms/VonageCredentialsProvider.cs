using Vonage.Request;

namespace Adapter.Sms
{
    internal interface IVonageCredentialsProvider
    {
        Credentials Get();
    }

    internal sealed class VonageCredentialsProvider : IVonageCredentialsProvider
    {
        public Credentials Get() => new()
        {
            ApiKey = "e2aea84b",
            ApiSecret = "5qvwkabv7m1Ypsi6",
        };
    }
}
