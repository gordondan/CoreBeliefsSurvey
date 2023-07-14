using System;

namespace CoreBeliefsSurvey.Shared.Models
{
    public class PdfCreationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public byte[] PdfData { get; set; }
        public string Url { get; set; }
        public Guid PdfId { get; set; }
    }
}
