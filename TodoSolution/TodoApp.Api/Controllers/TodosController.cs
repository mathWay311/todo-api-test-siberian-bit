using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Models;
using TodoApp.Api.Services;
using TodoApp.Domain.Entities;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController(ITodoService todoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoResponse>>> GetAll([FromQuery] bool? isCompleted, [FromQuery] TodoPriority? priority)
    {
        var result = await todoService.GetAllAsync(isCompleted, priority);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoResponse>> GetById(int id)
    {
        var todo = await todoService.GetByIdAsync(id);
        return todo != null ? Ok(todo) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<TodoResponse>> Create(CreateTodoRequest request)
    {
        var result = await todoService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTodoRequest request)
    {
        var updated = await todoService.UpdateAsync(id, request);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await todoService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult<TodoResponse>> Complete(int id)
    {
        var result = await todoService.CompleteAsync(id);
        return result != null ? Ok(result) : NotFound();
    }
}