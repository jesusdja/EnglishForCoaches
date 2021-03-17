using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Validation
{
    public class RespuestasFillTheGapAttribute : ValidationAttribute
    {
        private readonly string _respuesta;

        public RespuestasFillTheGapAttribute(string respuesta)
        {
            _respuesta = respuesta;
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;
            var otherValue = validationContext.ObjectType.GetProperty(_respuesta).GetValue(validationContext.ObjectInstance, null);

            int TotalRespuestasEnunciado=0;
            if (otherValue != null)
            {
                TotalRespuestasEnunciado = otherValue.ToString().Split('#').Count() - 1;

                int TotalRespuestas;
                if (value != null)
                {
                    TotalRespuestas = value.ToString().Split('#').Count();

                    if (otherValue == null || String.IsNullOrEmpty(otherValue.ToString()))
                    {
                        if (value != null)
                        {
                            result = new ValidationResult("Debe rellenar");
                        }
                    }
                    else
                    {
                        if (TotalRespuestasEnunciado != TotalRespuestas)
                        {
                            result = new ValidationResult("El número no coincide");
                        }
                    }

                }
                else
                {
                    result = new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                result = new ValidationResult(ErrorMessage);
            }

            return result;        
        }

        public override string FormatErrorMessage(string name)
        {
            string format = base.ErrorMessageString;
            return String.Format(CultureInfo.CurrentCulture, format, new object[] { _respuesta });
        }
    }
}