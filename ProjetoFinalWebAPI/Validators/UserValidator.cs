using Requests;
using Responses;
using System.Collections.Generic;
using FinalProj;
using System.Text.RegularExpressions;

namespace Validators;

public class UserValidator : IValidator<BaseUserRequest>
{
    public List<ErrorMessageResponse> Validate(BaseUserRequest user)
    {
        var errors = new List<ErrorMessageResponse>();

        if (string.IsNullOrEmpty(user.Name))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Name",
                Message = "Field is required!"
            });

        if (string.IsNullOrEmpty(user.Password))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Password",
                Message = "Field is required!"
            });

        if (user.Admin == true && user.Name == "Admin")
            errors.Add(new ErrorMessageResponse
            {
                Field = "Admin",
                Message = "Cannot update/delete an Admin user!"
            });

        if (string.IsNullOrEmpty(user.Email))
        {
            errors.Add(new ErrorMessageResponse() { Field = "email", Message = "E-mail cannot be empty!" });
        }
        else
        {
            string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(user.Email, emailPattern))
            {
                errors.Add(new ErrorMessageResponse() { Field = "email", Message = "Invalid Email" });
            }
        }

        return errors;
    }
}
