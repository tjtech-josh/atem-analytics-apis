namespace TJTech.APIs.ChatGTP.Data.Contracts;

public class Error
{
    public string Field { get; set; }
    public string Message { get; set; }

    public Error(string field, string message)
    {
        Field = field;
        Message = message;
    }
}