using Mailer.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Mailer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {

        private readonly IMailServices _mailServices;
        public MailController(IMailServices mailServices)
        {
            _mailServices = mailServices;
        }   

        [HttpPost]
        [Route("send-mail")]
        public IActionResult EmailVerification(string email)
        {
            //var user = context.Users.Where(q => (q.UserId == userid)).FirstOrDefault();
            string subject = "Account Activation for RSE-HIE"; ;
            //string appURL = _appUrl;
            string url = "https://www.w3.org/Provider/Style/dummy.html";
                //+ "/Registration/AccountActivation?reference=" + user.UserId;
            //string body = "Hi " + user.Firstname + ", <br><br>A Request for a new account was processed and generated this automated <br><br>Your User Name: " + user.Username + " <br><br> In order to activate this account,please click the following link: <a href=" + url + ">" + url+ "</a><br><br> If the link does not work when you click it,copy and paste the link directly into your browser.<br><br> Thank you,<br>RSE-HIE Customer Service.";
            //string body = "Hi RSE-HIE";
            string body = "Hi " + "Aarya Garg" + ", <br><br>A Request for a new account was processed and generated this automated <br><br>Your User Name:" + 
                "Aarya123" + " <br><br> In order to activate this account,please click the following link: <br><a href=" + url + ">"+url+"</a><br> If the link does not work when you click it,copy and paste the link directly into your browser.<br><br> Thank you,<br>RSE-HIE Customer Service.";

            _mailServices.SendEmailAsync(email, subject, body);

            //SendEmailToUser(body, "agarg@moreyeahs.com", subject);
            return Ok("Mail Send Successfully");
        }

    }
}
