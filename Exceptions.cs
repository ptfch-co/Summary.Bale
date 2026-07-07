using System;

namespace Summary.Bale
{
    using Core.Workflows;

    public static class ThrowExceptionIf
    {
        public static void MobileIsNotValid(string mobile)
        {
            if (String.IsNullOrWhiteSpace(mobile)) throw new ObjectDoesNotExist();
            if (mobile.IsMobileNumber() is false) throw new ObjectDoesNotExist();
        }

        public static void SendMessageResponseIsNotOk(
            Error_Code code,
            string error_message,
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
                    error_message,
                    null,
                    data,
                    "کارشناس پشتیبانی؛ در صورت مشاهده این خطاء تیکت را به سطح بعدی ارجاع دهید.",
                    Level.Error
                );

                default: throw new NotImplementedException();
            }
        }
    }
}