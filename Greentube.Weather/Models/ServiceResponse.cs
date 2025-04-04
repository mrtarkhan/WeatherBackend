namespace Greentube.Weather.Models;

public class ServiceResponse<T> where T : class
{
    public T Data { get; set; }
    public string Message { get; set; }
    public int Status { get; set; }
    
    public static ServiceResponse<T> Ok(T data)
    {
        return new ServiceResponse<T>()
        {
            Data = data,
            Message = "request processed successfully",
            Status = 200
        };
    }
    
    public static ServiceResponse<string> Error(string message)
    {
        return new ServiceResponse<string>
        {
            Data = string.Empty,
            Message = message,
            Status = 400
        };
    }
}