using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Images
{
    public class VM_Folder
    {
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Вы не указали название новой категории!")]
        public string Name { get; set; }
        [HiddenInput]
        public string Path { get; set; }

        public VM_Folder()
        {
            Name = "";
            Path = "";
        }
        public VM_Folder(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}