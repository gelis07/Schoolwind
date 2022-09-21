using System.Net;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class StudentsModel
    {
        public List<int> ids = new List<int>();
        public string sqlCommand = "SELECT id,FirstName,LastName,Email,PhoneNumber,Gender,BirthDate FROM students WHERE account_id=";
        public void AddItem(int data){
            ids.Add(data);
        }
    }
}