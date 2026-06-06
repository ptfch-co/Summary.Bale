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
    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IMessageService
    {
        Task SendMessageAsync(
            string phone_no,
            string message,
            string file_path
        );
    }

    public class MessageService : IMessageService
    {
        private readonly HttpRequestClient _client;
        private readonly BaleSettings _options;
        private readonly IMemoryCache _memory;

        private const string URL = "https://safir.bale.ai";

        public MessageService(
            HttpRequestClient client,
            IOptions<BaleSettings> options,
            IMemoryCache memory)
        {
            _client = client;
            _options = options.Value;
            _memory = memory;
        }

        public async Task SendMessageAsync(
            string phone_no,
            string message,
            string file_path)
        {
            ThrowExceptionIf.MobileIsNotValid(phone_no);

            phone_no = phone_no.RemoveMobilePrefixNo("98");
            message = message.ConvertHtmlToBaleFormat();

            var file_id = string.Empty;

            if (String.IsNullOrWhiteSpace(file_path) is false)
            {
                if (_memory.TryGetValue(file_path, out var value)) file_id = (string)value;

                else
                {
                    var stream = await _client.SendGetRequestAsync<Stream>(file_path);

                    var file_name = Path.GetFileName(new Uri(file_path).LocalPath);

                    if (String.IsNullOrEmpty(file_name)) file_name = "downloaded_file";

                    using var formData = new MultipartFormDataContent
                    {
                        { new StreamContent(stream), "file", file_name }
                    };

                    var file = await _client.SendPostRequestAsync<UploadFileResponseModel>(
                        $"{URL}/api/v3/upload_file",
                        formData,
                        new KeyValuePair<string, string>("api-access-key", _options.ApiAccessKey)
                    );

                    file_id = file.File_Id;

                    _memory.Set(file_path, file_id, TimeSpan.FromHours(24));
                }
            }

            var data = new
            {
                bot_id = _options.Bot_Id,
                phone_number = phone_no,
                message_data = new
                {
                    message = new
                    {
                        text = message,
                        file_id
                    }
                }
            };

            var response = await _client.SendPostRequestAsync<SendMessageResponseModel>(
                $"{URL}/api/v3/send_message",
                data,
                new KeyValuePair<string, string>("api-access-key", _options.ApiAccessKey),
                "application/json",
                true
            );

            if (response.Error_Data != null) ThrowExceptionIf.SendMessageResponseIsNotOk(
                response.Error_Data[0].Code,
                response.Error_Data[0].Description,
                JsonConvert.SerializeObject(data)
            );
        }
    }
}