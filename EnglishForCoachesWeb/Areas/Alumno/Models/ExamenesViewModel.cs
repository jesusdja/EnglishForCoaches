using System.Collections.Generic;

using EnglishForCoachesWeb.Database.Ejercicios;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class ExamenesIndexViewModel
    {
        public List<Examen> Examenes { get; set; }

        public string TextoExamen(bool aprobado)
        {
            if (aprobado)
                return "Superado";
            else
                return "No Superado";
        }

        public string ColorResultado(bool aprobado)
        {
            if (aprobado)
                return "text-success";
            else
                return "text-danger";
        }
    }

    /********* TIPO GENERAL *********/
    public class ExamenIndexViewModel
    {
        public int examenId { get; set; }
        public SubTema subTema { get; set; }

        public List<PreguntaExamen> contenidos { get; set; }
    }
    public class PreguntaExamen
    {
        public int id { get; set; }

        public string tipo { get; set; }
        public int tiempoRestante { get; set; }
        public bool contestada { get; set; }
        public string respuesta { get; set; }
        
    }



    public class ResultadoExamen
    {
        public bool Correcto { get; set; }
        public int id { get; set; }
        public int respuesta { get; set; }
    }

    public class FinExamen
    {
        public bool Aprobado { get; set; }
        public int Correctas { get; set; }
        public int Porcentaje { get; set; }
        public int IdSiguienteLeccion { get; set; }
        public string SiguienteLeccion { get; set; }
        public bool UltimoExamen { get; set; }
    }


    /********* TIPO TEST *********/
    public class ExamenesTestIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<Test> tests { get; set; }
    }

    public class PreguntaTestExamen
    {
        public string Enunciado { get; set; }

        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta4 { get; set; }
    }

    public class RespuestaIncorrectaTestExamen
    {
        public int id { get; set; }
        public int respuesta { get; set; }
    }

    public class ResultadoTestExamen
    {
        public bool Correcto { get; set; }
        public int id { get; set; }
        public int respuesta { get; set; }
    }

    public class FinExamenTest
    {
        public bool Aprobado { get; set; }
        public int Correctas { get; set; }
        public int Porcentaje { get; set; }
        public int IdSiguienteLeccion { get; set; }
        public string SiguienteLeccion { get; set; }
        public bool UltimoExamen { get; set; }
    }



    /***** TIPO FILL THE GAP *****/
    public class ExamenesFillTheGapIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<FillTheGap> fillTheGaps { get; set; }
    }

    public class PreguntaFillTheGapExamen
    {
        public string Enunciado { get; set; }
    }

    public class ResultadoFillTheGapExamen
    {
        public bool Correcto { get; set; }

        public List<bool> correctas { get; set; }
        public int id { get; set; }
        public string respuesta { get; set; }
    }

    public class RespuestaIncorrectaFillTheGapExamen
    {
        public int id { get; set; }
        public string respuesta { get; set; }
    }
    public class FinExamenFillTheGap
    {
        public bool Aprobado { get; set; }
        public int Correctas { get; set; }
        public int Porcentaje { get; set; }
        public int IdSiguienteLeccion { get; set; }
        public string SiguienteLeccion { get; set; }
        public bool UltimoExamen { get; set; }
    }
}