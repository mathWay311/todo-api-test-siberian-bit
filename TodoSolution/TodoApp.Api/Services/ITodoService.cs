using TodoApp.Api.Models;

namespace TodoApp.Api.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoResponse>> GetAllAsync(bool? isCompleted, TodoApp.Domain.Entities.TodoPriority? priority);
    Task<TodoResponse?> GetByIdAsync(int id);
    Task<TodoResponse> CreateAsync(CreateTodoRequest request);
    Task<bool> UpdateAsync(int id, UpdateTodoRequest request);
    Task<bool> DeleteAsync(int id);
    Task<TodoResponse?> CompleteAsync(int id);
}