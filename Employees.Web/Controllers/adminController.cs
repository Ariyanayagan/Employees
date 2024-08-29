using Employees.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleViewModel roleViewModel)
        {
            IdentityResult? result = null;
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole
                {
                    Name = roleViewModel.role
                };

                result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
            }

            return BadRequest(new
            {
                message = string.Join(", ", result?.Errors.Select(e => e.Description)?? new List<string>()
                {
                    "Error Occured!"
                })
            });
        }

        [HttpGet("roles")]
        public IQueryable<IdentityRole> ListRoles()
        {
            return _roleManager.Roles;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
                if (!roleExists)
                {
                    return BadRequest(new { message = "Role does not exist" });
                }

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    return Ok(new { message = "User assigned to role successfully" });
                }

                return BadRequest(new { message = "Failed to assign role", errors = result.Errors });
            }

            return BadRequest(ModelState);
        }

        // POST api/admin/remove-role
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
                if (!roleExists)
                {
                    return BadRequest(new { message = "Role does not exist" });
                }

                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Role removed from user successfully" });
                }

                return BadRequest(new { message = "Failed to remove role", errors = result.Errors });
            }

            return BadRequest(ModelState);
        }
    }
}
