﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Security;
using FinalProject.Web.Models.User;
using FinalProject.Data.Extensions;

namespace FinalProject.Web.Controllers;

public class UserController : BaseController
{
    private readonly IConfiguration _config;
    private readonly IMailService _mailer;
    private readonly IUserService _svc;

    public UserController(IUserService svc, IConfiguration config, IMailService mailer)
    {
        _config = config;
        _mailer = mailer;
        _svc = svc;
    }

    // HTTP GET - Display Paged List of Users
    [Authorize(Roles = "admin")]
    public ActionResult Index(string searchQuery, int page = 1, int size = 20, string order = "id", string direction = "asc")
    {

        var query = string.IsNullOrWhiteSpace(searchQuery)
            ? _svc.GetUsers(page, size, order, direction).Data.AsQueryable()
            : _svc.SearchUsers(searchQuery);

        var pagedUsers = query.ToPaged(page, size, order);

        //if no users are found, set a flag to show a message in the view
        ViewBag.SearchQuery = searchQuery;

        if (!pagedUsers.Data.Any() && !string.IsNullOrWhiteSpace(searchQuery))
        {
            Alert($"No users found matching '{searchQuery}'. Please check the spelling or try again.", AlertType.warning);
        }

        return View(pagedUsers);
    }

    // HTTP GET - Display Login page
    public IActionResult Login()
    {
        return View();
    }

    // HTTP POST - Login action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel m)
    {
        var user = _svc.Authenticate(m.Email, m.Password);
        // check if login was unsuccessful and add validation errors
        if (user == null)
        {
            ModelState.AddModelError("Email", "Invalid Login Credentials");
            ModelState.AddModelError("Password", "Invalid Login Credentials");
            return View(m);
        }

        // Login Successful, so sign user in using cookie authentication
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            BuildClaimsPrincipal(user)
        );

        Alert("Successfully Logged in", AlertType.info);

