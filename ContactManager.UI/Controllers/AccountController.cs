using ContactsManager.Core.Domain.Entities.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }


            var result = await signManager.PasswordSignInAsync(
                loginDTO.Email,
                loginDTO.Password,
                isPersistent: true, lockoutOnFailure: false); //lockoutOnFailure for a while after 3 attempts prohibit to log in
            //1 if they exsist and suit then accse will be true


            if (result.Succeeded)
            {
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            ModelState.AddModelError("Login", "Invalid user data : email or password"); //name of padge and error

            return View(loginDTO);
        }


        [HttpGet]
        public async Task< IActionResult> Logout()
        {
             await signManager.SignOutAsync();

            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

    }
}
