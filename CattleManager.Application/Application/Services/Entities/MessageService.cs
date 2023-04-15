using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.Entities.Messages;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MessageService(
        IMessageRepository messageRepository,
        IUserRepository userRepository,
        IGuidProvider guidProvider,
        IDateTimeProvider dateTimeProvider)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _guidProvider = guidProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaginatedMessageResponse> GetAllMessagesToUserAsync(Guid senderId, Guid receiverId, int page)
    {
        double amountOfPages = _messageRepository.GetAmountOfPages(senderId, receiverId);

        if ((page > amountOfPages && amountOfPages > 0) || page < 1)
            throw new BadRequestException($"Resultado possui {amountOfPages} página(s), insira um valor entre 1 e o número de páginas.");

        IEnumerable<Message> messages = await _messageRepository.GetAllMessagesToUser(senderId, receiverId, page);

        return new PaginatedMessageResponse(messages, page, amountOfPages);
    }

    public async Task<MessageResponse> GetMessageByIdAsync(Guid messageId, Guid userId)
    {
        Message? message = await _messageRepository.GetMessageByIdAsync(messageId, userId);
        if (message is null)
            throw new NotFoundException("Mensagem com o id especificado não existe.");

        return new MessageResponse(
            Id: message.Id,
            Content: message.Content,
            Date: message.Date,
            HasBeenRead: message.HasBeenRead,
            SenderId: message.SenderId,
            ReceiverId: message.ReceiverId);
    }

    public async Task<MessageResponse> SendMessageAsync(MessageRequest messageRequest, Guid senderId)
    {
        if (senderId != messageRequest.SenderId)
            throw new UnauthorizedException("Você não possui autorização para enviar essa mensagem com o remetente especificado.");

        User? receiver = await _userRepository.GetByIdAsync(messageRequest.ReceiverId);
        if (receiver is null)
            throw new NotFoundException("Destinatário com o id especificado não existe.");

        User? sender = await _userRepository.GetByIdAsync(messageRequest.SenderId);
        if (sender is null)
            throw new NotFoundException("Remetente com o id especificado não existe.");

        Message message = new()
        {
            Id = _guidProvider.NewGuid(),
            Content = messageRequest.Content,
            Receiver = receiver,
            Sender = sender,
            Date = _dateTimeProvider.UtcNow()
        };
        _messageRepository.Add(message);
        await _messageRepository.CommitAsync();

        return new MessageResponse(
            Id: message.Id,
            Content: message.Content,
            Date: message.Date,
            HasBeenRead: message.HasBeenRead,
            SenderId: sender.Id,
            ReceiverId: receiver.Id);
    }
}