using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagment.Data;

namespace UserManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManagmentDataContext context;

        public UsersController(UserManagmentDataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("me")]
        public async Task<ActionResult<UserResult>> getCurrentUser()
        {
            var userName = this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await context.Users.Include(x => x.Groups).FirstOrDefaultAsync(x => x.NameIdentifier == userName.Value);

            return Ok(new UserResult(user.Id,user.NameIdentifier,user.Email, user.FirstName,user.LastName));
        }
        [HttpGet]
        public async Task<ActionResult<UserResult>> getUsers([FromQuery] string? filter = null)

        {
            var users = context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower().Trim();
                users = users.Where(x =>

                    (x.LastName != null && x.LastName.Equals(filter) ||
                    x.Email.Contains(filter) ||
                    (x.FirstName != null && x.FirstName.Equals(filter)
                )));
               
                
            }
            return Ok(await users.Select(x => new UserResult(x.Id,x.NameIdentifier,x.Email,x.FirstName,x.LastName)).ToListAsync());
          
        }
    }
}
