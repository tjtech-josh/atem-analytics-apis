using System.Collections.Concurrent;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;
using TJTech.APIs.ChatGTP.Data.Models;

namespace TJTech.APIs.ChatGTP.Data.Dto;

public static partial class Transform
{
    public static List<ChatResponse> ToContracts(List<Chat> models)
    {
        var contracts = new ConcurrentBag<ChatResponse>();
        Parallel.ForEach(models, (model) =>
        {
            var contract = ToContract(model);
            if (contract is not null)
                contracts.Add(contract);
        });
        return contracts.ToList();
    }

    public static ChatResponse? ToContract(Chat? model) => model is not null
        ? new()
        {
            Id = model.id,
            Prompt = model.prompt,
            InputText = model.input_text,
            CreatedById = model.created_by_id,
            CreatedByName = model.created_by_name,
            CreatedAt = model.created_at,
            Deleted = model.deleted,
            Processed = model.processed,
            ResponseText = model.response_text,
            ErrorMessage = model.error_message,
            Validated = model.validated,
            DeletedAt = model.deleted_at,
            DeletedById = model.deleted_by_id,
            DeletedByName = model.deleted_by_name,
            LastProcessedAt = model.last_processed_at,
            LastProcessedById = model.last_processed_by_id,
            LastProcessedByName = model.last_processed_by_name,
            LastValidatedAt = model.last_validated_at,
            LastValidatedById = model.last_validated_by_name,
            LastValidatedByName = model.last_validated_by_name,
            CurrentStage = model.current_stage,
            StageUpdatedById = model.stage_updated_by_id,
            StageUpdatedByName = model.stage_updated_by_name,
            StageUpdatedAt = model.stage_updated_at
        }
        : null;
}