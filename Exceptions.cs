using Core.Workflows;
using Core.Mvc.Core;
using System;
using Core.Mvc.Utilities;

namespace Summary.Bale
{
    public static class ThrowExceptionIf
    {
        public static void MobileIsNotValid(string mobile)
        {
            if (String.IsNullOrWhiteSpace(mobile)) throw new ObjectDoesNotExist();
            if (mobile.IsMobileNo() is false) throw new ObjectDoesNotExist();
        }

        public static void SendSafirMessageResponseIsNotOk(
            Error_Code code,
            string message,
            string data)
        {
            switch (code)
            {
                case Error_Code.NotBaleUser: throw new ObjectDoesNotExist();

                case Error_Code.InvalidInput:
                case Error_Code.InternalServerError:
                case Error_Code.InvalidPhone:
                case Error_Code.PaymentRequired:
                case Error_Code.RateLimitExceeded: throw new WorkflowException(
                    message,
                    null,
                    data,
                    "کارشناس پشتیبانی؛ در صورت مشاهده این خطاء تیکت را به سطح بعدی ارجاع دهید.",
                    Level.Error
                );

                default: throw new NotImplementedException();
            }
        }

        public static void SendBotMessageResponseIsNotOk(
            Bot_Error_Code code,
            string message,
            string data)
        {
            throw new WorkflowException(
                message,
                null,
                data,
                "کارشناس پشتیبانی؛ در صورت مشاهده این خطاء تیکت را به سطح بعدی ارجاع دهید.",
                Level.Error
            );
        }
    }
}