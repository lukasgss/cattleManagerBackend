using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.Entities.Messages;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class MessageServiceTests
{
    private readonly IMessageRepository _messageRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IGuidProvider _guidProviderMock;
    private readonly IDateTimeProvider _dateTimeProviderMock;
    private readonly IMessageService _sut;
    private static readonly Guid _senderId = Guid.NewGuid();
    private static readonly Guid _receiverId = Guid.NewGuid();

    public MessageServiceTests()
    {
        _messageRepositoryMock = A.Fake<IMessageRepository>();
        _userRepositoryMock = A.Fake<IUserRepository>();
        _guidProviderMock = A.Fake<IGuidProvider>();
        _dateTimeProviderMock = A.Fake<IDateTimeProvider>();
        _sut = new MessageService(
            _messageRepositoryMock,
            _userRepositoryMock,
            _guidProviderMock,
            _dateTimeProviderMock);
    }

    [Fact]
    public async Task Get_All_Messages_To_User_Returns_All_Messages()
    {
        A.CallTo(() => _messageRepositoryMock.GetAmountOfPages(_senderId, _receiverId)).Returns(1);
        List<Message> expectedMessages = new()
        {
            GenerateMessage()
        };
        A.CallTo(() => _messageRepositoryMock.GetAllMessagesToUser(_senderId, _receiverId, 1)).Returns(expectedMessages);
        PaginatedMessageResponse expectedResponse = new(expectedMessages, 1, 1);

        PaginatedMessageResponse response = await _sut.GetAllMessagesToUserAsync(_senderId, _receiverId, 1);
    }

    [Fact]
    public async Task Get_Message_By_Non_Existent_Id_Throws_NotFoundException()
    {
        Message? nullMessage = null;
        Guid messageId = Guid.NewGuid();
        A.CallTo(() => _messageRepositoryMock.GetMessageByIdAsync(messageId, _senderId)).Returns(nullMessage);

        async Task result() => await _sut.GetMessageByIdAsync(messageId, _senderId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Mensagem com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Message_By_Id_Returns_Message()
    {
        Guid messageId = Guid.NewGuid();
        Message message = GenerateMessage(messageId);
        A.CallTo(() => _messageRepositoryMock.GetMessageByIdAsync(messageId, _senderId)).Returns(message);
        MessageResponse expectedMessageResponse = GenerateMessageResponseFromMessage(message);

        MessageResponse messageResponse = await _sut.GetMessageByIdAsync(messageId, _senderId);

        Assert.Equivalent(expectedMessageResponse, messageResponse);
    }

    [Fact]
    public async Task Get_Amount_Of_Message_Notifications_From_Non_Existent_User_Throws_NotFoundException()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_receiverId)).Returns(nullUser);

        async Task result() => await _sut.GetAmountOfMessageNotificationsFromDistinctUsers(_receiverId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Usuário com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Amount_Of_Message_Notifications_Returns_Amount_Of_Notifications()
    {
        User user = new();
        const int amountOfNotifications = 5;
        A.CallTo(() => _messageRepositoryMock.GetAmountOfMessageNotifications(_receiverId)).Returns(amountOfNotifications);
        MessageNotificationAmount expectedMessageNotificationAmount = new()
        {
            Amount = amountOfNotifications
        };

        MessageNotificationAmount messageNotificationAmount = await _sut.GetAmountOfMessageNotificationsFromDistinctUsers(_receiverId);

        Assert.Equivalent(expectedMessageNotificationAmount, messageNotificationAmount);
    }

    [Fact]
    public async Task Mark_Messages_As_Read_From_Non_Existent_Sender_Throws_NotFoundException()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_senderId)).Returns(nullUser);

        async Task result() => await _sut.MarkMessagesAsRead(_receiverId, _senderId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Usuário com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Send_Message_With_Non_Existent_Receiver_Throws_NotFoundException()
    {
        User? nullReceiver = null;
        MessageRequest messageRequest = GenerateMessageRequest();
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_receiverId)).Returns(nullReceiver);

        async Task result() => await _sut.SendMessageAsync(messageRequest, _senderId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Destinatário com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Send_Message_With_Non_Existent_Sender_Throws_NotFoundException()
    {
        User receiver = new();
        MessageRequest messageRequest = GenerateMessageRequest();
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_receiverId)).Returns(receiver);
        User? nullSender = null;
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_senderId)).Returns(nullSender);

        async Task result() => await _sut.SendMessageAsync(messageRequest, _senderId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Remetente com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Send_Message_Returns_Sent_Message()
    {
        MessageRequest messageRequest = GenerateMessageRequest();
        User receiver = new() { Id = _receiverId };
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_receiverId)).Returns(receiver);
        User sender = new() { Id = _senderId };
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_senderId)).Returns(sender);
        DateTime date = DateTime.UtcNow;
        Message message = GenerateMessageFromMessageRequest(messageRequest, date);
        MessageResponse expectedMessageResponse = GenerateMessageResponseFromMessage(message);
        A.CallTo(() => _guidProviderMock.NewGuid()).Returns(expectedMessageResponse.Id);
        A.CallTo(() => _dateTimeProviderMock.UtcNow()).Returns(date);

        MessageResponse messageResponse = await _sut.SendMessageAsync(messageRequest, _senderId);

        Assert.Equivalent(expectedMessageResponse, messageResponse);
    }

    private static Message GenerateMessage(Guid? messageId = null)
    {
        return new Message()
        {
            Id = messageId ?? Guid.NewGuid(),
            Content = "Content",
            ReceiverId = _receiverId,
            SenderId = _senderId
        };
    }

    private static MessageResponse GenerateMessageResponseFromMessage(Message message)
    {
        return new MessageResponse(
            Id: message.Id,
            Content: message.Content,
            Date: message.Date,
            HasBeenRead: message.HasBeenRead,
            SenderId: message.SenderId,
            ReceiverId: message.ReceiverId);
    }

    private static MessageRequest GenerateMessageRequest()
    {
        return new MessageRequest(
            Content: "Content",
            ReceiverId: _receiverId);
    }

    private static Message GenerateMessageFromMessageRequest(MessageRequest messageRequest, DateTime date)
    {
        return new Message()
        {
            Content = messageRequest.Content,
            ReceiverId = messageRequest.ReceiverId,
            SenderId = _senderId,
            Date = date
        };
    }
}