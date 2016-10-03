using Bankiru.Models.Domain.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Comments
{
    public class VM_ArtComment
    {
        public VM_Article Art { get; set; }
        public VM_Comment Review { get; set; }
        public VM_ArtComment()
        {
            Art = new VM_Article();
            Review = new VM_Comment();
        }
    }
}