        return Redirect("/");
    }

    // HTTP GET - Display Register page
    public IActionResult Register()
    {
        return View();
    }

    // HTTP POST - Register action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register([Bind("Name,Email,Password,PasswordConfirm,DogBreed,ProfileImage")] RegisterViewModel m)
    {
        if (!ModelState.IsValid)
        {
            return View(m);
        }

        // add user via service
        var user = _svc.AddUser(m.Name, m.Email, m.Password, Role.guest, m.DogBreed, null);

        // check if error adding user and display warning
        if (user == null)
        {
            Alert("There was a problem Registering. Please try again", AlertType.warning);
            return View(m);
        }

        Alert("Successfully Registered. Now login", AlertType.info);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet("Profile/{id}")]
    public IActionResult Profile(int id)
    {
        var user = _svc.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // HTTP GET - Display Update profile page
    [Authorize]
    public IActionResult UpdateProfile()
    {
        // use BaseClass helper method to retrieve Id of signed in user 
        var user = _svc.GetUser(User.GetSignedInUserId());
        var profileViewModel = new ProfileViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            DogBreed = user.DogBreed,
            Role = user.Role
        };
        return View(profileViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteConfirmed(int id)
    {
        var deleted = _svc.DeleteUser(id);
        if (deleted)
        {
            Alert("User successfully deleted.", AlertType.success);
        }
        else
        {
            Alert("User could not be deleted. Please try again.", AlertType.warning);
        }
        return RedirectToAction(nameof(Index));
    }

    // HTTP POST - Update profile action
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile([Bind("Id,Name,Email,DogBreed,ProfileImage")] ProfileViewModel m)
    {
        var user = _svc.GetUser(m.Id);
        // check if form is invalid and redisplay
        if (!ModelState.IsValid || user == null)
        {
            return View(m);
        }

        // update user details and call service
        user.Name = m.Name;
        user.Email = m.Email;
        user.DogBreed = m.DogBreed;
        user.ProfileImageUrl = m.ProfileImageUrl;
        var updated = _svc.UpdateUser(user);

        // check if error updating service
        if (updated == null)
        {
            Alert("There was a problem Updating. Please try again", AlertType.warning);
            return View(m);
        }

        Alert("Successfully Updated Account Details", AlertType.info);

        // sign the user in with updated details)
        await SignInCookie(user);

        return RedirectToAction("Index", "Home");
    }

    // HTTP GET - Allow admin to update a User
    [Authorize(Roles = "admin")]
    public IActionResult Update(int id)
    {
        // retrieve user 
        var user = _svc.GetUser(id);
        var profileViewModel = new ProfileViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            DogBreed = user.DogBreed,
            ProfileImageUrl = user.ProfileImageUrl,
            Role = user.Role
        };
        return View(profileViewModel);
    }

    // HTTP POST - Update User action
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update([Bind("Id,Name,Email,DogBreed,ProfileImageUrl,Role")] ProfileViewModel m)
    {
        var user = _svc.GetUser(m.Id);
        // check if form is invalid and redisplay
        if (!ModelState.IsValid || user == null)
        {
            return View(m);
        }

        //handle the uploaded profile image 
        if (m.ProfileImage != null && m.ProfileImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(m.ProfileImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                m.ProfileImage.CopyTo(stream);
            }

            user.ProfileImageUrl = $"/uploads/{uniqueFileName}";
        }

        // update user details and call service
        user.Name = m.Name;
        user.Email = m.Email;
        user.DogBreed = m.DogBreed;
        user.Role = m.Role;

        var updated = _svc.UpdateUser(user);

        // check if error updating service
        if (updated == null)
        {
            Alert("There was a problem Updating. Please try again", AlertType.warning);
            return View(m);
        }

        Alert("Successfully Updated User Account Details", AlertType.info);

        return RedirectToAction("Index", "User");
    }

    // HTTP GET - Display update password page
    [Authorize]
    public IActionResult UpdatePassword()
    {
        // use BaseClass helper method to retrieve Id of signed in user 
        var user = _svc.GetUser(User.GetSignedInUserId());
        var passwordViewModel = new PasswordViewModel
        {
            Id = user.Id,
            Password = user.Password,
            PasswordConfirm = user.Password,
        };
        return View(passwordViewModel);
    }

    // HTTP POST - Update Password action
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword([Bind("Id,OldPassword,Password,PasswordConfirm")] PasswordViewModel m)
    {
        var user = _svc.GetUser(m.Id);
        if (!ModelState.IsValid || user == null)
        {
            return View(m);
        }
        // update the password
        user.Password = m.Password;
        // save changes      
        var updated = _svc.UpdateUser(user);
        if (updated == null)
        {
            Alert("There was a problem Updating the password. Please try again", AlertType.warning);
            return View(m);
        }

        Alert("Successfully Updated Password", AlertType.info);
        // sign the user in with updated details
        await SignInCookie(user);

        return RedirectToAction("Index", "Home");
    }

    // HTTP POST - Logout action
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }


    // HTTP GET - Display Forgot password page
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // HTTP POST - Forgot password action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ForgotPassword([Bind("Email")] ForgotPasswordViewModel m)
    {
        var token = _svc.ForgotPassword(m.Email);
        if (token == null)
        {
            // No such account. Alert only for testing
            Alert("No account found", AlertType.warning);
            return RedirectToAction(nameof(Login));
        }

        // build reset password url and email html message
        var url = $"{Request.Scheme}://{Request.Host}/User/ResetPassword?token={token}&email={m.Email}";
        var message = @$" 
            <h3>Password Reset</h3>
            <a href='{url}'>
                {url}
            </a>
        ";

        // send email containing reset token
        if (!_mailer.SendMail("Password Reset Request", message, m.Email))
        {
            Alert("There was a problem sending a password reset email", AlertType.warning);
            return RedirectToAction(nameof(ForgotPassword));
        }

        Alert("Password Reset Token sent to your registered email account", AlertType.info);
        return RedirectToAction(nameof(ResetPassword));
    }

    // HTTP GET - Display Reset password page
    public IActionResult ResetPassword(string email, string token)
    {
        return View(new ResetPasswordViewModel { Email = email, Token = token });
    }


    // HTTP POST - ResetPassword action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetPassword([Bind("Email,Password,Token")] ResetPasswordViewModel m)
    {
        // verify reset request
        var user = _svc.ResetPassword(m.Email, m.Token, m.Password);
        if (user == null)
        {
            Alert("Invalid Password Reset Request", AlertType.warning);
            return RedirectToAction(nameof(ResetPassword));
        }

        Alert("Password reset successfully", AlertType.success);
        return RedirectToAction(nameof(Login));
    }

    // HTTP GET - Display not authorised and not authenticated pages
    public IActionResult ErrorNotAuthorised() => View();
    public IActionResult ErrorNotAuthenticated() => View();

    // -------------------------- Helper Methods ------------------------------

    // Called by Remote Validation attribute on RegisterViewModel to verify email address is available
    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyEmailAvailable(string email, int id)
    {
        // check if email is available, or owned by user with id 
        if (!_svc.IsEmailAvailable(email, id))
        {
            return Json($"A user with this email address {email} already exists.");
        }
        return Json(true);
    }

    // Called by Remote Validation attribute on ChangePassword to verify old password
    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyPassword(string oldPassword)
    {
        // use BaseClass helper method to retrieve Id of signed in user 
        var id = User.GetSignedInUserId();
        // check if email is available, unless already owned by user with id
        var user = _svc.GetUser(id);
        if (user == null || !Hasher.ValidateHash(user.Password, oldPassword))
        {
            return Json($"Please enter current password.");
        }
        return Json(true);
    }

    // =========================== PRIVATE UTILITY METHODS ==============================

    // return a claims principle using the info from the user parameter
    private ClaimsPrincipal BuildClaimsPrincipal(User user)
    {
        // define user claims
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        // build principal using claims
        return new ClaimsPrincipal(claims);
    }

    // Sign user in using Cookie authentication scheme
    private async Task SignInCookie(User user)
    {
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            BuildClaimsPrincipal(user)
        );
    }
}