namespace Greentube.Weather.Application.SharedModels;

public interface IValidator<T> where T : class
{
    void Validate(T inputModel);
}