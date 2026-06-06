namespace Summary.Bale.Workflows.Task.Message.Send
{
    using Core.Workflows.Display;

    public class SendMessageInBaleTaskDisplay :
        ActivityDisplayDriver<SendMessageInBaleTask, SendMessageInBaleTaskViewModel>
    {
        protected override void EditActivity(
            SendMessageInBaleTask activity,
            SendMessageInBaleTaskViewModel model)
        {
            model.PhoneNo = activity.PhoneNo;
            model.Message = activity.Message;
            model.File = activity.File;
        }

        protected override void UpdateActivity(
            SendMessageInBaleTaskViewModel model,
            SendMessageInBaleTask activity)
        {
            activity.PhoneNo = model.PhoneNo;
            activity.Message = model.Message;
            activity.File = model.File;
        }
    }
}