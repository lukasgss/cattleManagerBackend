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

    [HttpGet("user/{senderId:guid}")]
    public async Task<ActionResult<PaginatedMessageResponse>> GetAllMessagesToUser(Guid senderId, int page = 1)
    {
        Guid receiverId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        PaginatedMessageResponse messages = await _messageService.GetAllMessagesToUserAsync(receiverId, senderId, page);
        return Ok(messages);
    }

    [HttpGet("{messageId:guid}", Name = "GetMessageById")]
    public async Task<ActionResult<MessageResponse>> GetMessageById(Guid messageId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MessageResponse messageResponse = await _messageService.GetMessageByIdAsync(messageId, userId);
        return Ok(messageResponse);
    }

    [HttpGet("notifications")]
    public async Task<ActionResult<MessageNotificationAmount>> GetAmountOfMessageNotifications()
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MessageNotificationAmount notificationAmount = await _messageService.GetAmountOfMessageNotificationsFromDistinctUsers(userId);
        return Ok(notificationAmount);
    }

    [HttpGet("read/{senderId:guid}")]
    public async Task<ActionResult> ReadMessage(Guid senderId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _messageService.MarkMessagesAsRead(userId, senderId);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> SendMessage(MessageRequest messageRequest)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MessageResponse message = await _messageService.SendMessageAsync(messageRequest, userId);
        return new CreatedAtRouteResult(nameof(GetMessageById), new { MessageId = message.Id }, message);
    }
}