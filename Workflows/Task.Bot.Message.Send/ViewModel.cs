namespace Summary.Bale.Workflows.Task.Bot.Message.Send
{
    using System.ComponentModel.DataAnnotations;

    public class SendBotMessageInBaleViewModel
    {
        [Required]
        public string To { get; set; }
        public string Message { get; set; }
        public string File { get; set; }
    }
}