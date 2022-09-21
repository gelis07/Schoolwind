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
    [Serializable]
    public class RelationModel
    {
        public string[] RelColumns;
        public string[] MainColumns;
        public string WhereColumn;
        public string RelTable;
        public string MainTable;
        public void Set(string[] CRelColumns, string[] CMainColumns, string CWhereColumn, string CRelTable, string CMainTable){
            RelColumns = CRelColumns;
            MainColumns = CMainColumns;
            WhereColumn = CWhereColumn;
            RelTable = CRelTable;
            MainTable = CMainTable;
        }
    }
}