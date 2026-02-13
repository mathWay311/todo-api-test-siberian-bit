using TodoApp.Api.Models;
using TodoApp.Domain.Entities;

namespace TodoApp.Api.Mappings;

public static class TodoMappingExtensions
{
    public static TodoResponse ToResponse(this TodoItem item)
    {
        return new TodoResponse(
            item.Id,
            item.Title,
            item.Description,
            item.IsCompleted,
            item.Priority,
            item.CreatedAt,
            item.CompletedAt
        );
    }
}