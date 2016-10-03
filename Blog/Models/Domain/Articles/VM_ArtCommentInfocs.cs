using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Articles
{
    public class VM_ArtCommentInfo
    {
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public int DisLikesCount { get; set; }

        public VM_ArtCommentInfo()
        {
            CommentsCount = 0;
            LikesCount = 0;
            DisLikesCount = 0;
        }
        public void Assign(VM_ArtCommentInfo obj)
        {
            CommentsCount = obj.CommentsCount;
            LikesCount = obj.LikesCount;
            DisLikesCount = obj.DisLikesCount;
        }
    }
}