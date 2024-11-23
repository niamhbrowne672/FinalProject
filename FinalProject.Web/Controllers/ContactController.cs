using Microsoft.AspNetCore.Mvc;
using FinalProject.Data.Services;
using FinalProject.Web.Models.Contact;

namespace FinalProject.Web.Controllers;

public class ContactController : BaseController
{
    private readonly IMailService _mailService;

    public ContactController(IMailService mailService)
    {
        _mailService = mailService;
    }

    public IActionResult Contact()
    {
        var model = new ContactViewModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SendMessage(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            Alert("Please correct the errors in the form and try again.", AlertType.warning);
            return View("Contact", model);
        }

        string subject = $"New Contact Message from {model.Name}";
        string body = $@"
            <h3>Contact Message</h3>
            <p><strong>Name:</strong> {model.Name}</p>
            <p><strong>Email:</strong> {model.Email}</p>
            <p><strong>Message:</strong></p>
            <p>{model.Message}</p>
        ";

        bool success = _mailService.SendMail(subject, body, "admin@mail.com");

        if (success)
        {
            Alert($"Thank you, {model.Name}! Your message has been sent successfully! We'll get back to you as soon as possible!", AlertType.success);
        }
        else
        {
            Alert($"Sorry, {model.Name}. We couldn't send your message. Please try again later.", AlertType.warning);
        }
        return RedirectToAction(nameof(Contact));
    }
}