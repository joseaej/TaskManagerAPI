using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Services;
using TasksManagerAPI.Data;
using TasksManagerAPI.Models.Entity;

namespace TasksManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public ProjectController(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            if (id == 0) return NotFound();
            var result = await _appDbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreatedProject([FromForm] Project project)
        {
            if (project is null || string.IsNullOrWhiteSpace(project.Name)) return BadRequest();

            await _appDbContext.Projects.AddAsync(project);
            await _appDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }


        [HttpPut]
        public async Task<ActionResult> UpdatedProject([FromForm] Project project,int id)
        {
            if (project is null || string.IsNullOrWhiteSpace(project.Name)) return BadRequest();

            var existingProject = await _appDbContext.Projects.FindAsync(id);
            if (existingProject == null)
                return NotFound();

            existingProject.Name = project.Name;

            _appDbContext.Projects.Update(existingProject);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
