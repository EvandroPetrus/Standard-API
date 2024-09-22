using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Standard_Solution.API.Filter;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
            return;
        }

        var parameters = context.ActionDescriptor.Parameters;
        foreach (var parameter in parameters)
        {
            var parameterValue = context.ActionArguments[parameter.Name];
            if (parameterValue is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                var validationContext = new ValidationContext<object>(parameterValue);
                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult);
                    return;
                }
            }
        }
        await next();
    }
}
