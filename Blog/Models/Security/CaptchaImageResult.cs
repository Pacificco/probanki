using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Security
{
    public class CaptchaImageResult : ActionResult
    {
        private int _captchaWidth;
        private int _captchaHeight;
        private int _totalCharactersToDisplay;

        public CaptchaImageResult(int imageWidth, int imageHeight, int totalCharactersToDisplay)
        {
            _captchaWidth = imageWidth;
            _captchaHeight = imageHeight;
            _totalCharactersToDisplay = totalCharactersToDisplay;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Bitmap bmp = createBitMap(context);
            writeToOutPutStream(context, bmp);            
        }

        private static void writeToOutPutStream(ControllerContext context, Bitmap bmp)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            bmp.Save(response.OutputStream, ImageFormat.Jpeg);
            bmp.Dispose();
        }

        public Bitmap createBitMap(ControllerContext context)
        {
            Bitmap bmp = new Bitmap(_captchaWidth, _captchaHeight, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.Clear(getColor("#39b387"));
            
            string randomString = GetCaptchaString(_totalCharactersToDisplay);
            context.HttpContext.Session["captcha_code"] = randomString;
            g.DrawString(randomString, new Font("Arial", 16), getHatchBrush(), 10, 2);
            return bmp;
        }

        public string GetCaptchaString(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString = "";

            Random random = new Random(System.DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString.ToLower();
        }

        private Brush getHatchBrush()
        {
            HatchBrush hatchBrush = new HatchBrush
            (
                HatchStyle.DottedGrid,
                Color.LightGray,
                Color.White
            );
            return hatchBrush;
        }

        private Color getColor(string hexColor)
        {
            return ColorTranslator.FromHtml(hexColor);
        }
    }
}