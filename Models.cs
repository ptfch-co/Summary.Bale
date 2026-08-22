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

    public class BotApiResponseModel<T>
    {
        public bool Ok { get; set; }
        public T Result { get; set; }
        public Bot_Error_Code? Error_Code { get; set; }
        public string Description { get; set; }
    }

    public enum Bot_Error_Code
    {
        Unknown = 0,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
    }

    public class BotChatModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
    }

    public class BotFromModel
    {
        public long Id { get; set; }
        public bool Is_Bot { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
    }

    public class SendMessageResultModel
    {
        public long Message_Id { get; set; }
        public BotFromModel From { get; set; }
        public long Date { get; set; }
        public BotChatModel Chat { get; set; }
        public string Text { get; set; }
    }

    public class PhotoSizeModel
    {
        public string File_Id { get; set; }
        public string File_Unique_Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long File_Size { get; set; }
    }

    public class SendPhotoResultModel
    {
        public long Message_Id { get; set; }
        public BotFromModel From { get; set; }
        public long Date { get; set; }
        public BotChatModel Chat { get; set; }
        public System.Collections.Generic.List<PhotoSizeModel> Photo { get; set; }
        public string Caption { get; set; }
    }
}

