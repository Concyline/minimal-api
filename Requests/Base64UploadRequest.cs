namespace MinimalApi.Requests
{
    public class Base64UploadRequest
    {
        public string Nome { get; set; } = "";
        public string? ContentType { get; set; }
        public string Base64 { get; set; } = "";
    }
}
