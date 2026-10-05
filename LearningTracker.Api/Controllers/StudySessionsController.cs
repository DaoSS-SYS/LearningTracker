using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;
using LearningTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Api.Controllers;

[ApiController]
[Route("api/")]
public class StudySessionsController : ControllerBase
{
    private readonly IStudySessionService _service;
    public StudySessionsController(IStudySessionService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("LearningTopics/{id:int}/sessions")]
    public async Task<ActionResult<List<StudySessionDto>>> GetByTopic(int id)
    {
        var topics = await _service.GetSessionsAsync(id);
        return Ok(topics);
    }

    [HttpPost]
    [Route("LearningTopics/{id:int}/sessions")]
    public async Task<ActionResult<StudySessionDto>> Create(int id,
    CreateStudySessionRequest request)
    {
        var createdSession = await _service.CreateAsync(request, id);

        return CreatedAtAction(
            nameof(GetByTopic),
            new {id = createdSession.LearningTopicId},
            createdSession);
    }
}
