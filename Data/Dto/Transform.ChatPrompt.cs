using System.Collections.Concurrent;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;
using TJTech.APIs.ChatGTP.Data.Models;

namespace TJTech.APIs.ChatGTP.Data.Dto;

public static partial class Transform
{
    public static List<ChatPromptResponse> PromptsToContracts(List<Chat> models)
    {
        var contracts = new ConcurrentBag<ChatPromptResponse>();
        Parallel.ForEach(models, (model) =>
        {
            var contract = PromptToContract(model);
            if (contract is not null)
                contracts.Add(contract);
        });
        return contracts.ToList();
    }

    public static ChatPromptResponse? PromptToContract(Chat? model) => model is not null
        ? new()
        {
            Id = model.id,
            Prompt = model.prompt,
            InputText = model.input_text,
            CreatedById = model.created_by_id,
            CreatedByName = model.created_by_name,
            CreatedAt = model.created_at
        }
        : null;
}