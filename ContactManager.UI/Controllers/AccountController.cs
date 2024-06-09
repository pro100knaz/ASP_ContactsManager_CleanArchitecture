using ContactsManager.Core.Domain.Entities.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            this.userManager = userManager;
            this.signManager = signManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            //Check For Validation errors
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(register);
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.Email, //for login
                PhoneNumber = register.PhoneNumber,
                PersonName = register.PersonName,
            };

            IdentityResult result = await userManager.CreateAsync(user, register.Password); //status of db operation

            if (result.Succeeded)
            {

                await signManager.SignInAsync(user, isPersistent: false); 
                //создает куки и отправляет их в виде ответа браузера
                //persisten означает что куки будут сохраннены даже после закрытия браузер
                //иначе удаляются автоматически



                
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description); //name of padge and error
                }
            }

            return View(register);

        }
    }
}
