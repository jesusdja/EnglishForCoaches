using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Database.Clientes;
using System.Data.SqlClient;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{

    public enum PestanyasBloques
    {
        Gramaticas = 1,
        Contenidos = 2,
        JuegosOffline = 3,
        JuegosOnline = 4,
        Videos = 5,
        Extras = 6
    }

    [Authorize(Roles = "Admin")]
    public class BloquesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Bloques
        public ActionResult Index(int id, int? pestanya)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }


            var bloques = db.Bloques.Include(t => t.Area).Include(t => t.TipoEjercicio).Where(t => t.SubTemaId == id).ToList();
            List<Extra> Extras;
            var Gramaticas = db.Gramaticas.Where(gram => gram.SubTemaId == id).OrderBy(d => d.Orden).ToList();
            var Juegos = db.Juegos.Include(jue => jue.CategoriaJuego).Where(jue => jue.SubTemaId == id).ToList();
            var JuegoOnlines = db.JuegoOnlines.Include(t => t.TipoJuegoOnline).Where(jue => jue.SubTemaId == id).ToList();
            var SubTemaVideos = db.SubTemaVideos.Where(jue => jue.SubTemaId == id).ToList();
            try
            {
                Extras = db.Extras.Include(ex => ex.CategoriaExtra).Where(jue => jue.SubTemaId == id).ToList();
            }
            catch (SqlException te)
            {
                Extras = new List<Extra>();
            }

            BloquesIndexViewModel viewModel = new BloquesIndexViewModel();

            if (pestanya.HasValue)
            {
                viewModel.pestanyaSeleccionada = pestanya.Value;
            }
            else
            {
                viewModel.pestanyaSeleccionada = (int)PestanyasBloques.Gramaticas;
            }

            viewModel.listadoBloques = bloques;
            viewModel.listadoGramaticas = Gramaticas;
            viewModel.listadoJuegos = Juegos;

            viewModel.listadoSubTemaVideos = SubTemaVideos;
            viewModel.listadoExtras = Extras;

            viewModel.listadoJuegoOnlines = JuegoOnlines;

            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }



        // POST: Admin/Bloques/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int id, BloquesIndexViewModel viewModel)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }

            var busqueda = db.Bloques.Include(t => t.Area).Include(t => t.TipoEjercicio).Where(t => t.SubTemaId == id).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Descripcion.Contains(viewModel.TextoBusqueda) ||
                x.TipoEjercicio.Descripcion.Contains(viewModel.TextoBusqueda) ||
                x.Area.Descripcion.Contains(viewModel.TextoBusqueda)).ToList();
            }
            if (viewModel.AreaBusquedaId.HasValue)
            {
                busqueda = busqueda.Where(x => x.AreaId == viewModel.AreaBusquedaId.Value).ToList();
            }
            if (viewModel.TipoEjercicioBusquedaId.HasValue)
            {
                busqueda = busqueda.Where(x => x.TipoEjercicioId == viewModel.TipoEjercicioBusquedaId.Value).ToList();
            }

            var Gramaticas = db.Gramaticas.Where(gram => gram.SubTemaId == id).OrderBy(d => d.Orden).ToList();
            var Juegos = db.Juegos.Include(jue => jue.CategoriaJuego).Where(jue => jue.SubTemaId == id).ToList();
            var SubTemaVideos = db.SubTemaVideos.Where(jue => jue.SubTemaId == id).ToList();
            var JuegoOnlines = db.JuegoOnlines.Include(t => t.TipoJuegoOnline).Where(jue => jue.SubTemaId == id).ToList();
            var Extras = db.Extras.Include(ex => ex.CategoriaExtra).Where(jue => jue.SubTemaId == id).ToList();

            viewModel.listadoJuegos = Juegos;
            viewModel.listadoGramaticas = Gramaticas;
            viewModel.listadoBloques = busqueda;
            viewModel.Subtema = subtema;
            viewModel.listadoSubTemaVideos = SubTemaVideos;

            viewModel.listadoExtras = Extras;
            viewModel.listadoJuegoOnlines = JuegoOnlines;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }


        // GET: Admin/Bloques/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }


            BloquesCreateViewModel viewModel = new BloquesCreateViewModel();
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            viewModel.CargarClienteSeleccionado(db);

            return View(viewModel);
        }


        // POST: Admin/Bloques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BloquesCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isValid = true;
                if (viewModel.Bloque.AreaId == (int)AreasId.Examen)
                {
                    if (viewModel.Bloque.TipoEjercicioId != (int)TiposDeEjerciciosId.Test && viewModel.Bloque.TipoEjercicioId != (int)TiposDeEjerciciosId.FillTheGap)
                    {
                        ModelState.AddModelError("Bloque.TipoEjercicioId", "El examen sólo puede ser Test o Fill the gap");
                        isValid = false;
                    }

                    var examen = db.Bloques.FirstOrDefault(bl => bl.AreaId == (int)AreasId.Examen && bl.SubTemaId == viewModel.Subtema.SubTemaId);
                    if (examen != null)
                    {
                        ModelState.AddModelError("Bloque.AreaId", "Ya existe un examen");
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    viewModel.Bloque.SubTemaId = viewModel.Subtema.SubTemaId;
                    db.Bloques.Add(viewModel.Bloque);
                    db.SaveChanges();

                    AccesoClientesHelper.AnyadirBloqueConHijos(viewModel.Bloque.BloqueId, viewModel.Clientes.Where(cli => cli.Selected)
                        .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                    if (viewModel.Bloque.TipoEjercicioId == (int)TiposDeEjerciciosId.Crucigrama)
                    {
                        var crucigrama = new Crucigrama()
                        {
                            AreaId = viewModel.Bloque.AreaId,
                            BloqueId = viewModel.Bloque.BloqueId,
                            Descripcion = "Crucigrama",
                            Enunciado = "Crucigrama",
                            Horizontales = "Pistas de palabras horizontales",
                            SubTemaId = viewModel.Bloque.SubTemaId,
                            TipoEjercicioId = viewModel.Bloque.TipoEjercicioId,
                            Verticales = "Pistas de palabras verticales"
                        };
                        List<CasillaCrucigrama> icollection = new List<CasillaCrucigrama>();
                        for (int i = 0; i < 12; i++)
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                icollection.Add(new CasillaCrucigrama()
                                {
                                    letra = "",
                                    PosH = j,
                                    PosV = i
                                });
                            }
                        }
                        crucigrama.CasillaCrucigramas = icollection;
                        db.Crucigramas.Add(crucigrama);
                        db.SaveChanges();
                    }

                    TipoEjercicio tipoEjercicio = db.TipoEjercicios.Find(viewModel.Bloque.TipoEjercicioId);
                    return RedirectToAction("Create", tipoEjercicio.Controlador, new { id = viewModel.Bloque.BloqueId });
                }
            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == viewModel.Subtema.SubTemaId);
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }


        // GET: Admin/Bloques/Edit/1
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloque = db.Bloques.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }


            BloquesEditViewModel viewModel = new BloquesEditViewModel();
            viewModel.Bloque = bloque;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }


        // POST: Admin/Bloques/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BloquesEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.Bloque).State = EntityState.Modified;
                db.SaveChanges();

                AccesoClientesHelper.AnyadirBloqueConHijos(viewModel.Bloque.BloqueId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                TipoEjercicio tipoEjercicio = db.TipoEjercicios.Find(viewModel.Bloque.TipoEjercicioId);
                return RedirectToAction("Index", "Bloques", new { id = viewModel.Bloque.SubTemaId });
            }

            return View(viewModel);
        }

        // GET: Admin/Bloques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloque bloque = db.Bloques
                        .Include(s => s.Tests).Include(s => s.AudioSentenceExercises).Include(s => s.Skillwises).Include(s => s.MatchTheWords)
                        .Include(s => s.FillTheGaps).Include(s => s.WordByWords).Include(s => s.Crucigramas)
                        .Include(s => s.MatchThePictures).Include(s => s.TrueFalses).Include(s => s.FillTheBoxs)
                        .FirstOrDefault(b => b.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }

            if (bloque.TrueFalses.Count > 0)
            {
                foreach (TrueFalse TrueFalse in bloque.TrueFalses.ToList())
                {
                    var fullPath = Request.MapPath("~/media/upload/TrueFalse/" + TrueFalse.UrlImagen);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }
            if (bloque.Tests.Count > 0)
            {
                foreach (Test Test in bloque.Tests.ToList())
                {
                    var fullPath = Request.MapPath("~/media/upload/Test/" + Test.UrlImagen);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            if (bloque.MatchThePictures.Count > 0)
            {
                foreach (MatchThePicture MatchThePicture in bloque.MatchThePictures.ToList())
                {
                    var fullPath = Request.MapPath("~/media/upload/MatchThePicture/" + MatchThePicture.UrlImagen);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }
            if (bloque.FillTheBoxs.Count > 0)
            {
                foreach (FillTheBox FillTheBox in bloque.FillTheBoxs.ToList())
                {
                    var fullPath = Request.MapPath("~/media/upload/imagen_fillthebox/" + FillTheBox.UrlImagen);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    fullPath = Request.MapPath("~/media/upload/audio_fillthebox/" + FillTheBox.FicheroAudio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }


            if (bloque.Crucigramas.Count > 0)
            {
                foreach (Crucigrama Crucigrama in bloque.Crucigramas.ToList())
                {
                    var cruci = db.Crucigramas.Include(cruc => cruc.CasillaCrucigramas).FirstOrDefault(cr => cr.Id == Crucigrama.Id);
                    db.CasillaCrucigramas.RemoveRange(cruci.CasillaCrucigramas);

                }
                db.Crucigramas.RemoveRange(bloque.Crucigramas);
            }
            if (bloque.AudioSentenceExercises.Count > 0)
            {
                foreach (AudioSentenceExercise AudioSentenceExercise in bloque.AudioSentenceExercises.ToList())
                {
                    var fullPath = Request.MapPath("~/media/upload/audio_ejercicio/" + AudioSentenceExercise.FicheroAudio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }


            db.Bloques.Remove(bloque);
            db.SaveChanges();
            return RedirectToAction("Index", "Bloques", new { id = bloque.SubTemaId, pestanya = (int)PestanyasBloques.Contenidos });
        }


        // GET: Admin/Bloques/Copy
        public ActionResult Copy(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloque = db.Bloques.Include(s => s.TipoEjercicio).SingleOrDefault(s => s.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }

            BloquesCopyViewModel viewModel = new BloquesCopyViewModel();
            viewModel.Bloque = bloque;
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/Bloques/Copy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(BloquesCopyViewModel viewModel)
        {
            int id = viewModel.Bloque.BloqueId;
            viewModel.Bloque = db.Bloques.Include(s => s.TipoEjercicio)
                .SingleOrDefault(s => s.BloqueId == id);
            if (viewModel.Bloque != null)
            {
                viewModel.Bloque.SubTemaId = viewModel.SubTemaCopiarId;
                viewModel.Bloque.AreaId = viewModel.AreaCopiarId;

                db.Bloques.Add(viewModel.Bloque);
                db.SaveChanges();

                //Meter en nuevos clientes 
                var clientes = db.ClienteSubTemas.Where(cb => cb.SubTemaId == viewModel.SubTemaCopiarId);
                foreach (var clienteBloque in clientes)
                {
                    db.ClienteBloques.Add(new ClienteBloque()
                    {
                        BloqueId = viewModel.Bloque.BloqueId,
                        ClienteId = clienteBloque.ClienteId
                    });
                }
                db.SaveChanges();

                var bloque = db.Bloques.Include(s => s.TipoEjercicio)
                        .Include(s => s.Tests).Include(s => s.AudioSentenceExercises).Include(s => s.Skillwises).Include(s => s.MatchTheWords)
                        .Include(s => s.FillTheGaps).Include(s => s.WordByWords).Include(s => s.Crucigramas).Include(s => s.MatchThePictures)
                        .Include(s => s.TrueFalses).Include(s => s.FillTheBoxs)
                        .SingleOrDefault(s => s.BloqueId == id);


                if (bloque.Tests.Count > 0)
                {
                    foreach (Test test in bloque.Tests.ToList())
                    {
                        test.BloqueId = viewModel.Bloque.BloqueId;
                        test.SubTemaId = viewModel.SubTemaCopiarId;
                        test.AreaId = viewModel.AreaCopiarId;
                        db.Tests.Add(test);
                        db.SaveChanges();

                        if (!string.IsNullOrEmpty(test.UrlImagen))
                        {
                            string oldPathAndName = "~/media/upload/Test/" + test.UrlImagen;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/Test/" + test.Id + ".jpg";

                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                test.UrlImagen = test.Id + ".jpg";
                            }
                            else
                            {
                                test.UrlImagen = null;
                            }
                            db.Entry(test).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }
                if (bloque.Skillwises.Count > 0)
                {
                    foreach (Skillwise skillwise in bloque.Skillwises.ToList())
                    {
                        skillwise.BloqueId = viewModel.Bloque.BloqueId;
                        skillwise.SubTemaId = viewModel.SubTemaCopiarId;
                        skillwise.AreaId = viewModel.AreaCopiarId;
                        db.Skillwises.Add(skillwise);
                        db.SaveChanges();
                    }
                }
                if (bloque.MatchTheWords.Count > 0)
                {
                    foreach (MatchTheWord MatchTheWord in bloque.MatchTheWords.ToList())
                    {
                        MatchTheWord.BloqueId = viewModel.Bloque.BloqueId;
                        MatchTheWord.SubTemaId = viewModel.SubTemaCopiarId;
                        MatchTheWord.AreaId = viewModel.AreaCopiarId;
                        db.MatchTheWords.Add(MatchTheWord);
                        db.SaveChanges();
                    }
                }
                if (bloque.TrueFalses.Count > 0)
                {
                    foreach (TrueFalse TrueFalse in bloque.TrueFalses.ToList())
                    {
                        TrueFalse.BloqueId = viewModel.Bloque.BloqueId;
                        TrueFalse.SubTemaId = viewModel.SubTemaCopiarId;
                        TrueFalse.AreaId = viewModel.AreaCopiarId;
                        db.TrueFalses.Add(TrueFalse);
                        db.SaveChanges();


                        if (!string.IsNullOrEmpty(TrueFalse.UrlImagen))
                        {
                            string oldPathAndName = "~/media/upload/TrueFalse/" + TrueFalse.UrlImagen;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/TrueFalse/" + TrueFalse.Id + ".jpg";

                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                TrueFalse.UrlImagen = TrueFalse.Id + ".jpg";
                            }
                            else
                            {
                                TrueFalse.UrlImagen = null;
                            }
                            db.Entry(TrueFalse).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }

                if (bloque.MatchThePictures.Count > 0)
                {
                    foreach (MatchThePicture MatchThePicture in bloque.MatchThePictures.ToList())
                    {
                        MatchThePicture.BloqueId = viewModel.Bloque.BloqueId;
                        MatchThePicture.SubTemaId = viewModel.SubTemaCopiarId;
                        MatchThePicture.AreaId = viewModel.AreaCopiarId;
                        db.MatchThePictures.Add(MatchThePicture);
                        db.SaveChanges();


                        if (!string.IsNullOrEmpty(MatchThePicture.UrlImagen))
                        {
                            string oldPathAndName = "~/media/upload/MatchThePicture/" + MatchThePicture.UrlImagen;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/MatchThePicture/" + MatchThePicture.Id + ".jpg";

                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                MatchThePicture.UrlImagen = MatchThePicture.Id + ".jpg";
                            }
                            else
                            {
                                MatchThePicture.UrlImagen = null;
                            }
                            db.Entry(MatchThePicture).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }

                if (bloque.FillTheBoxs.Count > 0)
                {
                    foreach (FillTheBox FillTheBox in bloque.FillTheBoxs.ToList())
                    {
                        FillTheBox.BloqueId = viewModel.Bloque.BloqueId;
                        FillTheBox.SubTemaId = viewModel.SubTemaCopiarId;
                        FillTheBox.AreaId = viewModel.AreaCopiarId;
                        db.FillTheBoxs.Add(FillTheBox);
                        db.SaveChanges();


                        if (!string.IsNullOrEmpty(FillTheBox.UrlImagen))
                        {
                            string oldPathAndName = "~/media/upload/imagen_fillthebox/" + FillTheBox.UrlImagen;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/imagen_fillthebox/" + FillTheBox.Id + ".jpg";

                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                FillTheBox.UrlImagen = FillTheBox.Id + ".jpg";
                            }
                            else
                            {
                                FillTheBox.UrlImagen = null;
                            }
                            db.Entry(FillTheBox).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                        if (!string.IsNullOrEmpty(FillTheBox.FicheroAudio))
                        {
                            string oldPathAndName = "~/media/upload/audio_fillthebox/" + FillTheBox.FicheroAudio;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/audio_fillthebox/" + FillTheBox.Id + ".mp3";

                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                FillTheBox.FicheroAudio = FillTheBox.Id + ".mp3";
                            }
                            else
                            {
                                FillTheBox.FicheroAudio = null;
                            }
                            db.Entry(FillTheBox).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }
                if (bloque.FillTheGaps.Count > 0)
                {
                    foreach (FillTheGap FillTheGap in bloque.FillTheGaps.ToList())
                    {
                        FillTheGap.BloqueId = viewModel.Bloque.BloqueId;
                        FillTheGap.SubTemaId = viewModel.SubTemaCopiarId;
                        FillTheGap.AreaId = viewModel.AreaCopiarId;
                        db.FillTheGaps.Add(FillTheGap);
                        db.SaveChanges();
                    }
                }
                if (bloque.WordByWords.Count > 0)
                {
                    foreach (WordByWord WordByWord in bloque.WordByWords.ToList())
                    {
                        WordByWord.BloqueId = viewModel.Bloque.BloqueId;
                        WordByWord.SubTemaId = viewModel.SubTemaCopiarId;
                        WordByWord.AreaId = viewModel.AreaCopiarId;
                        db.WordByWords.Add(WordByWord);
                        db.SaveChanges();
                    }
                }
                if (bloque.Crucigramas.Count > 0)
                {
                    foreach (Crucigrama Crucigrama in bloque.Crucigramas.ToList())
                    {
                        var crucigramaNuevo = db.Crucigramas.Include(cr => cr.CasillaCrucigramas).AsNoTracking().FirstOrDefault(cr => cr.Id == Crucigrama.Id);

                        crucigramaNuevo.BloqueId = viewModel.Bloque.BloqueId;

                        crucigramaNuevo.SubTemaId = viewModel.SubTemaCopiarId;
                        crucigramaNuevo.AreaId = viewModel.AreaCopiarId;
                        db.Crucigramas.Add(crucigramaNuevo);
                        db.SaveChanges();
                    }
                }


                if (bloque.AudioSentenceExercises.Count > 0)
                {
                    foreach (AudioSentenceExercise AudioSentenceExercise in bloque.AudioSentenceExercises.ToList())
                    {
                        AudioSentenceExercise.BloqueId = viewModel.Bloque.BloqueId;
                        AudioSentenceExercise.SubTemaId = viewModel.SubTemaCopiarId;
                        AudioSentenceExercise.AreaId = viewModel.AreaCopiarId;
                        db.AudioSentenceExercises.Add(AudioSentenceExercise);
                        db.SaveChanges();

                        if (!string.IsNullOrEmpty(AudioSentenceExercise.FicheroAudio))
                        {
                            string oldPathAndName = "~/media/upload/audio_ejercicio/" + AudioSentenceExercise.FicheroAudio;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/audio_ejercicio/" + AudioSentenceExercise.Id + ".mp3";
                                if (System.IO.File.Exists(newPathAndName))
                                {
                                    System.IO.File.Delete(newPathAndName);
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                AudioSentenceExercise.FicheroAudio = AudioSentenceExercise.Id + ".mp3";
                            }
                            else
                            {
                                AudioSentenceExercise.FicheroAudio = null;
                            }
                            db.Entry(AudioSentenceExercise).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }


                return RedirectToAction("Create", bloque.TipoEjercicio.Controlador, new { id = viewModel.Bloque.BloqueId });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }
    }
}
