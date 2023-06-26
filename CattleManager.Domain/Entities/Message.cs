using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(1000)]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    public bool HasBeenRead { get; set; } = false;

    [ForeignKey("SenderId")]
    public User Sender { get; set; } = null!;
    public Guid SenderId { get; set; }

    [ForeignKey("ReceiverId")]
    public User Receiver { get; set; } = null!;
    public Guid ReceiverId { get; set; }
}