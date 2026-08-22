namespace Summary.Bale.Services
{
    using Core.Mvc.Utilities;
    using Core.Workflows;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public interface IBotService
    {
        Task SendMessageAsync(
            string to,
            string message,
            string file
        );
    }

    public class BotService : IBotService
    {
        private readonly HttpRequestClient _client;
        private readonly BaleSettings _options;
        private const string URL = "https://tapi.bale.ai";

        public BotService(
            HttpRequestClient client,
            IOptions<BaleSettings> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task SendMessageAsync(
            string to,
            string message,
            string file)
        {
            message = message.ConvertHtmlToBaleFormat();

            if (String.IsNullOrWhiteSpace(file)) await SendTextMessageAsync(to, message);
            else
                await SendPhotoMessageAsync(to, file, message);
        }

        private async Task SendTextMessageAsync(string to, string message)
        {
            if (String.IsNullOrWhiteSpace(message)) throw new WorkflowException(
                "مقدار فیلد توضیحات خالی است.",
                null,
                null,
                "کاربر گرامی؛ در تسک ارسال پیغام از طریق بات پیام رسان بله، فیلد توضیحات خالی است. لطفاً مقدار مناسبی برای این فیلد وارد نمایید."
            );

            var data = new
            {
                chat_id = to,
                text = message,
                reply_markup = new
                {
                    keyboard = new object[] { },
                    resize_keyboard = true,
                    one_time_keyboard = true,
                    selective = true
                },
                reply_to_message_id = 0
            };

            var response = await _client.SendPostRequestAsync<BotApiResponseModel<SendMessageResultModel>>(
                $"{URL}/bot{_options.Token}/sendMessage",
                data,
                true
            );

            if (response.Ok is false) ThrowExceptionIf.SendBotMessageResponseIsNotOk(
                response.Error_Code.Value,
                response.Description,
                JsonConvert.SerializeObject(data)
            );
        }

        private async Task SendPhotoMessageAsync(string to, string file, string caption)
        {
            var data = new
            {
                chat_id = to,
                photo = file,
                caption = caption
            };

            var response = await _client.SendPostRequestAsync<BotApiResponseModel<SendPhotoResultModel>>(
                $"{URL}/bot{_options.Token}/sendPhoto",
                data,
                true
            );

            if (response.Ok is false) ThrowExceptionIf.SendBotMessageResponseIsNotOk(
                response.Error_Code.Value,
                response.Description,
                JsonConvert.SerializeObject(data)
            );
        }
    }
}