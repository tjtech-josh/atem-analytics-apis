namespace TJTech.APIs.ChatGTP.Data.Contracts.Http.Requests;

public class UpdatePayload
{
    public ChatAction Action { get; set; }
    public List<Guid>? Ids { get; set; }
}