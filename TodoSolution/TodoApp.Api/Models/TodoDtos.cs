using TodoApp.Domain.Entities;

namespace TodoApp.Api.Models;

public record CreateTodoRequest(string Title, string? Description, TodoPriority Priority);
public record UpdateTodoRequest(string Title, string? Description, TodoPriority Priority, bool IsCompleted);
public record TodoResponse(
    int Id, 
    string Title, 
    string? Description, 
    bool IsCompleted, 
    TodoPriority Priority, 
    DateTime CreatedAt, 
    DateTime? CompletedAt);