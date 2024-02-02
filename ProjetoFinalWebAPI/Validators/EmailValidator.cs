using Responses;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Validators
{
    public class EmailValidator : IValidator<string>
    {
        public List<ErrorMessageResponse> Validate(string email)
        {
            List<ErrorMessageResponse> errors = new List<ErrorMessageResponse>();

            if (string.IsNullOrEmpty(email))
            {
                errors.Add(new ErrorMessageResponse() { Field = "email", Message = "E-mail cannot be empty!"});
            }
            else
            {
                string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    errors.Add(new ErrorMessageResponse() { Field = "email", Message = "E-mail cannot be empty!" });
                }
            }

            return errors;
        }
    }
}
