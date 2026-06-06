namespace Summary.Bale
{
    public class SendMessageResponseModel
    {
        public string Message_Id { get; set; }
        public Error_Data[] Error_Data { get; set; }
    }

    public class Error_Data
    {
        public string Phone_Number { get; set; }
        public Error_Code Code { get; set; }
        public string Description { get; set; }
    }

    public enum Error_Code
    {
        InternalServerError = 2,
        RateLimitExceeded = 3,
        InvalidInput = 4,
        InvalidPhone = 8,
        NotBaleUser = 17,
        PaymentRequired = 20
    }

    public class UploadFileResponseModel
    {
        public string File_Id { get; set; }
    }
}