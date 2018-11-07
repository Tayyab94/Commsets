using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalProject.Models
{
    public class DDLViewModel
    {
        public string Name { get; set; }

        public string Caption { get; set; }

        public string Glyphicon { get; set; }

        public List<SelectListItem> values { get; set; }
    }
}