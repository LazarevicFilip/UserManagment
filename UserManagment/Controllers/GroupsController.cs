using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using UserManagment.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace UserManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "administrator")]
    public class GroupsController : ControllerBase
    {
        private readonly UserManagmentDataContext context;

        public GroupsController(UserManagmentDataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GroupResult>> getSpecificGroup(int id)
        {
            var group = await context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if(group == null)
                return NotFound();
            return Ok(new GroupResult(group.Id,group.Name));
        }
        [HttpGet]
        public async Task<ActionResult<GroupResult>> getGroups()
        {
            //if(!(this.User.Claims.First(x => x.Type == ClaimTypes.Role).Value == "administrator"))
            //{
            //    return Forbid();
            //}
            return Ok(await context.Groups.Select(x => new GroupResult(x.Id,x.Name)).ToArrayAsync());
        }
        [HttpGet]
        [Route("{id}/groups")]
        public async Task<ActionResult<GroupResult>> getMembersOfGroup(int id)
        {
            //var group = await context.Groups.FirstOrDefaultAsync(x => x.Id == id);
            //if (group == null)
            //    return NotFound();
            //var result = await context.Groups.Where(x => x.ParentGroupId == id).Select(x => new GroupResult(x.Id,x.Name)).ToListAsync();
            var group = await context.Groups.Include(x => x.ChildGroups).FirstOrDefaultAsync(x => x.Id == id);
            if (group == null)
                return NotFound();
            return Ok(group.ChildGroups!.Select(x => new GroupResult(x.Id,x.Name)));
        }
        [HttpGet]
        [Route("{id}/users")]

        public async Task<ActionResult<UserResult>> getUserMembersOfGroup(int id, [FromQuery] bool recursive = false)
        {
            if (!await context.Groups.AnyAsync(g => g.Id == id)) return NotFound();

            var users = await getMembers(id, recursive);

            return Ok(users.Select(u => new UserResult(u.Id, u.NameIdentifier, u.Email, u.FirstName, u.LastName)));
        }
        public async Task<IEnumerable<User>> getMembers(int id, bool recursive)
        {
            var group = await context.Groups.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);


            var users = new List<User>();
            if (group.Users!.Any())
            {
                users.AddRange(group.Users!.ToList());
            }
            if (recursive)
            {
               
                await context.Entry(group).Collection(x => x.ChildGroups).LoadAsync();
                if (group.ChildGroups!.Any())
                {
                    foreach (var child in group.ChildGroups!)
                    {
                        users.AddRange(await getMembers(child.Id, recursive));
                    }
                    return users;
                }
                
            }

            return users;
        }
    }
}
