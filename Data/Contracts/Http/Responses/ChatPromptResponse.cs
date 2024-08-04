namespace TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;

public class ChatPromptResponse
{
    public Guid Id { get; set; }
    public string Prompt { get; set; }
    public string? InputText { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public string CreatedByName { get; set; }
}