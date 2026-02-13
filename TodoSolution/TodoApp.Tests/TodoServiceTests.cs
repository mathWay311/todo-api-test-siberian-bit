using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApp.Api.Models;
using TodoApp.Api.Services;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure;
using Xunit;

namespace TodoApp.Tests;

public class TodoServiceTests
{
    private TodoDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new TodoDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnValidResponse_WhenDataIsCorrect()
    {
        var context = CreateContext();
        var loggerMock = new Mock<ILogger<TodoService>>();
        var service = new TodoService(context, loggerMock.Object);
        var request = new CreateTodoRequest("Buy crypto", "Buy some BTC", TodoPriority.High);

        var result = await service.CreateAsync(request);

        Assert.Equal(request.Title, result.Title);
        Assert.Equal(TodoPriority.High, result.Priority);
        Assert.False(result.IsCompleted);
        Assert.NotEqual(default, result.CreatedAt);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResults()
    {
        var context = CreateContext();
        var loggerMock = new Mock<ILogger<TodoService>>();
        var service = new TodoService(context, loggerMock.Object);

        for (int i = 1; i <= 15; i++)
        {
            context.Todos.Add(new TodoItem { Title = $"Task {i}", Priority = TodoPriority.Low });
        }
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync(null, null, page: 2, pageSize: 5);

        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task CompleteAsync_ShouldSetTimestamp_WhenCompleted()
    {
        var context = CreateContext();
        var loggerMock = new Mock<ILogger<TodoService>>();
        var service = new TodoService(context, loggerMock.Object);
        
        var item = new TodoItem { Title = "Test Task" };
        context.Todos.Add(item);
        await context.SaveChangesAsync();

        var result = await service.CompleteAsync(item.Id);

        Assert.True(result!.IsCompleted);
        Assert.NotNull(result.CompletedAt);
    }

    [Fact]
    public async Task BulkCompleteAsync_ShouldUpdateOnlyRequestedItems()
    {
        var context = CreateContext();
        var loggerMock = new Mock<ILogger<TodoService>>();
        var service = new TodoService(context, loggerMock.Object);

        context.Todos.AddRange(
            new TodoItem { Id = 1, Title = "Task 1" },
            new TodoItem { Id = 2, Title = "Task 2" },
            new TodoItem { Id = 3, Title = "Task 3" }
        );
        await context.SaveChangesAsync();

        var updatedCount = await service.BulkCompleteAsync(new[] { 1, 2 });

        Assert.Equal(2, updatedCount);
        var item3 = await context.Todos.FindAsync(3);
        Assert.False(item3!.IsCompleted);
    }
}