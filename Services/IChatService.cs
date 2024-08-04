using System.Security.Claims;
using TJTech.APIs.ChatGTP.Data.Contracts;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Requests;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Responses;

namespace TJTech.APIs.ChatGTP.Services;

public interface IChatService
{
    Task<ServiceResponse<List<ChatResponse>>> GetAsync(ClaimsPrincipal user);
    Task<ServiceResponse<ChatResponse>> GetAsync(ClaimsPrincipal user, Guid id);
    Task<ServiceResponse<List<ChatPromptResponse>>> GetPromptsAsync(ClaimsPrincipal user);
    Task<ServiceResponse<TextResponse>> ValidateAsync(ClaimsPrincipal user, Guid id);
    Task<ServiceResponse<TextResponse>> ProcessAsync(ClaimsPrincipal user, Guid id);
    Task<ServiceResponse<TextResponse>> UpdateStageAsync(ClaimsPrincipal user, Guid id, short stage);
    Task<ServiceResponse<TextResponse>> DeleteAsync(ClaimsPrincipal user, Guid id);
    Task<ServiceResponse<ChatResponse>> ChatAsync(ClaimsPrincipal user, ChatPayload payload);
}