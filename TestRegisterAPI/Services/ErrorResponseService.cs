using TestRegisterAPI.Model;
using TestRegisterAPI.ViewModel;

namespace TestRegisterAPI.Services
{
    public class ErrorResponseService
    {
        public CustomerErrorResponseViewModel CreateErrorResponse(int status, string error)
        {
            return new CustomerErrorResponseViewModel
            {
                Status = status,
                Error = error,
                Data = null
            };
        }
    }
}
