namespace Ecommerce_DBFirst.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Extra fields for a richer error page
        public int StatusCode { get; set; } = 500;
        public string Title { get; set; } = "Something went wrong";
        public string Message { get; set; } = "We're sorry — an unexpected error occurred while processing your request.";
    }
}