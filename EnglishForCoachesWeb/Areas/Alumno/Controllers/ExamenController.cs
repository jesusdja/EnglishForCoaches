using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Helpers;
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class ExamenController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        public ActionResult Index()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            var _examenes = db.Examenes.Include(e => e.SubTema).Include(e => e.SubTema.Tema).Where(e => e.AlumnoId == userId).OrderByDescending(e => e.FechaExamen).ToList();

            ExamenesIndexViewModel _model = new ExamenesIndexViewModel();
            _model.Examenes = _examenes;

            return View(_model);
        }
        
        public ActionResult Examen(int id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(st => st.Tema).FirstOrDefault(b => b.SubTemaId == id);
            if (subtema == null)
                return HttpNotFound();

            if (!ComprobarAccesoExamen(id))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

            List<PreguntaExamen> contenidos = db.Tests.Include(t => t.TipoEjercicio).Where(sk => sk.SubTemaId == subtema.SubTemaId && sk.PreguntaExamen.HasValue ? sk.PreguntaExamen.Value : false).Select(t => new PreguntaExamen() { id = t.Id, tipo = t.TipoEjercicio.Controlador }).ToList();
            contenidos.AddRange(db.FillTheGaps.Include(t => t.TipoEjercicio).Where(sk => sk.SubTemaId == subtema.SubTemaId && sk.PreguntaExamen.HasValue ? sk.PreguntaExamen.Value : false).Select(t => new PreguntaExamen() { id = t.Id, tipo = t.TipoEjercicio.Controlador }).ToList());
            contenidos.AddRange(db.TrueFalses.Include(t => t.TipoEjercicio).Where(sk => sk.SubTemaId == subtema.SubTemaId && sk.PreguntaExamen.HasValue ? sk.PreguntaExamen.Value : false).Select(t => new PreguntaExamen() { id = t.Id, tipo = t.TipoEjercicio.Controlador }).ToList());
            contenidos.AddRange(db.AudioSentenceExercises.Include(t => t.TipoEjercicio).Where(sk => sk.SubTemaId == subtema.SubTemaId && sk.PreguntaExamen.HasValue ? sk.PreguntaExamen.Value : false).Select(t => new PreguntaExamen() { id = t.Id, tipo = t.TipoEjercicio.Controlador }).ToList());


            contenidos.Shuffle();

            ExamenIndexViewModel _modelTest = new ExamenIndexViewModel();
            _modelTest.subTema = subtema;
            _modelTest.contenidos = contenidos.Take(30).ToList();
            _modelTest.examenId = id;
            
            return View("Examen", _modelTest);


        }

        public JsonResult FinExamen(int id, PreguntaExamen[] respuestas)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            int aciertos = 0;
            int totales = respuestas.Length;

            List<RespuestaIncorrecta> respuestasIncorrectas = new List<RespuestaIncorrecta>();

            foreach (var pregunta in respuestas)
            {
                bool acertada = false;
                string enunciado = "";
                string respuestaCorrecta = "";
                string respuestaSeleccionada = "";


                if (pregunta.tipo == "Tests")
                {
                    Test test = db.Tests.Find(pregunta.id);
                    if (pregunta.respuesta != null && test.RespuestaCorrecta.ToString() == pregunta.respuesta)
                    {
                        aciertos++;
                        acertada = true;
                    }
                    else
                    {
                        enunciado = test.Enunciado;
                        respuestaCorrecta = ObtenerRespuestaTest(test, test.RespuestaCorrecta);
                        respuestaSeleccionada = ObtenerRespuestaTest(test, Convert.ToInt32(pregunta.respuesta));
                    }
                }
                if (pregunta.tipo == "FillTheGaps")
                {
                    if (pregunta.respuesta != null) {
                        pregunta.respuesta = pregunta.respuesta.Replace(',', '#');
                    }
                    FillTheGap fillTheGap = db.FillTheGaps.Find(pregunta.id);
                    if (pregunta.respuesta != null && fillTheGap.Respuestas.ToLower() == pregunta.respuesta.ToLower())
                    {
                        aciertos++;
                        acertada = true;
                    }
                    else
                    {
                        enunciado = fillTheGap.Enunciado;
                        respuestaCorrecta = fillTheGap.Respuestas.Replace('#', ' ');
                        if (pregunta.respuesta != null)
                        {
                            respuestaSeleccionada = pregunta.respuesta.Replace('#', ' ');
                        }
                    }
                }
                if (pregunta.tipo == "TrueFalses")
                {
                    TrueFalse TrueFalse = db.TrueFalses.Find(pregunta.id);
                    if (pregunta.respuesta != null && TrueFalse.RespuestaCorrecta.ToString() == pregunta.respuesta)
                    {
                        aciertos++;
                        acertada = true;
                    }
                    else
                    {
                        enunciado = TrueFalse.Enunciado;
                        respuestaCorrecta = TrueFalse.RespuestaCorrecta.ToString();
                        respuestaSeleccionada = pregunta.respuesta;
                    }
                }
                if (pregunta.tipo == "AudioSentenceExercises") {
                    if (pregunta.respuesta != null) {
                        pregunta.respuesta = pregunta.respuesta.Replace(',', '#');
                    }
                    AudioSentenceExercise audioSentence = db.AudioSentenceExercises.Find(pregunta.id);
                    if (pregunta.respuesta!=null && audioSentence.Respuestas.ToLower() == pregunta.respuesta.ToLower())
                    {
                        aciertos++;
                        acertada = true;
                    }
                    else
                    {
                        enunciado = audioSentence.Enunciado;
                        respuestaCorrecta = audioSentence.Respuestas.Replace('#', ' ');
                        if (pregunta.respuesta != null)
                        {
                            respuestaSeleccionada = pregunta.respuesta.Replace('#', ' ');
                        }
                    }
                }

                if (!acertada)
                {
                    RespuestaIncorrecta respuestaInc = new RespuestaIncorrecta();
                    respuestaInc.Pregunta = enunciado;
                    respuestaInc.RespuestaCorrecta = respuestaCorrecta;
                    respuestaInc.RespuestaSeleccionada = respuestaSeleccionada;
                    respuestaInc.Tipo = pregunta.tipo;
                    respuestaInc.PreguntaId = pregunta.id;
                    respuestasIncorrectas.Add(respuestaInc);
                }
            }


            bool _aprobado = (aciertos >= 28);




            FinExamenTest resultado = new Models.FinExamenTest();
            resultado.Correctas = aciertos;
            resultado.Aprobado = _aprobado;
            resultado.Porcentaje = (aciertos * 100 / totales);
            resultado.IdSiguienteLeccion = 0;
            resultado.SiguienteLeccion = "";


            SubTema _subtemaActual = SubTemaDataAccess.ObtenerSubTemas(db).FirstOrDefault(s => s.SubTemaId == id);
            SubTema _siguienteTema = SubTemaDataAccess.ObtenerSubTemas(db).Where(s => s.TemaId == _subtemaActual.TemaId && s.Orden > _subtemaActual.Orden).OrderBy(s => s.Orden).FirstOrDefault();

            resultado.UltimoExamen = true;
            if (_siguienteTema != null)
            {
                resultado.UltimoExamen = false;
                if (_aprobado)
                {
                    resultado.IdSiguienteLeccion = _siguienteTema.SubTemaId;
                    resultado.SiguienteLeccion = _siguienteTema.Descripcion;



                    var BloquearSubtemas = ((ClaimsIdentity)User.Identity).FindFirst("BloquearSubtemas").Value;
                    bool anyadir = true;
                    if (Convert.ToBoolean(BloquearSubtemas))
                    {
                        var listaSubtemasAcceso = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == userId).Select(sau => sau.SubTemaId).ToList();
                        if (!listaSubtemasAcceso.Contains(_siguienteTema.SubTemaId))
                        {
                            anyadir = false;
                        }
                    }
                    if (anyadir)
                    {
                        SubTemaDesbloqueado _desbloqueado = new SubTemaDesbloqueado
                        {
                            AlumnoId = userId,
                            FechaDesbloqueo = DateTime.Now,
                            SubTemaId = _siguienteTema.SubTemaId
                        };
                        db.SubTemaDesbloqueados.Add(_desbloqueado);
                        db.SaveChanges();
                    }
                }
            }

            var NombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst("NombreUsuario").Value;
            Examen _examen = new Examen
            {
                Aciertos = aciertos,
                AlumnoId = userId,
                NombreAlumno = NombreUsuario,
                Aprobado = _aprobado,
                SubTemaId = id,
                Fallos = totales-aciertos,
                FechaExamen = DateTime.Now,
                Total = totales
            };
            _examen.RespuestasIncorrectas = respuestasIncorrectas;

            db.Examenes.Add(_examen);
            db.SaveChanges();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        #region ********* TIPO TEST *********

        public JsonResult GetTest(int id)
        {
            Test test = db.Tests.Find(id);
            PreguntaTestExamen pregunta = new PreguntaTestExamen()
            {
                Enunciado = test.Enunciado,
                Respuesta1 = test.Respuesta1,
                Respuesta2 = test.Respuesta2,
                Respuesta3 = test.Respuesta3,
                Respuesta4 = test.Respuesta4
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContestarTest(int id, int respuesta)
        {
            Test test = db.Tests.Find(id);
            ResultadoTestExamen resultado = new ResultadoTestExamen();
            if (test.RespuestaCorrecta == respuesta)
                resultado.Correcto = true;
            else
                resultado.Correcto = false;
            resultado.id = id;
            resultado.respuesta = respuesta;
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinExamenTest(int id, int aciertos, int errores, int totales, RespuestaIncorrectaTestExamen[] respuestasIncorrectas)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            Test _test = db.Tests.Find(id);

            bool _aprobado = (aciertos >= 28);

            FinExamenTest resultado = new Models.FinExamenTest();
            resultado.Correctas = aciertos;
            resultado.Aprobado = _aprobado;
            resultado.Porcentaje = (aciertos * 100 / totales);
            resultado.IdSiguienteLeccion = 0;
            resultado.SiguienteLeccion = "";

            SubTema _tema = SubTemaDataAccess.ObtenerSubTemas(db).Where(s => s.SubTemaId == _test.SubTemaId).FirstOrDefault();
            SubTema _siguienteTema = SubTemaDataAccess.ObtenerSubTemas(db).Where(s => s.TemaId == _tema.TemaId && s.Orden> _tema.Orden ).OrderBy(s=>s.Orden).FirstOrDefault();

            resultado.UltimoExamen = true;
            if (_siguienteTema != null)
            {
                resultado.UltimoExamen = false;
                if (_aprobado)
                {
                    resultado.IdSiguienteLeccion = _siguienteTema.SubTemaId;
                    resultado.SiguienteLeccion = _siguienteTema.Descripcion;



                    var BloquearSubtemas = ((ClaimsIdentity)User.Identity).FindFirst("BloquearSubtemas").Value;
                    bool anyadir = true;
                    if (Convert.ToBoolean(BloquearSubtemas))
                    {
                        var listaSubtemasAcceso = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == userId).Select(sau => sau.SubTemaId).ToList();
                        if (!listaSubtemasAcceso.Contains(_siguienteTema.SubTemaId))
                        {
                            anyadir = false;
                        }
                    }
                    if (anyadir)
                    {
                        SubTemaDesbloqueado _desbloqueado = new SubTemaDesbloqueado
                        {
                            AlumnoId = userId,
                            FechaDesbloqueo = DateTime.Now,
                            SubTemaId = _siguienteTema.SubTemaId
                        };
                        db.SubTemaDesbloqueados.Add(_desbloqueado);
                        db.SaveChanges();
                    }
                }
            }

            var NombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst("NombreUsuario").Value;
            Examen _examen = new Examen
            {
                Aciertos = aciertos,
                AlumnoId = userId,
                NombreAlumno = NombreUsuario,
                Aprobado = _aprobado,
                Fallos = errores,
                FechaExamen = DateTime.Now,
                Total = totales
            };
            List<RespuestaIncorrecta> respuestas = new List<RespuestaIncorrecta>();
            foreach (RespuestaIncorrectaTestExamen respuestaIncorrecta in respuestasIncorrectas)
            {
                Test test = db.Tests.Find(respuestaIncorrecta.id);
                RespuestaIncorrecta respuestaInc = new RespuestaIncorrecta();
                respuestaInc.Pregunta = test.Enunciado;
                respuestaInc.RespuestaCorrecta = ObtenerRespuestaTest(test,test.RespuestaCorrecta);
                respuestaInc.RespuestaSeleccionada = ObtenerRespuestaTest(test, respuestaIncorrecta.respuesta);
                respuestas.Add(respuestaInc);
            }
            _examen.RespuestasIncorrectas = respuestas;

            db.Examenes.Add(_examen);
            db.SaveChanges();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        private string ObtenerRespuestaTest(Test test, int respuesta)
        {
            switch (respuesta)
            {
                case 1:
                    return test.Respuesta1;

                case 2:
                    return test.Respuesta2;

                case 3:
                    return test.Respuesta3;

                case 4:
                    return test.Respuesta4;
                default:
                    return "";
            }
        }

        #endregion

        #region ***** TIPO FILL THE GAP *****

        public JsonResult GetFillTheGap(int id)
        {
            FillTheGap fillTheGap = db.FillTheGaps.Find(id);
            PreguntaFillTheGapExamen pregunta = new PreguntaFillTheGapExamen()
            {
                Enunciado = fillTheGap.Enunciado
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContestarFillTheGap(int id, string respuesta)
        {
            respuesta = respuesta.Replace(',', '#');
            FillTheGap fillTheGap = db.FillTheGaps.Find(id);
            ResultadoFillTheGapExamen resultado = new ResultadoFillTheGapExamen();
            if (fillTheGap.Respuestas.ToLower() == respuesta.ToLower())
                resultado.Correcto = true;
            else
            {
                resultado.Correcto = false;
                string[] _arraySoluciones = fillTheGap.Respuestas.ToLower().Split('#');
                string[] _arrayRespuestas = respuesta.ToLower().Split('#');

                resultado.correctas = new List<bool>();
                for (int i = 0; i < _arraySoluciones.Length; i++)
                    resultado.correctas.Add((_arraySoluciones[i] == _arrayRespuestas[i]));
            }
            resultado.id = id;
            resultado.respuesta = respuesta;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinExamenFillTheGap(int id, int aciertos, int errores, int totales, RespuestaIncorrectaFillTheGapExamen[] respuestasIncorrectas)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            FillTheGap _fillTheGap = db.FillTheGaps.Find(id);

            bool _aprobado = (aciertos >= 28);

            FinExamenFillTheGap resultado = new FinExamenFillTheGap();
            resultado.Correctas = aciertos;
            resultado.Aprobado = _aprobado;
            resultado.Porcentaje = (aciertos * 100 / totales);
            resultado.IdSiguienteLeccion = 0;
            resultado.SiguienteLeccion = "";

            SubTema _tema = SubTemaDataAccess.ObtenerSubTemas(db).Where(s => s.SubTemaId == _fillTheGap.SubTemaId).FirstOrDefault();
            SubTema _siguienteTema = SubTemaDataAccess.ObtenerSubTemas(db).Where(s => s.TemaId == _tema.TemaId && s.Orden > _tema.Orden).OrderBy(s => s.Orden).FirstOrDefault();

            resultado.UltimoExamen = true;
            if (_siguienteTema != null)
            {
                resultado.UltimoExamen = false;
                if (_aprobado)
                {

                    resultado.IdSiguienteLeccion = _siguienteTema.SubTemaId;
                    resultado.SiguienteLeccion = _siguienteTema.Descripcion;

                    var BloquearSubtemas = ((ClaimsIdentity)User.Identity).FindFirst("BloquearSubtemas").Value;
                    bool anyadir = true;
                    if (Convert.ToBoolean(BloquearSubtemas))
                    {
                        var listaSubtemasAcceso = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == userId).Select(sau => sau.SubTemaId).ToList();
                        if (!listaSubtemasAcceso.Contains(_siguienteTema.SubTemaId))
                        {
                            anyadir = false;
                        }
                    }
                    if (anyadir)
                    {
                        SubTemaDesbloqueado _desbloqueado = new SubTemaDesbloqueado
                        {
                            AlumnoId = userId,
                            FechaDesbloqueo = DateTime.Now,
                            SubTemaId = _siguienteTema.SubTemaId
                        };
                        db.SubTemaDesbloqueados.Add(_desbloqueado);
                        db.SaveChanges();
                    }
                }
            }

            var NombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst("NombreUsuario").Value;
            Examen _examen = new Examen
            {
                Aciertos = aciertos,
                AlumnoId = userId,
                NombreAlumno= NombreUsuario,
                Aprobado = _aprobado,
                Fallos = errores,
                FechaExamen = DateTime.Now,
                Total = totales
            };

            List<RespuestaIncorrecta> respuestas = new List<RespuestaIncorrecta>();
            foreach (RespuestaIncorrectaFillTheGapExamen respuestaIncorrecta in respuestasIncorrectas)
            {
                FillTheGap fillTheGap = db.FillTheGaps.Find(respuestaIncorrecta.id);
                RespuestaIncorrecta respuestaInc = new RespuestaIncorrecta();
                respuestaInc.Pregunta = fillTheGap.Enunciado;
                respuestaInc.RespuestaCorrecta = fillTheGap.Respuestas.Replace('#',' ');
                respuestaInc.RespuestaSeleccionada =  respuestaIncorrecta.respuesta.Replace('#', ' ');
                respuestas.Add(respuestaInc);
            }
            _examen.RespuestasIncorrectas = respuestas;



            db.Examenes.Add(_examen);
            db.SaveChanges();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public JsonResult Desbloquear(int id)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            AuthRepository authRepository = new AuthRepository();
            ApplicationUser user = authRepository.FindByName(User.Identity.Name);
            string resultado = "";
            if (user.PuntosActual >= 100)
            {
                user.PuntosActual = user.PuntosActual - 100;

                var userResult = authRepository.Update(user);

                ExamenDesbloqueado exDes = new ExamenDesbloqueado()
                {
                    AlumnoId = userId,
                    SubTemaId = id,
                    FechaDesbloqueo = DateTime.Now
                };

                db.ExamenDesbloqueados.Add(exDes);
                db.SaveChanges();
                resultado = "Desbloqueado";
            }
            else
            {
                resultado = "NoPuntos";
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}