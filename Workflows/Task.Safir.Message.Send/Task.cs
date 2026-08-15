namespace Summary.Bale.Workflows.Task.Message.Send
{
    using Core.Workflows;
    using Core.Workflows.Activities;
    using Core.Workflows.Abstractions.Models;
    using Core.Workflows.Models;
    using Microsoft.Extensions.Localization;
    using Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SendMessageInBaleTask : TaskActivity
    {
        private readonly IStringLocalizer<SendMessageInBaleTask> T;
        private readonly ISafirMessageService _message;

        public SendMessageInBaleTask(
            IStringLocalizer<SendMessageInBaleTask> t,
            ISafirMessageService message)
        {
            T = t;
            _message = message;
        }

        public override string Name => T[Bale.Localization.SOfSendMessage];

        public override LocalizedString DisplayText => T[Bale.Localization.DOfSendMessage];

        public override LocalizedString Category => T[Bale.Public.Category];

        public override IEnumerable<Outcome> GetPossibleOutcomes(
            WorkflowExecutionContext workflowContext,
            ActivityContext activityContext)
        {
            return Outcomes(T[Bale.Workflows.Done], T[Bale.Workflows.NotExist]);
        }

        public string PhoneNo
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public string Message
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public string File
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(
            WorkflowExecutionContext workflowContext,
            ActivityContext activityContext)
        {
            var phone_no = workflowContext.GetInputOrDefault(PhoneNo);
            var message = workflowContext.GetInputOrDefault(Message);
            var file = workflowContext.GetInputOrDefault(File);

            try
            {
                await _message.SendMessageAsync(
                    phone_no,
                    message,
                    file
                );

                return Outcomes(Bale.Workflows.Done);
            }

            catch (ObjectDoesNotExist)
            {
                return Outcomes(Bale.Workflows.NotExist);
            }

            catch
            {
                throw;
            }
        }
    }
}