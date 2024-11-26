using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Lab4.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Добавляем пользователя в указанную роль
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new CreateUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() // Получаем роль пользователя
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CreateUserViewModel model)
        {
            // Отладочная информация
            System.Diagnostics.Debug.WriteLine($"Editing user with ID: {id}");
            System.Diagnostics.Debug.WriteLine($"Email: {model.Email}");
            System.Diagnostics.Debug.WriteLine($"Password: {model.Password}");
            System.Diagnostics.Debug.WriteLine($"Role: {model.Role}");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Обновляем информацию о пользователе
                user.Email = model.Email;
                user.UserName = model.Email; // Обновляем имя пользователя

                // Проверяем, есть ли новый пароль
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                // Обновляем информацию о пользователе
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    // Обновляем роль пользователя, если она изменена
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (model.Role != null && !currentRoles.Contains(model.Role))
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }
                    return RedirectToAction("Index");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                // Проверяем ошибки в модели
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}