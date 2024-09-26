namespace BusinessObjects.DTOs
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public int StatusCode { get; set; }
    }
}
