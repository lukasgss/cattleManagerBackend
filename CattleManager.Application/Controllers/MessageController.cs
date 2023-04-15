using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/message")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public MessageController(IMessageService messageService, IUserAuthorizationService userAuthorizationService)
    {
        _messageService = messageService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("user/{receiverId:guid}")]
    public async Task<ActionResult<PaginatedMessageResponse>> GetAllMessagesToUser(Guid receiverId, int page)
    {
        string senderId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        PaginatedMessageResponse messages = await _messageService.GetAllMessagesToUserAsync(new Guid(senderId), receiverId, page);
        return Ok(messages);
    }

    [HttpGet("{messageId:guid}", Name = "GetMessageById")]
    public async Task<ActionResult<MessageResponse>> GetMessageById(Guid messageId)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MessageResponse messageResponse = await _messageService.GetMessageByIdAsync(messageId, new Guid(userId));
        return Ok(messageResponse);
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> SendMessage(MessageRequest messageRequest)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MessageResponse message = await _messageService.SendMessageAsync(messageRequest, new Guid(userId));
        return new CreatedAtRouteResult(nameof(GetMessageById), new { MessageId = message.Id }, message);
    }
}