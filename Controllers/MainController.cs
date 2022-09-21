using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
namespace WebApplication2.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            ViewData["Title"] = "Manage Index";
            return View();
        }
        #region Views
        public IActionResult Students()
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            ViewData["Title"] = "Manage Students";
            if (HttpContext.Session.GetString("SCommand") == null)
            {
                HttpContext.Session.SetString("SCommand", "SELECT FirstName,LastName,Email,PhoneNumber,Gender,BirthDate FROM students WHERE account_id=" + HttpContext.Session.GetInt32("UserID"));
                HttpContext.Session.SetString("SiCommand", "SELECT id FROM students WHERE account_id=" + HttpContext.Session.GetInt32("UserID"));
            }
            ManageModel model = new ManageModel();
            string[] Array = { "First Name", "Last Name", "Email Adress", "Phone Number", "Gender", "Birth Date" };
            model.Set("Students","SCommand","SiCommand", Array, "'FirstName, LastName, Email, PhoneNumber, Gender, BirthDate'", 6, "students");
            HttpContext.Session.Set("ManageModel", BinaryConv.ObjectToByteArray(model));
            return View();
        }
        public IActionResult Classes()
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            ViewData["Title"] = "Manage Classes";
            if (HttpContext.Session.GetString("CCommand") == null)
            {
                HttpContext.Session.SetString("CCommand", "SELECT name FROM classes WHERE account_id=" + HttpContext.Session.GetInt32("UserID"));
                HttpContext.Session.SetString("CiCommand", "SELECT id FROM classes WHERE account_id=" + HttpContext.Session.GetInt32("UserID"));
            }
            ManageModel model = new ManageModel();
            RelationModel Relmodel = new RelationModel();
            string[] Array = { "Name" };
            string[] RelColumns = { "student_id" };
            string[] MainColumns = { "FirstName", "LastName", "Email", "PhoneNumber", "Gender", "BirthDate" };
            Relmodel.Set(RelColumns, MainColumns, "class_id", "student_class", "students");
            model.Set("Classes","CCommand","CiCommand", Array, "'Name'", 1, "classes", Relmodel);
            HttpContext.Session.Set("ManageModel", BinaryConv.ObjectToByteArray(model));
            return View("Students");
        }
        public IActionResult Subjects()
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            ViewData["Title"] = "Manage Subjects";
            if (HttpContext.Session.GetString("SuCommand") == null)
            {
                HttpContext.Session.SetString("SuCommand", "SELECT name FROM subjects WHERE account_id=" + HttpContext.Session.GetInt32("UserID"));
                HttpContext.Session.SetString("SuiCommand", "SELECT id FROM subjects WHERE account_id=" + HttpContext.Session.GetInt32("UserID"));
            }
            ManageModel model = new ManageModel();
            RelationModel Relmodel = new RelationModel();
            string[] Array = { "Name" };
            string[] RelColumns = { "class_id" };
            string[] MainColumns = { "Name" };
            Relmodel.Set(RelColumns, MainColumns, "subject_id", "class_subjects", "classes");
            model.Set("Subjects","SuCommand","SuiCommand", Array, "'Name'", 1, "subjects", Relmodel);
            HttpContext.Session.Set("ManageModel", BinaryConv.ObjectToByteArray(model));
            return View("Students");
        }
        #endregion
        [HttpPost]
        public IActionResult Search(string[] SearchRow, string Columns, string table, string sort)
        {
            string sqlString = "";
            var ColumnsArray = Columns.Split(", ");
            for (int i = 0; i < SearchRow.Length; i++)
            {
                if (SearchRow[i] != "")
                {
                    sqlString = sqlString + " " + ColumnsArray[i] + " LIKE '" + SearchRow[i] + "%' AND";
                }
            }
            sort = sort.Replace(" ", "");
            if (sort == "EmailAdress")
            {
                sort = "email";
            }
            string sqlcom = "SELECT " + Columns + " FROM " + table + " WHERE" + sqlString + " account_id=" + HttpContext.Session.GetInt32("UserID") + " ORDER BY " + sort;
            if (SearchRow == null)
            {
                sqlcom = "SELECT " + Columns + " FROM " + table + " WHERE account_id=" + HttpContext.Session.GetInt32("UserID") + " ORDER BY " + sort;
            }
            switch (table)
            {
                case "classes":
                    HttpContext.Session.SetString("CCommand", sqlcom);
                    break;
                case "students":
                    HttpContext.Session.SetString("SCommand", sqlcom);
                    break;
                case "subjects":
                    HttpContext.Session.SetString("SuCommand", sqlcom);
                    break;
            }
            return View();
        }
        public IActionResult LogOut()
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public dynamic Update(string[][] StudentData, int[] ids, bool[] hidden, string table, string[] Columns)
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            dynamic data = "";
            for (int i = 0; i < StudentData.Length; i++)
            {
                if(StudentData[i][0] == null){
                    continue;
                }
                if (i < ids.Length)
                {
                    if (hidden[i] == true)
                    {
                        data = GetSqlData.GetData("DELETE FROM " + table + " WHERE id=" + ids[i]);
                        if (data["Response"] != "Success")
                        {
                            return data["Error"];
                        }
                    }
                }
                if (ids.Length > i)
                {
                    var SetString = Columns[0] + "='" + StudentData[i][0]+"'";
                    for (int j = 1; j < StudentData[i].Length; j++)
                    {
                        DateTime dateValue;
                        if(DateTime.TryParse(StudentData[i][j].ToString(), out dateValue))
                        {
                            StudentData[i][j] = dateValue.ToString("yyyy-MM-dd");
                        }
                        SetString = SetString + ", " + Columns[j] + "='" + StudentData[i][j]+"'";
                    }
                    data = GetSqlData.GetData("UPDATE " + table + " SET " + SetString + " WHERE id=" + ids[i]);
                    if (data["Response"] != "Success")
                    {
                        return data["Error"];
                    }
                }
                else if (ids.Length <= i)
                {
                    string values = MakeArraySql(StudentData[i]);
                    data = GetSqlData.GetData("INSERT INTO " + table + "(" + string.Join(",", Columns) + ", account_id) VALUES(" +values + ", " + HttpContext.Session.GetInt32("UserID") + ")");
                    if (data["Response"] != "Success")
                    {
                        return data["Error"];
                    }
                }
            }
            return "Success";
        }
        [HttpPost]
        public dynamic ShowRelations(int id)
        {
            ManageModel manage = (ManageModel)BinaryConv.ByteArrayToObject(HttpContext.Session.Get("ManageModel"));
            dynamic data = GetSqlData.GetData("SELECT "+string.Join(",", manage.Relation.MainColumns)+" FROM "+manage.Relation.MainTable+" WHERE id IN"+
            "(SELECT "+string.Join(",", manage.Relation.RelColumns)+" FROM "+manage.Relation.RelTable+" WHERE "+manage.Relation.WhereColumn+" = "+id+")");
            dynamic Ids = GetSqlData.GetData("SELECT id FROM "+manage.Relation.MainTable+" WHERE id IN"+
            "(SELECT "+string.Join(",", manage.Relation.RelColumns)+" FROM "+manage.Relation.RelTable+" WHERE "+manage.Relation.WhereColumn+" = "+id+")");
            List<dynamic> returend = new List<dynamic>();
            for (int i = 0; i < data["Data"].Length/data["Columns"]; i++)
            {
                List<dynamic> Temp = new List<dynamic>();
                for (int j = 0; j < data["NewData"]["Row"+i].Length; j++)
                {
                    DateTime dateValue;
                    if(DateTime.TryParse(data["NewData"]["Row"+i][j].ToString(), out dateValue))
                    {
                        string date = dateValue.ToString("yyyy-MM-dd");
                        Temp.Add(date);
                    }else
                    {
                        Temp.Add(data["NewData"]["Row"+i][j]);
                    }
                    
                }
                returend.Add(Temp);
            }
            List<dynamic> final = new List<dynamic>();
            final.Add(returend);
            final.Add(Ids["Data"]);
            return final.ToArray();
        }
        [HttpPost]
        public dynamic AddRelations(bool subject=false)
        {
            ManageModel manage = (ManageModel)BinaryConv.ByteArrayToObject(HttpContext.Session.Get("ManageModel"));
            dynamic Ids;
            dynamic Names;
            if(!subject){
                Ids = GetSqlData.GetData("SELECT id FROM "+manage.Relation.MainTable+" WHERE id NOT IN ("+
            "SELECT "+string.Join(",", manage.Relation.RelColumns)+" FROM "+manage.Relation.RelTable+")");
                Names = GetSqlData.GetData("SELECT "+string.Join(",", manage.Relation.MainColumns)+" FROM "+manage.Relation.MainTable+" WHERE id NOT IN ("+
            "SELECT "+string.Join(",", manage.Relation.RelColumns)+" FROM "+manage.Relation.RelTable+")");
            }else{
                Ids = GetSqlData.GetData("SELECT id FROM " +manage.Relation.MainTable+" WHERE account_id="+HttpContext.Session.GetInt32("UserID"));
                Names = GetSqlData.GetData("SELECT "+string.Join(",", manage.Relation.MainColumns)+" FROM " +manage.Relation.MainTable+" WHERE account_id="+HttpContext.Session.GetInt32("UserID"));
            }
            List<dynamic> returend = new List<dynamic>();
            for (int i = 0; i < Names["Data"].Length/Names["Columns"]; i++)
            {
                List<dynamic> Row = new List<dynamic>();
                Row.Add(Ids["Data"][i]);
                Row.Add(Names["NewData"]["Row"+i]);
                returend.Add(Row);
            }
            return returend;
        }
        [HttpPost]
        public dynamic UpdateRelation(int[] ids, int class_id, bool[] hidden, int Started)
        {
            if (HttpContext.Session.GetString("LogedIn") == "false")
            {
                return RedirectToAction("SignIn", "Home");
            }
            ManageModel manage = (ManageModel)BinaryConv.ByteArrayToObject(HttpContext.Session.Get("ManageModel"));
            dynamic data = "";
            for (int i = 0; i < ids.Length; i++)
            {
                if (i <= hidden.Length)
                {
                    if (hidden[i] == true)
                    {
                        data = GetSqlData.GetData("DELETE FROM " + manage.Relation.RelTable + " WHERE "+string.Join(",", manage.Relation.RelColumns)+"=" + ids[i]);
                        if (data["Response"] != "Success")
                        {
                            return data["Error"];
                        }
                    }
                }
                if (Started <= i)
                {
                    data = GetSqlData.GetData("INSERT INTO " + manage.Relation.RelTable + "("+string.Join(",", manage.Relation.RelColumns)+", "+manage.Relation.WhereColumn+") VALUES(" +ids[i] + ", "+class_id+" )");
                    if (data["Response"] != "Success")
                    {
                        return data["Error"];
                    }
                }
            }
            return "Success";
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Settings(){
            return RedirectToAction("Index", "Settings");
        }
        string MakeArraySql(dynamic[] array){
            string values = "'"+array[0]+"'";
            for (int j = 1; j < array.Length; j++)
            {
                DateTime dateValue;
                if(DateTime.TryParse(array[j].ToString(), out dateValue))
                {
                    values += ",'"+dateValue.ToString("yyyy-MM-dd")+"'";
                }else{
                    values += ",'"+array[j]+"'";
                }
            }
            return values;
        }


    }
}