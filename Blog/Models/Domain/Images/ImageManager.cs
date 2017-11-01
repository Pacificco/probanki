using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Images
{
    public class ImageManager
    {
        public string LastError = "";
        private static string[] _allowFormats = new string[] { ".jpg", ".png", ".gif" };        
        
        public VM_ImageFolder GetFolder(string basePath, string path)
        {
            try
            {
                VM_ImageFolder result = new VM_ImageFolder();
                string folderBase = basePath;
                if (!String.IsNullOrEmpty(path))
                {
                    result.TargetPath = path;
                    folderBase += path;
                    int i = path.LastIndexOf("\\");
                    if (i != -1)
                        result.BackPath = path.Substring(0, i);
                    else
                        result.BackPath = "";
                }
                DirectoryInfo dir = new DirectoryInfo(folderBase);
                DirectoryInfo[] folders = dir.GetDirectories();
                if (folders != null && folders.Length > 0)
                {
                    //VM_Folder folder = null;
                    foreach (var item in dir.GetDirectories())
                    {
                        //folder = new VM_Folder(item.Name);
                        result.Folders.Add(item.Name);
                    }
                }

                FileInfo[] files = dir.GetFiles();
                if (files != null && files.Length > 0)
                {
                    foreach (var item in dir.GetFiles())
                    {
                        if(_isFileValidate(item.Name))
                            result.Files.Add(item.Name);
                    }
                }

                return result;
            }
            catch(Exception ex)
            {
                LastError = ex.ToString();
                return null;
            }
        }
        public bool CreateFile(HttpPostedFileBase file, string folderPath, bool createPreView = true)
        {
            try
            {
                string filename = Path.GetFileName(file.FileName);
                if (!_isFileValidate(filename))
                {
                    LastError = String.Format("Файл {0} не является изображением!", filename);
                    return false;
                }
                string fileSavePath = Path.Combine(folderPath, filename);
                if (File.Exists(fileSavePath))
                {
                    string name = filename.Substring(0, filename.LastIndexOf("."));
                    string format = filename.Substring(filename.LastIndexOf("."));
                    string newFileSavePath = "";
                    int i = 2;
                    do
                    {
                        filename = name + "_" + i.ToString() + format;
                        newFileSavePath = Path.Combine(folderPath, filename);
                        i++;
                    }
                    while (File.Exists(newFileSavePath));
                    fileSavePath = newFileSavePath;
                }
                file.SaveAs(fileSavePath);
                //Создание миниатюры для предпросмотра
                if (createPreView)
                {
                    string thumbsPath = folderPath + "thumbs";
                    if (!Directory.Exists(thumbsPath))
                        Directory.CreateDirectory(thumbsPath);
                    Image image = Bitmap.FromFile(fileSavePath);                    
                    Rectangle rect = new Rectangle(0, 0, 178, 178);
                    if (image.Width > image.Height)
                    {
                        double r = (double)image.Height / (double)image.Width;
                        rect.Height = (int)(178.0 * r);
                    }
                    else if (image.Width < image.Height)
                    {
                        double r = (double)image.Width / (double)image.Height;
                        rect.Width = (int)(178.0 * r);
                    }
                    Bitmap bp = new Bitmap(rect.Width, rect.Height);
                    using (Graphics g = Graphics.FromImage(bp))
                    {
                        //Сглаживание и интерполяция
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        //Изменение размера                                                
                        g.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                    }
                    fileSavePath = Path.Combine(thumbsPath, filename);
                    if (File.Exists(fileSavePath))
                    {
                        string name = filename.Substring(0, filename.LastIndexOf("."));
                        string format = filename.Substring(filename.LastIndexOf("."));
                        string newFileSavePath = "";
                        int i = 2;
                        do
                        {
                            filename = name + "_" + i.ToString() + format;
                            newFileSavePath = Path.Combine(thumbsPath, filename);
                            i++;
                        }
                        while (File.Exists(newFileSavePath));
                        fileSavePath = newFileSavePath;
                    }
                    bp.Save(fileSavePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex.ToString();
                return false;
            }
        }

        #region ПРИВАТНАЯ ЧАСТЬ
        private bool _isFileValidate(string fileName)
        {
            foreach (string f in _allowFormats)
                if (fileName.ToLower().IndexOf(f) != -1)
                    return true;
            return false;
        }
        #endregion
    }

    public class ImageData
    {
        private byte[] _data;
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Id { get; set; }
        public byte[] Data
        {
            get { return _data; }
        }

        public ImageData(string fileName, string contentType, byte[] data)
        {
            FileName = fileName;
            ContentType = contentType;
            _data = data;
            Id = Guid.NewGuid().ToString();
        }
    }
}