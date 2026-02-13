using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Mappings;
using TodoApp.Api.Models;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure;

namespace TodoApp.Api.Services;

public class TodoService(TodoDbContext context, ILogger<TodoService> logger) : ITodoService
{
    public async Task<IEnumerable<TodoResponse>> GetAllAsync(bool? isCompleted, TodoPriority? priority, int page, int pageSize)
    {
        IQueryable<TodoItem> query = context.Todos.AsNoTracking();

        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        var items = await query
            .OrderByDescending(t => t.CreatedAt) // Хороший тон - свежие сверху
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return items.Select(t => t.ToResponse());
    }

    public async Task<TodoResponse?> GetByIdAsync(int id)
    {
        var item = await context.Todos.FindAsync(id);
        return item?.ToResponse();
    }

    public async Task<TodoResponse> CreateAsync(CreateTodoRequest request)
    {
        var item = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority
        };

        context.Todos.Add(item);
        await context.SaveChangesAsync();
        
        logger.LogInformation("Создана новая задача с ID: {Id}", item.Id);
        return item.ToResponse();
    }

    public async Task<TodoResponse?> CompleteAsync(int id)
    {
        var item = await context.Todos.FindAsync(id);
        if (item == null) return null;

        item.MarkAsCompleted();
        await context.SaveChangesAsync();
        
        return item.ToResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateTodoRequest request)
    {
        var item = await context.Todos.FindAsync(id);
        if (item == null) return false;

        item.Title = request.Title;
        item.Description = request.Description;
        item.Priority = request.Priority;
        item.IsCompleted = request.IsCompleted;
        
        if (item.IsCompleted && item.CompletedAt == null)
            item.CompletedAt = DateTime.UtcNow;
        else if (!item.IsCompleted)
            item.CompletedAt = null;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.Todos.FindAsync(id);
        if (item == null) return false;

        context.Todos.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<int> BulkCompleteAsync(IEnumerable<int> ids)
{
        var items = await context.Todos
            .Where(t => ids.Contains(t.Id) && !t.IsCompleted)
            .ToListAsync();

        foreach (var item in items)
        {
            item.MarkAsCompleted();
        }

        await context.SaveChangesAsync();
        return items.Count;
    }
}