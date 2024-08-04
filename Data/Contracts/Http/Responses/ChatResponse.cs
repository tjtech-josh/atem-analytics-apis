namespace TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;

public class ChatResponse
{
    public Guid Id { get; set; }
    public string Prompt { get; set; }
    public string? InputText { get; set; }
    public string? ResponseText { get; set; }
    public string? ErrorMessage { get; set; }
    public short CurrentStage { get; set; }
    public DateTime? StageUpdatedAt { get; set; }
    public string? StageUpdatedById { get; set; }
    public string? StageUpdatedByName { get; set; }
    public short Validated { get; set; }
    public DateTime? LastValidatedAt { get; set; }
    public string? LastValidatedById { get; set; }
    public string? LastValidatedByName { get; set; }
    public short Processed { get; set; }
    public DateTime? LastProcessedAt { get; set; }
    public string? LastProcessedById { get; set; }
    public string? LastProcessedByName { get; set; }
    public short Deleted { get; set; } 
    public DateTime? DeletedAt { get; set; }
    public string? DeletedById { get; set; }
    public string? DeletedByName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public string CreatedByName { get; set; }
}