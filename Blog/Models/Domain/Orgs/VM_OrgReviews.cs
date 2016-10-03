using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgReviews
    {
        public VM_Org Org { get; set; }
        public List<VM_Review> Items { get; set; }
        public VM_OrgsReviewsFilters Filters { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }
        public VM_ReviewResponse FormReviewData { get; set; }

        public VM_OrgReviews()
        {
            Org = new VM_Org();
            Items = new List<VM_Review>();
            Filters = new VM_OrgsReviewsFilters();
            PagingInfo = new VM_PagingInfo();
            FormReviewData = null;
        }
    }
}