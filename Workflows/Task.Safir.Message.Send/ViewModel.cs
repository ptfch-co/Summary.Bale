namespace Summary.Bale.Workflows.Task.Message.Send
{
    using System.ComponentModel.DataAnnotations;

    public class SendMessageInBaleTaskViewModel
    {
        [Required] public string PhoneNo { get; set; }
        [Required] public string Message { get; set; }
        public string File { get; set; }
    }
}