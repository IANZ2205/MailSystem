namespace MailSystem.Shared.Envelopes
{
    public class ResponseEnvelope
    {
        public string Type { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Payload { get; set; } // JSON string
    }
}