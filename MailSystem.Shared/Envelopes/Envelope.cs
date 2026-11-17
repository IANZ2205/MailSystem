namespace MailSystem.Shared.Envelopes
{
    public class Envelope
    {
        public string Type { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public string? Payload { get; set; } // JSON string
    }
}