namespace Summary.Bale.Workflows.Task.Bot.Message.Send
{
    using Core.Workflows;
    using Core.Workflows.Activities;
    using Core.Workflows.Abstractions.Models;
    using Core.Workflows.Models;
    using Microsoft.Extensions.Localization;
    using Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SendBotMessageInBaleTask : TaskActivity
    {
        private readonly IStringLocalizer<SendBotMessageInBaleTask> T;
        private readonly IBotService _message;

        public SendBotMessageInBaleTask(
            IStringLocalizer<SendBotMessageInBaleTask> t,
            IBotService message)
        {
            T = t;
            _message = message;
        }

        public override string Name => T[Bale.Localization.SOfSendBotMessage];

        public override LocalizedString DisplayText => T[Bale.Localization.DOfSendBotMessage];

        public override LocalizedString Category => T[Bale.Public.Category];

        public override IEnumerable<Outcome> GetPossibleOutcomes(
            WorkflowExecutionContext workflowContext,
            ActivityContext activityContext)
        {
            return Outcomes(T[Bale.Workflows.Done]);
        }

        public string To
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
            var to = workflowContext.GetInputOrDefault(To);
            var message = workflowContext.GetInputOrDefault(Message);
            var file = workflowContext.GetInputOrDefault(File);

            await _message.SendMessageAsync(
                to,
                message,
                file
            );

            return Outcomes(Bale.Workflows.Done);
        }
    }
}