using System;


namespace Elmah.Everywhere
{
    public interface IErrorService
    {
        bool ValidateToken(string token);
        bool ValidateErrorInfo(ErrorInfo model);
    }

    public class ErrorService : IErrorService
    {
        public bool ValidateToken(string token)
        {
            // TODO: Validate token here

            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }
            return true;
        }

        public bool ValidateErrorInfo(ErrorInfo model)
        {
            return true;
        }
    }
}
