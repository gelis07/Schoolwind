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
    public class ManageModel
    {
        public string Name;
        public string InsCommand;
        public string IdCommand;
        public string[] columns;
        public string[] Sqlcolumns;
        public string Search;
        public string table;
        public int SearchColumns;
        public RelationModel Relation;

        public void Set(string CName,string CInsCommand,string CIdCommand, string[] Ccolumns, string CSearch,int CSearchColumns, string Ctable, RelationModel CRelation=null){
            Name = CName;
            InsCommand = CInsCommand;
            IdCommand = CIdCommand;
            columns = Ccolumns;
            Search = CSearch;
            table = Ctable;
            SearchColumns = CSearchColumns;
            Relation = CRelation;
            Sqlcolumns = CSearch.Replace("'","").Split(", ");
        }
    }
}