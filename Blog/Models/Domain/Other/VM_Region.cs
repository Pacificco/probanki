using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Other
{
    public class VM_Region
    {
        public Guid AoGuid{ get; set; }
        public string FormalName{ get; set; }
        public string ShortName { get; set; }

    }
}