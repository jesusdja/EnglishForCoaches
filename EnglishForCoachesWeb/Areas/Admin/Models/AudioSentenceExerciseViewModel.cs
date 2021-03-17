﻿using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EnglishForCoachesWeb.Validation;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class AudioSentenceExerciseViewModel
    {
        public AudioSentenceExercise AudioSentenceExercise { get; set; }
        public Bloque bloque { get; set; }
        
        [Display(Name = "Fichero de audio")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase AudioFile { get; set; }
        public void Inicializar(int bloqueId)
        {
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.AudioSentenceExercises).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);
        }
    }

    public class AudioSentenceExerciseCreateViewModel : AudioSentenceExerciseViewModel
    {
    }

    public class AudioSentenceExerciseEditViewModel : AudioSentenceExerciseViewModel
    {
        public int? ExamenId { get; set; }

    }
}
