using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{  

    public class ExtraDesbloqueadoEstadistica
    {
        public int CateGoriaExtraId { get; set; }
        public int NExtras { get; set; }
        public int NDesbloqueados { get; set; }
        public int Porcentaje { get; set; }
    }

    public class ExtrasIndexViewModel
    {
        public SubTema Subtema { get; set; }

        public List<ExtraDesbloqueadoEstadistica> ExtraDesbloqueadoEstadisticas { get; set; }

        public List<CategoriaExtra> categorias { get; set; }

        public string ColorEstilo(int porcentaje)
        {
            //success 75-100
            //warning 25-75
            //danger 0-25
            if (porcentaje < 25)
            {
                return "danger";
            }
            if (porcentaje < 75)
            {
                return "warning";
            }
            return "success";
        }
    }


    public class ExtrasCategoriaViewModel
    {
        public SubTema Subtema { get; set; }
        public int siguienteId { get; set; }

        public ExtraDesbloqueadoEstadistica ExtraDesbloqueadoEstadistica { get; set; }
        public CategoriaExtra CategoriaExtra { get; set; }
        public List<Extra> extras { get; set; }
        public List<int> extrasDesbloqueados { get; set; }

        public string ColorEstilo(int porcentaje)
        {
            //success 75-100
            //warning 25-75
            //danger 0-25
            if (porcentaje < 25)
            {
                return "danger";
            }
            if (porcentaje < 75)
            {
                return "warning";
            }
            return "success";
        }
    }
    public class ExtrasViewViewModel
    {
        public Extra Extra { get; set; }

    }
    
}