namespace TodoApp.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public TodoPriority Priority { get; set; } = TodoPriority.Medium;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public void MarkAsCompleted()
    {
        if (IsCompleted) return;
        
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
}