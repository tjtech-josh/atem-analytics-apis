using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TJTech.APIs.ChatGTP.Data.Contracts.Http.Requests;
using TJTech.APIs.ChatGTP.Services;

namespace TJTech.APIs.ChatGTP.Controllers;

[Authorize]
[Route("chat")]
[ApiController]
public class ChatController(
    ILogger<ChatController> logger, 
    IChatService service) : ControllerBase
{
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var response = await service.GetAsync(User);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
    
    [Route("{id:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        try
        {
            var response = await service.GetAsync(User, id);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
    
    [Route("{id:guid}/validate")]
    [HttpPatch]
    public async Task<IActionResult> ValidateAsync([FromRoute] Guid id)
    {
        try
        {
            var response = await service.ValidateAsync(User, id);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
    
    [Route("{id:guid}/process")]
    [HttpPatch]
    public async Task<IActionResult> ProcessAsync([FromRoute] Guid id)
    {
        try
        {
            var response = await service.ProcessAsync(User, id);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
    
    [Route("{id:guid}/update-stage/{stage:int}")]
    [HttpPatch]
    public async Task<IActionResult> UpdateStageAsync([FromRoute] Guid id, [FromRoute] short stage)
    {
        try
        {
            var response = await service.UpdateStageAsync(User, id, stage);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
    
    [Route("{id:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        try
        {
            var response = await service.DeleteAsync(User, id);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }

    [Route("chat")]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ChatPayload payload)
    {
        try
        {
            var response = await service.ChatAsync(User, payload);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
    
    [Route("chat/prompts")]
    [HttpGet]
    public async Task<IActionResult> GetPromptsAsync()
    {
        try
        {
            var response = await service.GetPromptsAsync(User);
            return StatusCode(response.StatusCode, response.HasErrors ? response.Errors : response.Data);
        }
        catch (Exception e)
        {
            logger.LogError("{e}", e);
            return StatusCode(500, e.Message);
        }
    }
}