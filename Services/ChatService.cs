using System.Security.Claims;
using OpenAI_API;
using OpenAI_API.Models;
using TJTech.APIs.ChatGTP.Data.Contracts;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Requests;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;
using TJTech.APIs.ChatGTP.Data.Dto;
using TJTech.APIs.ChatGTP.Data.Models;
using TJTech.APIs.ChatGTP.Data.Repositories;

namespace TJTech.APIs.ChatGTP.Services;

public class ChatService(
    ILogger<ChatService> logger,
    IConfiguration configuration,
    IChatRepository repository) : IChatService
{
    public async Task<ServiceResponse<List<ChatResponse>>> GetAsync(ClaimsPrincipal user)
    {
        try
        {
            var models = await repository.ReadAsync();
            var response = Transform.ToContracts(models);
            return new ServiceResponse<List<ChatResponse>>(response);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<List<ChatResponse>>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<ChatResponse>> GetAsync(ClaimsPrincipal user, Guid id)
    {
        try
        {
            var model = await repository.ReadAsync(id);
            if (model is null)
                return new ServiceResponse<ChatResponse>(new Error("id", $"Chat not found by id '{id}'."), 404);
            var response = Transform.ToContract(model);
            return new ServiceResponse<ChatResponse>(response);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<ChatResponse>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<List<ChatPromptResponse>>> GetPromptsAsync(ClaimsPrincipal user)
    {
        try
        {
            var models = await repository.ReadPromptsAsync();
            var response = Transform.PromptsToContracts(models);
            return new ServiceResponse<List<ChatPromptResponse>>(response);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<List<ChatPromptResponse>>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<TextResponse>> ValidateAsync(ClaimsPrincipal user, Guid id)
    {
        try
        {
            var userId = user.GetUerId() ?? throw new ArgumentNullException("User id missing from JWT.");
            var userName = user.GetFullName() ?? throw new ArgumentNullException("User first and last name are missing from JWT.");
            var existing = await repository.ReadAsync(id);
            if (existing is null)
                return new ServiceResponse<TextResponse>(new Error("id", $"Chat not found by id '{id}'."), 404);
            await repository.ValidateAsync(userId, userName, id);
            return new ServiceResponse<TextResponse>(new TextResponse("success"));
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<TextResponse>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<TextResponse>> ProcessAsync(ClaimsPrincipal user, Guid id)
    {
        try
        {
            var userId = user.GetUerId() ?? throw new ArgumentNullException("User id missing from JWT.");
            var userName = user.GetFullName() ?? throw new ArgumentNullException("User first and last name are missing from JWT.");
            var existing = await repository.ReadAsync(id);
            if (existing is null)
                return new ServiceResponse<TextResponse>(new Error("id", $"Chat not found by id '{id}'."), 404);
            await repository.ProcessAsync(userId, userName, id);
            return new ServiceResponse<TextResponse>(new TextResponse("success"));
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<TextResponse>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<TextResponse>> UpdateStageAsync(ClaimsPrincipal user, Guid id, short stage)
    {
        try
        {
            var userId = user.GetUerId() ?? throw new ArgumentNullException("User id missing from JWT.");
            var userName = user.GetFullName() ?? throw new ArgumentNullException("User first and last name are missing from JWT.");
            var existing = await repository.ReadAsync(id);
            if (existing is null)
                return new ServiceResponse<TextResponse>(new Error("id", $"Chat not found by id '{id}'."), 404);
            await repository.UpdateStageAsync(userId, userName, id, stage);
            return new ServiceResponse<TextResponse>(new TextResponse("success"));
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<TextResponse>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<TextResponse>> DeleteAsync(ClaimsPrincipal user, Guid id)
    {
        try
        {
            var userId = user.GetUerId() ?? throw new ArgumentNullException("User id missing from JWT.");
            var userName = user.GetFullName() ?? throw new ArgumentNullException("User first and last name are missing from JWT.");
            var existing = await repository.ReadAsync(id);
            if (existing is null)
                return new ServiceResponse<TextResponse>(new Error("id", $"Chat not found by id '{id}'."), 404);
            await repository.DeleteAsync(userId, userName, id);
            return new ServiceResponse<TextResponse>(new TextResponse("success"));
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<TextResponse>(new Error("request", e.Message), 500);
        }
    }

    public async Task<ServiceResponse<ChatResponse>> ChatAsync(ClaimsPrincipal user, ChatPayload payload)
    {
        try
        {
            var userId = user.GetUerId() ?? throw new ArgumentNullException("User id missing from JWT.");
            var userName = user.GetFullName() ?? throw new ArgumentNullException("User first and last name are missing from JWT.");
            var apiKey = configuration["ChatApiKey"] ??
                         throw new ArgumentException("No ChatApiKey was found in appsettings.json");
            bool hasError = false;
            string? chatResponse = null;
            ServiceResponse<ChatResponse>? serviceResponse = null;
            var model = new Chat()
            {
                prompt = payload.Prompt,
                input_text = payload.Text,
                created_by_id = userId,
                created_by_name = userName,
            };
            try
            {
                var api = new OpenAIAPI(apiKey);
                var chat = api.Chat.CreateConversation();
                chat.Model = Model.GPT4_Turbo;
                chat.RequestParameters.Temperature = 0;
                chat.AppendUserInput($"{payload.Prompt} {payload.Text}");
                chatResponse = await chat.GetResponseFromChatbotAsync();
            }
            catch (Exception e)
            {
                model.error_message = e.Message;
                serviceResponse = new ServiceResponse<ChatResponse>(new Error("request", e.Message), 500);
                hasError = true;
            }
            model.response_text = chatResponse;
            model.created_at = DateTime.Now;
            model = await repository.CreateAsync(model);
            if (model is null)
            {
                serviceResponse = new ServiceResponse<ChatResponse>(new Error("database", "An error occurred while saving the response to the database"), 500);
                hasError = true;
            }
            var response = Transform.ToContract(model);
            return hasError ? serviceResponse : new ServiceResponse<ChatResponse>(response);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return new ServiceResponse<ChatResponse>(new Error("request", e.Message), 500);
        }
    }
}