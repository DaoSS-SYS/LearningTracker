using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;
using LearningTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearningTopicsController : ControllerBase
{
    private readonly ILearningTopicService _service;
    public LearningTopicsController(ILearningTopicService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}/stats")]
    public async Task<ActionResult<LearningTopicStatsDto>> GetStats(int id)
    {
        var topicStats = await _service.GetStatsAsync(id);
        return Ok(topicStats);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LearningTopicDto>> GetById(int id)
    {
        var topic = await _service.GetByIdAsync(id);
        return Ok(topic);
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult<LearningTopicDto>> Complete(int id)
    {
        var topic = await _service.CompleteAsync(id);
        return Ok(topic);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTopicDto>> Create(
    CreateLearningTopicRequest request)
    {
        var createdTopic = await _service.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTopic.Id },
            createdTopic);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LearningTopicDto>> Update(int id, UpdateLearningTopicRequest request)
    {
        var topic = await _service.UpdateAsync(id, request);
        return Ok(topic);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<LearningTopicDto>>> GetAll([FromQuery]LearningTopicsFilters filters)
    {
        var topics = await _service.GetAllAsync(filters);
        return Ok(topics);
    }
}
