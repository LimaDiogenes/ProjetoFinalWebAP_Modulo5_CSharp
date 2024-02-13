using Requests;
using Responses;
using System.Collections.Generic;
using FinalProj;
using System.Text.RegularExpressions;

namespace Validators;

public class ItemValidator : IValidator<BaseItemRequest>
{
    public List<ErrorMessageResponse> Validate(BaseItemRequest item)
    {
        var errors = new List<ErrorMessageResponse>();

        if (string.IsNullOrEmpty(item.Name))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Name",
                Message = "Field is required!"
            });

        if (string.IsNullOrEmpty(item.Category))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Category",
                Message = "Field is required"
            });
        
        if (string.IsNullOrEmpty(item.Variant))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Variant",
                Message = "Field is required"
            });

        if (string.IsNullOrEmpty(item.Size))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Size",
                Message = "Field is required"
            });

        if (double.IsNaN(item.Price))
            errors.Add(new ErrorMessageResponse
            {
                Field = "Price",
                Message = "Field must be a numeric value"
            });

        if (item.Price <= 0)
            errors.Add(new ErrorMessageResponse
            {
                Field = "Price",
                Message = "Field must be a positive numeric value"
            });


        return errors;
    }
}
