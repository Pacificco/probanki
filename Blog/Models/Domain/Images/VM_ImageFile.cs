using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Images
{
    public class VM_ImageFile
    {        
        public string FileName { get; set; }
        private string _filePath { get; set; }
        public string FilePath {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                BreadCrumbs.Clear();
                if (!String.IsNullOrEmpty(_filePath))                
                {
                    try
                    {
                        
                        string str = _filePath.Trim('\\');
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
                        while(str.Length > 0);
                    }
                    catch
                    {
                        BreadCrumbs.Clear();
                    }
                }
            }
        }
        public Dictionary<string, string> BreadCrumbs { get; set; }

        public VM_ImageFile()
        {
            BreadCrumbs = new Dictionary<string, string>();
            FileName = "";
            FilePath = "";            
        }
    }
}