namespace WebApp.Dto
{
    public sealed record InvoicePutRequest
    {
        public string? PaymentMethod { get; init; }
        public bool? IsPaid { get; init; }
    }
}
