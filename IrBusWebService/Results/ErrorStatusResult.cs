
namespace IrBusWebService.Results
{
    public abstract class ErrorStatusResult
    {
        public bool Status { get; set; }
        public string Error { get; set; } = "بدون خطا";
        public string ErrorDescription { get; set; } = "بدون توضیح";
    }
}
