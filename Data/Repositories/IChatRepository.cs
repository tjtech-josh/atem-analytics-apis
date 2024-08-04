using TJTech.APIs.ChatGTP.Data.Models;

namespace TJTech.APIs.ChatGTP.Data.Repositories;

public interface IChatRepository
{
    Task<List<Chat>> ReadAsync(string userId = "", int limit = 1000, int offset = 0);
    Task<Chat?> ReadAsync(Guid id);
    Task<List<Chat>> ReadPromptsAsync();
    Task<Chat?> CreateAsync(Chat model);
    Task ValidateAsync(string userId, string userName, Guid id);
    Task ProcessAsync(string userId, string userName, Guid id);
    Task UpdateStageAsync(string userId, string userName, Guid id, short stage = 0);
    Task DeleteAsync(string userId, string userName, Guid id);
}