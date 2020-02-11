using IRBusDotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRBusDotNet.Helpers
{
    public static class Extentions
    {
        public static string ToCode(this string statusCode)
        {
            switch (statusCode.Trim())
            {
                case "Pending":
                    return "بلیط در وضعیت انتظار قرار دارد";
                case "Issued":
                    return "بلیط شما صادر شده است";
                case "Refunded":
                    return "بلیط شما پس داده شده است";
                case "Failed":
                    return "بلیط شما ناموفق میباشد";
                case "PaymentUnsuccessful":
                    return "پرداخت بلیط ناموفق میباشد";
                case "Canceled":
                    return "بلیط شما کنسل شده است";
                case "Rejected":
                    return "بلیط شما رد شده است";
                case "Refundable":
                    return "بلیط شما قابل پس دادن میباشد";
                case "Nonrefundable":
                    return "بلیط شما قابل پس دادن نمیباشد";
                case "BadArgument":
                    return "";
                case "InvalidTicketStatus":
                    return "وضعیت بلیط مشخص نیست";
                case "RefundRequestIsRejected":
                    return "کنسلی بلیط رد شد";
                case "RefundDateExpired":
                    return "زمان کنسلی گذشته است";
                case "SeatUnavailable":
                    return "صندلی غیر قابل خرید";
                case "InvalidBankAccountNumber":
                    return "مشکل بانکی";
                case "InvalidBusServiceStatus":
                    return "مشکل در وضعیت اتبوبس";
                case "InsufficientCredit":
                    return "اعتبار ناکافی";
                case "InvalidDiscount":
                    return "تخفیف نامعتبر";
                case "TicketWasRefunded":
                    return "بلیط قبلا استرداد شده است";
                case "BookRequestIsRejected":
                    return "شرکت مسافربری درخواست خرید بیلط را رد کرده است";
                case "InvalidPaymentMethod":
                    return "نحوه پرداخت معتبر نمیباشد";
                case "InvalidTraceNumber":
                    return "کد پیگیری معتبر نمیباشد";
                case "ReserveDisabled":
                    return "صندلی مورد نظر موقتا غیرقابل خرید میباشد";
                case "ValidationFailed":
                    return "یک یا چند پارامتر ورودی معتبر نمیباشد";
                case "unsupported_grant_type":
                    return "یوزر و پسورد اشتباه";
            }
            return "خطا";
        }
        public static string ToUrl(this string url, string secondUrl = "")
        {
            return url + secondUrl;
        }

        public static string ToError(this BusErrorResult busErrorResult)
        {
            if (!string.IsNullOrEmpty(busErrorResult.Message))
            {
                return busErrorResult.Message.ToCode();
            }
            if (!string.IsNullOrEmpty(busErrorResult.Error))
            {
                return busErrorResult.Error.ToCode();
            }
            return "خطای نامشخص";

        }
    }
}
