using LearningTracker.Api.Data;
using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearningTopicsController : ControllerBase
{
    private readonly LearningTrackerDbContext _context;
    public LearningTopicsController(LearningTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<LearningTopic>>> GetAll()
    {
        var topics = await _context.LearningTopics
            .AsNoTracking()
            .OrderByDescending(topic => topic.CreatedAtUtc)
            .ToListAsync();

        return Ok(topics);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LearningTopic>> GetById(int id)
    {
        var topic = await _context.LearningTopics.FindAsync(id);

        if (topic is null)
            return NotFound("Такого айди не найдено");

        return Ok(topic);

    }

    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult<LearningTopic>> Complete(int id)
    {
        var topic = await _context.LearningTopics.FindAsync(id);
        if (topic is null)
            return NotFound("ID не найден");

        topic.IsCompleted = true;

        await _context.SaveChangesAsync();

        return Ok(topic);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTopic>> Create(
    CreateLearningTopicRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Название темы не может быть пустым");

        var topic = new LearningTopic
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAtUtc = DateTime.UtcNow,
            IsCompleted = false
        };

        _context.LearningTopics.Add(topic);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new {id = topic.Id},
            topic);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _context.LearningTopics.FindAsync(id);

        if (topic is null)
            return NotFound("ID не найден");

        _context.LearningTopics.Remove(topic);

        await _context.SaveChangesAsync();

        return NoContent();


    }

}
