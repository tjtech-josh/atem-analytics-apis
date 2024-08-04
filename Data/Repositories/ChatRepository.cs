using Dapper;
using Npgsql;
using TJTech.APIs.ChatGTP.Data.Models;

namespace TJTech.APIs.ChatGTP.Data.Repositories;

public class ChatRepository(
    ILogger<ChatRepository> logger,
    IConfiguration configuration) : IChatRepository
{
    private string SqlChatsCreate =>
        "select * from public.chats_create(@prompt, @input_text, @response_text, @error_message, @created_by_id, @created_by_name);";
    private string SqlChatsReadById => "select * from chats_read_by_id(@id)";
    private string SqlChatsRead => "select * from public.chats_read()";
    private string SqlChatsUpdateValidated => "select * from public.chats_validate(@id, @userId, @userName);";
    private string SqlChatsUpdateProcessed => "select * from public.chats_process(@id, @userId, @userName);";
    private string SqlChatsUpdateStage => "select * from public.chats_update_stage(@id, @stage, @userId, @userName);";
    private string SqlChatsDeleted => "select * from public.chats_delete(@id, @userId, @userName);";
    private string SqlChatsReadPrompts => "select distinct id, prompt, input_text, created_at, created_by_id, created_by_name from public.chats;";
    
    public async Task<List<Chat>> ReadAsync(string userId, int limit = 1000, int offset = 0)
    {
        try
        {
            var parameters = new
            {
                
            };
            var connection = Connection();
            return (await connection.QueryAsync<Chat>(SqlChatsRead, parameters, commandTimeout: 6000)).ToList();
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return [];
        }
    }

    public async Task<Chat?> ReadAsync(Guid chatId)
    {
        try
        {
            var connection = Connection();
            var res = await connection.QueryFirstOrDefaultAsync<Chat>(SqlChatsReadById, new { id = chatId }, commandTimeout: 6000);
            return res;
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return null;
        }
    }

    public async Task<List<Chat>> ReadPromptsAsync()
    {
        try
        {
            var connection = Connection();
            return (await connection.QueryAsync<Chat>(SqlChatsReadPrompts, commandTimeout: 6000))
                .ToList();
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return null;
        }
    }

    public async Task<Chat?> CreateAsync(Chat model)
    {
        try
        {
            var connection = Connection();
            var id = await connection.QueryFirstOrDefaultAsync<Guid>(SqlChatsCreate, model, commandTimeout: 6000);
            return await connection.QueryFirstOrDefaultAsync<Chat>(SqlChatsReadById, new { @id }, commandTimeout: 6000);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return null;
        }
    }

    public async Task ValidateAsync(string userId, string userName, Guid id)
    {
        try
        {
            var parameters = new
            {
                userId,
                userName,
                id
            };
            var connection = Connection();
            await connection.ExecuteAsync(SqlChatsUpdateValidated, parameters, commandTimeout: 6000);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
        }
    }

    public async Task ProcessAsync(string userId, string userName, Guid id)
    {
        try
        {
            var parameters = new
            {
                userId,
                userName,
                id
            };
            var connection = Connection();
            await connection.ExecuteAsync(SqlChatsUpdateProcessed, parameters, commandTimeout: 6000);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
        }
    }

    public async Task UpdateStageAsync(string userId, string userName, Guid id, short stage = 0)
    {
        try
        {
            var parameters = new
            {
                userId,
                userName,
                id,
                stage
            };
            var connection = Connection();
            await connection.ExecuteAsync(SqlChatsUpdateStage, parameters, commandTimeout: 6000);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
        }
    }

    public async Task DeleteAsync(string userId, string userName, Guid id)
    {
        try
        {
            var parameters = new
            {
                userId,
                userName,
                id
            };
            var connection = Connection();
            await connection.ExecuteAsync(SqlChatsDeleted, parameters, commandTimeout: 6000);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
        }
    }

    private NpgsqlConnection Connection()
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.Host = configuration["CockroachDb:Host"] ?? throw new ArgumentNullException("CockroachDb:Host");
        connStringBuilder.Port = int.Parse(configuration["CockroachDb:Port"]);
        connStringBuilder.SslMode = SslMode.VerifyFull;
        connStringBuilder.Username = configuration["CockroachDb:Username"] ?? throw new ArgumentNullException("CockroachDb:Username");
        connStringBuilder.Password = configuration["CockroachDb:Password"] ?? throw new ArgumentNullException("CockroachDb:Password");
        connStringBuilder.Database = configuration["CockroachDb:Database"] ?? throw new ArgumentNullException("CockroachDb:Database");
        return new NpgsqlConnection(connStringBuilder.ConnectionString);
    }
}