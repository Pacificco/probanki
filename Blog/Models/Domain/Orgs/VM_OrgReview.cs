using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgReview
    {
        public VM_Org Org { get; set; }
        public VM_Review Review { get; set; }
        public VM_OrgReview()
        {
            Org = new VM_Org();
            Review = new VM_Review();
        }
    }
}