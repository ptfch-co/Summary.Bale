namespace Summary.Bale.Workflows.Task.Bot.Message.Send
{
    using Core.Workflows.Display;

    public class SendBotMessageInBaleDisplay :
        ActivityDisplayDriver<SendBotMessageInBaleTask, SendBotMessageInBaleViewModel>
    {
        protected override void EditActivity(
            SendBotMessageInBaleTask activity,
            SendBotMessageInBaleViewModel model)
        {
            model.To = activity.To;
            model.Message = activity.Message;
            model.File = activity.File;
        }

        protected override void UpdateActivity(
            SendBotMessageInBaleViewModel model,
            SendBotMessageInBaleTask activity)
        {
            activity.To = model.To;
            activity.Message = model.Message;
            activity.File = model.File;
        }
    }
}