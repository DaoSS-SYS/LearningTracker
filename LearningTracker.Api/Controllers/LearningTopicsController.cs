using LearningTracker.Api.Dto;
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

    [HttpGet]
    public async Task<ActionResult<List<LearningTopic>>> GetAll()
    {
        var topics = await _service.GetAllAsync();
        return Ok(topics);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LearningTopic>> GetById(int id)
    {
        var topic = await _service.GetByIdAsync(id);
        return Ok(topic);
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult<LearningTopic>> Complete(int id)
    {
        var topic = await _service.CompleteAsync(id);
        return Ok(topic);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTopic>> Create(
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
    public async Task<ActionResult<LearningTopic>> Update(int id, UpdateLearningTopicRequest request)
    {
        var topic = await _service.UpdateAsync(id, request);
        return Ok(topic);
    }
}
