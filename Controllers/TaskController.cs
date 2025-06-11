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
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public TaskController(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        #region Gets

        [HttpGet("by-id/{id:int}")]
        public async Task<ActionResult<TaskEntity>> GetTaskByID(int id)
        {
            if (id == 0) return NotFound();
            var result = await _appDbContext.TasksEntity.FirstOrDefaultAsync(p => p.Id == id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("all-user-taks/{username}")]
        public async Task<ActionResult<TaskEntity>> GetAllAccountTasks(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return BadRequest("Account name and task name cannot be empty.");
            
            var result = _appDbContext.TasksEntity.Select(taskobject => taskobject.AccountsUsername.Select(account => account.Username == username)).ToList();
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("all-task")]
        public async Task<ActionResult<List<TaskEntity>>> GetAllTask()
        {
            if (_appDbContext.TasksEntity.Count()==0)
            {
                return NoContent();
            }
            var result = await _appDbContext.TasksEntity.ToListAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }

        
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<TaskEntity>> GetTaskByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return NotFound();
            var result = await _appDbContext.TasksEntity.FirstOrDefaultAsync(p => p.TaskName == name);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("by-project/{projectName}")]
        public async Task<ActionResult<TaskEntity>> GetTaskByProjectName(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName)) return NotFound();
            var result = await _appDbContext.TasksEntity.FirstOrDefaultAsync(predicate: p => p.Project.Name == projectName);
            if (result == null) return NotFound();
            return Ok(result);
        }

        #endregion

        [HttpPost]
        public async Task<ActionResult> CreatedTask([FromBody] TaskEntity task)
        {
            if (task is null || string.IsNullOrWhiteSpace(task.TaskName)) return BadRequest();

            await _appDbContext.TasksEntity.AddAsync(task);
            await _appDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTaskByID), new { id = task.Id }, task);
        }

        [HttpPut("asign-person/{personUsername}")]
        public async Task<ActionResult<TaskEntity>> AsignPersonToTask(string personUsername,string taskname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(personUsername) || string.IsNullOrWhiteSpace(taskname))
                {
                    return BadRequest("Person name and task name cannot be empty.");
                }
                var existTask = await _appDbContext.TasksEntity
                                                   .Include(t => t.AccountsUsername)
                                                   .FirstOrDefaultAsync(task => task.TaskName == taskname);

                if (existTask == null)
                {
                    return NotFound($"Task with name '{taskname}' not found.");
                }

                var existPerson = await _appDbContext.Accounts.FirstOrDefaultAsync(account => account.Username == personUsername);

                if (existPerson == null)
                {
                    return NotFound($"Account with name '{existPerson}' not found.");
                }

                existTask.AccountsUsername.Add(existPerson);

                await _appDbContext.SaveChangesAsync();

                return Ok(existTask);
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
                return NoContent();
            }
        }

        [HttpPut("change-project/{projectName}")]
        public async Task<ActionResult<TaskEntity>> ChangeProjectFromTask(string projectName, string taskname)
        {
            if (string.IsNullOrWhiteSpace(projectName) || string.IsNullOrWhiteSpace(taskname))
            {
                return BadRequest("Project name and task name cannot be empty.");
            }
            var existTask = await _appDbContext.TasksEntity
                                               .Include(t => t.Project)
                                               .FirstOrDefaultAsync(task => task.TaskName == taskname);

            if (existTask == null)
            {
                return NotFound($"Task with name '{taskname}' not found.");
            }

            var existProject = await _appDbContext.Projects.FirstOrDefaultAsync(project => project.Name == projectName);

            if (existProject == null)
            {
                return NotFound($"Project with name '{projectName}' not found.");
            }

            existTask.Project = existProject;


            Console.WriteLine($"Task '{existTask.TaskName}' new Project: '{existTask.Project.Name}'");

            await _appDbContext.SaveChangesAsync();

            return Ok(existTask);
        }


        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult> DeleteTaskByID(int id)
        {
            if (id==0) return NotFound();
            var task = await _appDbContext.TasksEntity.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            _appDbContext.TasksEntity.Remove(task);
            await _appDbContext.SaveChangesAsync();
            return Ok();

        }
    }
}
