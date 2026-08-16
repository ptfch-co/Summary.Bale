using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Summary.Bale.Services
{
    using Core.Mvc.Utilities;
    using Core.Workflows;
    using Core.Workflows.Helpers;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
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
            if (String.IsNullOrWhiteSpace(to))
                throw new ArgumentException("شناسه مقصد (chat_id) نمی‌تواند خالی باشد.", "to");

            if (String.IsNullOrWhiteSpace(message) is false)
                message = message.ConvertHtmlToBaleFormat();

            if (String.IsNullOrWhiteSpace(file))
                await SendTextMessageAsync(to, message);
            else
                await SendPhotoMessageAsync(to, file, message);
        }

        private async Task SendTextMessageAsync(string to, string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                throw new ArgumentException("برای ارسال پیام متنی، فیلد message نباید خالی باشد.", "message");

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
                $"{URL}/bot{_options.ApiAccessKey}/sendMessage",
                data,
                "application/json",
                true
            );

            if (response.Ok is false) ThrowExceptionIf.SendMessageResponseIsNotOk(
                response.Error_Code.ToLegacyErrorCode(),
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
                caption = String.IsNullOrWhiteSpace(caption) ? null : caption
            };

            var response = await _client.SendPostRequestAsync<BotApiResponseModel<SendPhotoResultModel>>(
                $"{URL}/bot{_options.ApiAccessKey}/sendPhoto",
                data,
                "application/json",
                true
            );

            if (response.Ok is false) ThrowExceptionIf.SendMessageResponseIsNotOk(
                response.Error_Code.ToLegacyErrorCode(),
                response.Description,
                JsonConvert.SerializeObject(data)
            );
        }
    }
}