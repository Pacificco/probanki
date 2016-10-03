using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Images
{
    public class VM_ImageFolder
    {
        public string BackPath { get; set; }
        private string _targetPath { get; set; }
        public string TargetPath
        {
            get
            {
                return _targetPath;
            }
            set
            {
                _targetPath = value;
                BreadCrumbs.Clear();
                if (!String.IsNullOrEmpty(_targetPath))
                {
                    try
                    {
                        string str = _targetPath.Trim('\\');
                        int i = 0;
                        string path = "";
                        do
                        {
                            i = str.IndexOf("\\");
                            if (i != -1)
                            {
                                path += str.Substring(0, i + 1);
                                BreadCrumbs.Add(str.Substring(0, i), path.Trim('\\'));
                                str = str.Substring(i + 1);
                            }
                            else
                            {
                                path += str;
                                BreadCrumbs.Add(str, path);
                                str = "";
                                break;
                            }
                        }
                        while (str.Length > 0);
                    }
                    catch
                    {
                        BreadCrumbs.Clear();
                    }
                }
            }
        }
        public List<string> Folders { get; set; }
        public List<string> Files { get; set; }
        public Dictionary<string, string> BreadCrumbs { get; set; }

        public VM_ImageFolder()
        {
            BreadCrumbs = new Dictionary<string, string>();
            BackPath = "";
            _targetPath = "";
            Folders = new List<string>();
            Files = new List<string>();
            
        }
    }
}