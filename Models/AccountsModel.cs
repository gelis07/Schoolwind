using System.Net;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;

namespace WebApplication2.Models
{
    [Serializable]
    public class AccountsModel
    {
        public string Email {get; set;} = default!;
        public string Name {get; set;} = default!;

        public string Password {get; set;} = default!;
        public int? ID;
    }
}