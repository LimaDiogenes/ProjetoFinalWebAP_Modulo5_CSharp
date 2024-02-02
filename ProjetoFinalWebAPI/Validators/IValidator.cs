using Responses;
using System.Collections.Generic;

namespace Validators;

public interface IValidator<T>
{
    public List<ErrorMessageResponse> Validate(T obj);
}
