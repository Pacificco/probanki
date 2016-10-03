using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Helpers
{
    public static class ConvertHelper
    {
        public static string TextToHtmlParagraphs(string text, int maxLength, ref bool moreThenMaxLength)
        {
            moreThenMaxLength = false;
            if (String.IsNullOrEmpty(text)) return "<p> </p>";
            string[] paragraphs = null;

            string result = text;
            if (maxLength > 0)
            {
                if (text.Length > maxLength)
                {
                    result = text.Substring(0, maxLength) + "...";
                    moreThenMaxLength = true;
                }
                else
                {
                    moreThenMaxLength = false;
                }
            }
            
            if (result.IndexOf("\r\n") != -1)
            {
                string[] sep = new string[] { "\r\n" };
                paragraphs = result.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                paragraphs = new string[] { result };
            }

            if (paragraphs != null && paragraphs.Length > 0)
            {
                result = "";
                foreach (string p in paragraphs)
                    result += "<p>" + p + "</p>";
            }
            else
            {
                result = "<p>" + result + "</p>";
            }
            return result.Replace("\r", "").Replace("\n", "");
        }
        public static string TextToHtmlBreaks(string text, int maxLength)
        {            
            if (String.IsNullOrEmpty(text)) return "";
            string[] paragraphs = null;

            string result = text;
            if (maxLength > 0)
            {
                if (text.Length > maxLength)
                {
                    result = text.Substring(0, maxLength) + "...";
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }

            if (result.IndexOf("\r\n") != -1)
            {
                string[] sep = new string[] { "\r\n" };
                paragraphs = result.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                paragraphs = new string[] { result };
            }

            if (paragraphs != null && paragraphs.Length > 0)
            {
                result = "";
                foreach (string p in paragraphs)
                    result += "<br />" + p;
                result = result.Substring(6);
            }
            else
            {
                return result;
            }
            return result.Replace("\r", "<br />").Replace("\n", "<br />");
        }
    }
}