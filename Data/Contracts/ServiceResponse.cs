namespace TJTech.APIs.ChatGTP.Data.Contracts;

public class ServiceResponse<T>
{
    public int StatusCode { get; set; } = 200;
    public T Data { get; set; }
    public List<Error>? Errors { get; set; }
    public bool HasErrors => Errors is not null && Errors.Count > 0;

    public ServiceResponse(T data, int statusCode = 200)
    {
        Data = data;
        StatusCode = statusCode;
        Errors = null;
    }

    public ServiceResponse(Error error, int statusCode = 400)
    {
        Errors = new List<Error>() { error };
        StatusCode = statusCode;
    }

    public ServiceResponse(List<Error> errors, int statusCode)
    {
        Errors = errors;
        StatusCode = StatusCode;
    }
}