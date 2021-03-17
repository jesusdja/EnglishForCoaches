using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;

namespace EnglishForCoachesWeb.Helpers
{
    public static class HtmlTextHelper
    {
        public static string QuitarEtiquetas(string texto)
        {
            string resultado = texto;
            resultado = Regex.Replace(resultado, @"<!--.*?-->", "", RegexOptions.Singleline);
            resultado = resultado.Replace("\r\n\r\n", "");
            resultado = Regex.Replace(resultado, @"<[^>]*>", "");
            return resultado;
        }
  
    }
}