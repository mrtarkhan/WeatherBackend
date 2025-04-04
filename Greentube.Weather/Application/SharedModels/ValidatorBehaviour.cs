using MediatR;

namespace Greentube.Weather.Application.SharedModels;

public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{

    private readonly IValidator<TRequest> _validator;

    public ValidatorBehaviour(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _validator.Validate(request);
        return await next(cancellationToken);
    }
}