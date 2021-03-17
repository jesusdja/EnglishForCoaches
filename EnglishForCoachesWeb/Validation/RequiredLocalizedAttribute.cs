using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace EnglishForCoachesWeb.Validation
{
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        public RequiredLocalizedAttribute()
        {
            this.ErrorMessageResourceType = typeof(Messages);
            this.ErrorMessageResourceName = "RequiredField";
        }
    }
}