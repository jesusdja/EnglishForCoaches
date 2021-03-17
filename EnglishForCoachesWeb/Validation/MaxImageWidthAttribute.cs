using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Validation
{
    public class MinImageSizeAttribute : ValidationAttribute
    {
        private readonly int _minImageWidth;
        private readonly int _minImageHeight;
        public MinImageSizeAttribute(int minImageWidth, int minImageHeight)
        {
            _minImageWidth = minImageWidth;
            _minImageHeight = minImageHeight;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return true;
            }
            var image = Image.FromStream(file.InputStream, true, true);
            return (image.Width >= _minImageWidth) && (image.Height >= _minImageHeight);
        }

        public override string FormatErrorMessage(string name)
        {
            string format = base.ErrorMessageString;
            return String.Format(CultureInfo.CurrentCulture, format, new object[] { _minImageWidth, _minImageHeight });
        }
    }
}