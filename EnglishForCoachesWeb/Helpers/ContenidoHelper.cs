using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Data;
using System.Data.Entity;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Helpers
{
    public static class ContenidoHelper
    {
        public static bool MarcarEjercicioHecho(int bloqueId)
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            bool noMostrarMistakes = false;
            var yaRealizado = db.BloqueRealizados.FirstOrDefault(br=>br.BloqueId==bloqueId && br.AlumnoId == userId);
            if (yaRealizado==null)
            {
                noMostrarMistakes = true;
                IniciarMistakes(bloqueId);
            }else
            {
                var tieneMistakes = db.Mistakes.Count(br => br.BloqueId == bloqueId && br.AlumnoId == userId)>0;
                if (!tieneMistakes)
                {
                    noMostrarMistakes = true;
                    IniciarMistakes(bloqueId);
                }

            }

            BloqueRealizado bloqueRealizado = new BloqueRealizado()
            {
                BloqueId = bloqueId,
                AlumnoId = userId,
                FechaRealizado = DateTime.Now
            };
            db.BloqueRealizados.Add(bloqueRealizado);
            db.SaveChanges();
            return noMostrarMistakes;
        }

        internal static List<PreguntaMistake> ObtenerPreguntasMistakes(List<Mistake> mistakes)
        {
            AuthContext db = new AuthContext();

            List<int> listaBloquesIds = mistakes.Select(mis=>mis.BloqueId).Distinct().ToList();

            var bloquesIds = db.Tests.Where(test =>
             listaBloquesIds.Contains(test.BloqueId)).Select(test=>test.BloqueId).ToList();
            List<int> listaPreguntasIds = mistakes.Where(mis=> bloquesIds.Contains(mis.BloqueId))
                .Select(mis => mis.PreguntaId).ToList();
            List<PreguntaMistake> preguntasMistakes = db.Tests.Where(test =>
            listaPreguntasIds.Contains(test.Id)).Include(test => test.Bloque.SubTema.Tema).Include(test=>test.Bloque.SubTema.Tema).Include(test => test.Area)
            .Select(cont =>new PreguntaMistake() { bloque=cont.Bloque,
                enunciado =cont.Descripcion,
            bloqueId=cont.BloqueId,
            preguntaId=cont.Id,
                area = cont.Bloque.Area.Descripcion,
                subtema = cont.Bloque.SubTema.Descripcion,
                tema = cont.Bloque.SubTema.Tema.Descripcion,
                controlador = cont.Bloque.TipoEjercicio.Controlador
            }).ToList();

             bloquesIds = db.Skillwises.Where(test =>
             listaBloquesIds.Contains(test.BloqueId)).Select(test => test.BloqueId).ToList();
             listaPreguntasIds = mistakes.Where(mis => bloquesIds.Contains(mis.BloqueId))
                .Select(mis => mis.PreguntaId).ToList();

            preguntasMistakes.AddRange(db.Skillwises.Where(test =>
            listaPreguntasIds.Contains(test.Id)).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Area)
            .Select(cont => new PreguntaMistake() { bloque = cont.Bloque,
                enunciado = cont.Descripcion,
                bloqueId = cont.BloqueId,
                preguntaId = cont.Id,
                area = cont.Bloque.Area.Descripcion,
                subtema = cont.Bloque.SubTema.Descripcion,
                tema = cont.Bloque.SubTema.Tema.Descripcion,
                controlador = cont.Bloque.TipoEjercicio.Controlador
            }).ToList());
            
             bloquesIds = db.FillTheGaps.Where(test =>
             listaBloquesIds.Contains(test.BloqueId)).Select(test => test.BloqueId).ToList();
             listaPreguntasIds = mistakes.Where(mis => bloquesIds.Contains(mis.BloqueId))
                .Select(mis => mis.PreguntaId).ToList();
            preguntasMistakes.AddRange(db.FillTheGaps.Where(test =>
            listaPreguntasIds.Contains(test.Id)).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Area)
            .Select(cont => new PreguntaMistake() { bloque = cont.Bloque,
                enunciado = cont.Descripcion,
                bloqueId = cont.BloqueId,
                preguntaId = cont.Id,
                area = cont.Bloque.Area.Descripcion,
                subtema = cont.Bloque.SubTema.Descripcion,
                tema = cont.Bloque.SubTema.Tema.Descripcion,
                controlador = cont.Bloque.TipoEjercicio.Controlador
            }).ToList());


             bloquesIds = db.AudioSentenceExercises.Where(test =>
             listaBloquesIds.Contains(test.BloqueId)).Select(test => test.BloqueId).ToList();
            listaPreguntasIds = mistakes.Where(mis => bloquesIds.Contains(mis.BloqueId))
                .Select(mis => mis.PreguntaId).ToList();
            preguntasMistakes.AddRange(db.AudioSentenceExercises.Where(test =>
            listaPreguntasIds.Contains(test.Id)).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Area)
            .Select(cont => new PreguntaMistake() { bloque = cont.Bloque,
                enunciado = cont.Descripcion,
                bloqueId = cont.BloqueId,
                preguntaId = cont.Id,
                area = cont.Bloque.Area.Descripcion,
                subtema = cont.Bloque.SubTema.Descripcion,
                tema = cont.Bloque.SubTema.Tema.Descripcion,
                controlador = cont.Bloque.TipoEjercicio.Controlador
            }).ToList());

             bloquesIds = db.WordByWords.Where(test =>
             listaBloquesIds.Contains(test.BloqueId)).Select(test => test.BloqueId).ToList();
             listaPreguntasIds = mistakes.Where(mis => bloquesIds.Contains(mis.BloqueId))
                .Select(mis => mis.PreguntaId).ToList();
            preguntasMistakes.AddRange(db.WordByWords.Where(test =>
            listaPreguntasIds.Contains(test.Id)).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Area)
            .Select(cont => new PreguntaMistake() { bloque = cont.Bloque,
                enunciado = cont.Bloque.Descripcion,
                bloqueId = cont.BloqueId,
                preguntaId = cont.Id,
                area = cont.Bloque.Area.Descripcion,
                subtema = cont.Bloque.SubTema.Descripcion,
                tema = cont.Bloque.SubTema.Tema.Descripcion,
                controlador = cont.Bloque.TipoEjercicio.Controlador
            }).ToList());

             bloquesIds = db.TrueFalses.Where(test =>
             listaBloquesIds.Contains(test.BloqueId)).Select(test => test.BloqueId).ToList();
             listaPreguntasIds = mistakes.Where(mis => bloquesIds.Contains(mis.BloqueId))
                .Select(mis => mis.PreguntaId).ToList();
            preguntasMistakes.AddRange(db.TrueFalses.Where(test =>
            listaPreguntasIds.Contains(test.Id)).Include(test => test.Bloque.SubTema).Include(test => test.Bloque.SubTema.Tema).Include(test => test.Area)
            .Select(cont => new PreguntaMistake() { bloque = cont.Bloque,
                enunciado = cont.Descripcion,
                bloqueId = cont.BloqueId,
                preguntaId = cont.Id,
                area=cont.Bloque.Area.Descripcion,
                subtema= cont.Bloque.SubTema.Descripcion,
                tema = cont.Bloque.SubTema.Tema.Descripcion,
                controlador=cont.Bloque.TipoEjercicio.Controlador
            }).ToList());
            return preguntasMistakes;
        }

        public static List<BloqueRealizado> ObtenerEjercicioHecho(int subtemaId, int areaId)
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            var bloques = db.Bloques.Where(bl => bl.SubTemaId == subtemaId && bl.AreaId == areaId).Select(bl => bl.BloqueId).ToList();

            return db.BloqueRealizados.Where(br => br.AlumnoId == userId && bloques.Contains(br.BloqueId)).ToList();
        }
        public static List<Mistake> ObtenerEjerciciosConMistakes(int subtemaId, int areaId)
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            var bloques = db.Bloques.Where(bl => bl.SubTemaId == subtemaId && bl.AreaId == areaId).Select(bl => bl.BloqueId).ToList();

            return db.Mistakes.Where(br => br.AlumnoId == userId && bloques.Contains(br.BloqueId)).ToList();
        }

        public static List<Mistake> ObtenerMistakes()
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            var bloques = BloqueDataAccess.ObtenerBloques(db).Select(bloque=>bloque.BloqueId);

            return db.Mistakes.Where(br => br.AlumnoId == userId && bloques.Contains(br.BloqueId)).ToList();
        }

        private static void IniciarMistakes(int bloqueId)
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            var mistakes = db.Mistakes.Where(mist => mist.BloqueId == bloqueId && mist.AlumnoId == userId).ToList();
            db.Mistakes.RemoveRange(mistakes);
            db.SaveChanges();

            List<int> contenidos = db.Tests.Where(test => test.BloqueId == bloqueId).Select(cont => cont.Id).ToList();
            contenidos.AddRange(db.Skillwises.Where(test => test.BloqueId == bloqueId).Select(cont => cont.Id).ToList());
            contenidos.AddRange(db.FillTheGaps.Where(test => test.BloqueId == bloqueId).Select(cont => cont.Id).ToList());
            contenidos.AddRange(db.AudioSentenceExercises.Where(test => test.BloqueId == bloqueId).Select(cont => cont.Id).ToList());
            contenidos.AddRange(db.WordByWords.Where(test => test.BloqueId == bloqueId).Select(cont => cont.Id).ToList());
            contenidos.AddRange(db.TrueFalses.Where(test => test.BloqueId == bloqueId).Select(cont => cont.Id).ToList());

            foreach (var contenidoId in contenidos)
            {
                db.Mistakes.Add(new Mistake
                {
                    AlumnoId = userId,
                    BloqueId = bloqueId,
                    PreguntaId = contenidoId
                });
            }
            db.SaveChanges();
        }

        internal static void QuitarMistake(int bloqueId,int preguntaId)
        {
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;
            AuthContext db = new AuthContext();
            var mistake = db.Mistakes.Where(mis=>mis.AlumnoId== userId && mis.BloqueId==bloqueId && mis.PreguntaId==preguntaId).ToList();
            if (mistake.Count>0)
            {
                db.Mistakes.RemoveRange(mistake);
                db.SaveChanges();
            }
        }
    }
}