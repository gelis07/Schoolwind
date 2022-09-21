using System.Security.AccessControl;
using System.Net;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using System.Net.Mail;
namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LogedIn", "false");

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult CheckGuid()
        {
            return View();
        }
        public IActionResult Email()
        {
            return View("RecoverPass/Email");
        }
        public IActionResult ChangePass()
        {
            return View("RecoverPass/ChangePass");
        }
        [HttpPost]
        public IActionResult AccountData(string Email, string Name, string Password)
        {
            AccountsModel am = new AccountsModel();
            am.Email = Email;
            am.Name = Name;
            am.Password = Password;
            HttpContext.Session.Set("Model", BinaryConv.ObjectToByteArray(am));
            SendGuid(Email);
            return View();
        }
        [HttpPost]
        public string LogIn(string Email, string Name, string Password)
        {
            dynamic info = GetSqlData.GetData("SELECT IF(SUM(IF(Email = '" + Email.ToLower() + "' AND password=AES_ENCRYPT('" + Password + "', '" + GetSqlData.sqlkey + "'),1,0))>=1, 'True','False') FROM accounts");
            Console.WriteLine(info["Data"][0]);
            string validation = info["Data"][0];
            if (validation == "True")
            {
                dynamic UserID = GetSqlData.GetData("SELECT id FROM accounts WHERE email='" + Email.ToLower() + "'");
                HttpContext.Session.SetInt32("UserID", (int)UserID["Data"][0]);
                HttpContext.Session.SetString("LogedIn", "true");
            }
            return validation;
        }
        public string LoggedIn()
        {
            return HttpContext.Session.GetString("LogedIn");
        }
        public void SendGuid(string Mail)
        {
            char quote = '"'; 
            string Code = GenerateRandomCode();
            HttpContext.Session.SetString("Code", Code);
            SendMail(Mail, "Email Verification", @"<html lang='en'>
                <head>
                    <link href='https://fonts.googleapis.com/css?family=Roboto' rel='stylesheet'>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Document</title>
                </head>
                <body style='background-color:#ECEBE4;'>
                    <h2 style="+quote+@"font-family: 'roboto';color: #CC998D;"+quote+@">Hello, "+Mail+@"</h2>
                    <h2 style="+quote+@"font-family: 'roboto';color: #CC998D;"+quote+@">Please insert the code below to the text field <br>on the website to verify your account</h2>
                    <h2 id='code' style="+quote+@"font-family: 'roboto';color: #CC998D;background-color: white;padding: 30px 150px 30px 150px;"+quote+@">"+Code+@"</h2>
                    <footer style="+quote+@"font-family: 'roboto';color: #CC998D;"+quote+@">Have a nice day!</footer>
                </body>
                </html>");
        }
        [HttpPost]
        public dynamic CreateAccount(string GuidInput)
        {
            if (GuidInput == HttpContext.Session.GetString("Code"))
            {
                DateTime dateTime = DateTime.Today;
                AccountsModel am = (AccountsModel)BinaryConv.ByteArrayToObject(HttpContext.Session.Get("Model"));
                dynamic state = GetSqlData.GetData("INSERT INTO accounts(email, SchoolName,password, DateCreated) VALUES('" + am.Email.ToLower() + "', '" + am.Name + "', AES_ENCRYPT('" + am.Password + "', '" + GetSqlData.sqlkey + "'), '" + dateTime.ToString("yyyy-MM-dd") + "')");
                if (state["Response"] == "Error")
                {
                    return state["Error"];
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public dynamic RecoverPassword(string Mail)
        {
            string Code = GenerateRandomCode();
            HttpContext.Session.SetString("RecoveryCode", Code);
            dynamic Email = GetSqlData.GetData("SELECT id FROM accounts WHERE email='" + Mail + "'");
            try
            {
                char quote = '"'; 
                HttpContext.Session.SetInt32("UserID", (int)Email["Data"][0]);
                SendMail(Mail, "Recover Password", @"<html lang='en'>
                <head>
                    <link href='https://fonts.googleapis.com/css?family=Roboto' rel='stylesheet'>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Document</title>
                </head>
                <body style='background-color:#ECEBE4;'>
                    <h2 style="+quote+@"font-family: 'roboto';color: #CC998D;"+quote+@">Hello, "+Mail+@"</h2>
                    <h2 style="+quote+@"font-family: 'roboto';color: #CC998D;"+quote+@">Please insert the code below to the text field <br>on the website to change your current password</h2>
                    <h2 id='code' style="+quote+@"font-family: 'roboto';color: #CC998D;background-color: white;padding: 30px 150px 30px 150px;"+quote+@">"+Code+@"</h2>
                    <footer style="+quote+@"font-family: 'roboto';color: #CC998D;"+quote+@">Have a nice day!</footer>
                </body>
                </html>");
            }
            catch (Exception ex)
            {
                return "Failed to send Email. Try using another Email Address";
            }
            return true;
        }
        [HttpPost]
        public bool ChangePassword(string code, string NewPassword)
        {
            string test = HttpContext.Session.GetString("RecoveryCode");
            if (code == HttpContext.Session.GetString("RecoveryCode"))
            {
                dynamic ChangePass = GetSqlData.GetData("UPDATE accounts SET password=AES_ENCRYPT('" + NewPassword + "', '" + GetSqlData.sqlkey + "') WHERE id=" + HttpContext.Session.GetInt32("UserID"));
                return true;
            }
            return false;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void SendMail(string Mail, string Subject, string body)
        {
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("schoolmanagementprofessional@gmail.com", "lzvwjmycqjoaqrat");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Mail);
                mail.To.Add(Mail);
                mail.Subject = Subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
            }
        }
        public string GenerateRandomCode(int digits=5){
            string number = "";
            for (int i = 0; i < digits; i++)
            {
                number += new Random().Next(10);
            }
            return number;
        }
    }
}