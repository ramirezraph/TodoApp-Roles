using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.ViewModels;

namespace TodoApp.Controllers
{
    public class RoleController : Controller
    {
        // model? 
        // IdentityRole 
        public RoleManager<IdentityRole> _roleManager { get; }
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = roleViewModel.Name
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role is null)
            {
                return NotFound();
            }

            var viewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel roleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(roleViewModel.Id);

            if (role is null)
            {
                return NotFound();
            }

            role.Name = roleViewModel.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(roleViewModel);
        }

        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(role);

            return RedirectToAction("Index");
        }
    }
}
