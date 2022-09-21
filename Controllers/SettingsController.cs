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
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void DeleteAccount(){
            dynamic Delete = GetSqlData.GetData("DELETE FROM accounts WHERE id="+HttpContext.Session.GetInt32("UserID"));
            HttpContext.Session.Clear();
        }
    }
}