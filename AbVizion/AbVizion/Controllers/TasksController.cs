using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using AbVizion.Models;

namespace AbVizion.Controllers
{
  //  [Authorize]
    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Tasks
        public IEnumerable<AbVizion.Models.Task> GetTasks()
        {
            IEnumerable<AbVizion.Models.Task> result;

            if (User.IsInRole("Developer"))
            {
                var user = db.Users.Where(x => x.Id == User.Identity.GetUserId()).FirstOrDefault();
                var tasks = db.Tasks.Where(x => x.ApplicationUserId == user.Id).Select(t=> t);
                result = tasks.ToArray();
            }
            else
            {
                result = db.Tasks.Select(t=>t).ToArray();
            }

            return result;
        }

         [Route("Complexity")]
        public string GetComplexity(AbVizion.Models.Task task)
        {
            var complexity = task.Complexity;
            return complexity;
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(AbVizion.Models.Task))]
        public async Task<IHttpActionResult> GetTask(int id)
        {
            AbVizion.Models.Task task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTask(int id, AbVizion.Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        //[Authorize(Roles="User")]
        [ResponseType(typeof(AbVizion.Models.Task))]
        public async Task<IHttpActionResult> PostTask(AbVizion.Models.Task task)
        {
            if (User.IsInRole("User"))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Tasks.Add(task);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
            }
            return Unauthorized();
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(AbVizion.Models.Task))]
        public async Task<IHttpActionResult> DeleteTask(int id)
        {
            AbVizion.Models.Task task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(task);
            await db.SaveChangesAsync();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }
    }
}