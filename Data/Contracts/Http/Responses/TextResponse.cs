namespace TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;

public class TextResponse
{
    public string Message { get; set; }

    public TextResponse(string message)
    {
        Message = message;
    }
}