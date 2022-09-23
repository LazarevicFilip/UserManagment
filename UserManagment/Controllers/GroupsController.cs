using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using UserManagment.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace UserManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
