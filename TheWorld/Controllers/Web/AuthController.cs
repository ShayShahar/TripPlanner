using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
  public class AuthController : Controller
  {
    private readonly SignInManager<WorldUser> m_signInManager;
    private readonly UserManager<WorldUser> m_userManager;

    public AuthController(SignInManager<WorldUser> p_signInManager, UserManager<WorldUser> p_userManager)
    {
        m_signInManager = p_signInManager;
        m_userManager = p_userManager;
    }

    public ActionResult Login()
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Trips", "App");
      }
      return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel p_model)
    {
        if (ModelState.IsValid)
        {
            string firstName = p_model.FirstName.First().ToString().ToUpper() +
                               p_model.FirstName.ToString().Substring(1);

            string familyName = p_model.FirstName.First().ToString().ToUpper() +
                   p_model.FirstName.ToString().Substring(1);

            var user = new WorldUser { UserName = p_model.UserName, Email = p_model.Email,  Name = firstName + " " + familyName};
            var result = await m_userManager.CreateAsync(user, p_model.Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                await m_signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "App");
            }
                var resultsError = result.Errors.Aggregate("\n", (current, t) => current + (t.Description + "\n"));

                var errorStr = string.Format("Registration Failed. {0}", resultsError);

                    ModelState.AddModelError("", errorStr);
            }


         // If we got this far, something failed, redisplay form
         return View(p_model);
      }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel p_model)
    {
      if (ModelState.IsValid)
      {
        var signinResult = await m_signInManager.PasswordSignInAsync(p_model.UserName,
                                                                    p_model.Password,
                                                                    false, false);
        if (signinResult.Succeeded)
        {
          return RedirectToAction("Trips", "App");
        }
      }

      // Just say Login failed on all errors
      ModelState.AddModelError("", "Login Failed");

      return View();
    }

    public async Task<ActionResult> Logout()
    {
      if (User.Identity.IsAuthenticated)
      {
        await m_signInManager.SignOutAsync();
      }
      return RedirectToAction("Index", "App");
    }

  }
}
