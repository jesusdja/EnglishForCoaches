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
using EnglishForCoachesWeb.Database.JuegoOnline;
using EnglishForCoachesWeb.Database.Clientes;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JuegoOnlinesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();



        // GET: Admin/JuegoOnlines/Create
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


            JuegoOnlinesCreateViewModel viewModel = new JuegoOnlinesCreateViewModel();
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            viewModel.CargarClienteSeleccionado(db);

            return View(viewModel);
        }


        // POST: Admin/JuegoOnlines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JuegoOnlinesCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                viewModel.JuegoOnline.SubTemaId = viewModel.Subtema.SubTemaId;
                db.JuegoOnlines.Add(viewModel.JuegoOnline);
                db.SaveChanges();


                if (viewModel.JuegoOnline.TipoJuegoOnlineId == (int)TiposDeJuegosOnlineId.SopaLetras)
                {
                    CrearSopaLetras(viewModel);
                }

                AccesoClientesHelper.AnyadirJuegoOnlineConHijos(viewModel.JuegoOnline.JuegoOnlineId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                TipoJuegoOnline tipoJuegoOnline = db.TipoJuegoOnlines.Find(viewModel.JuegoOnline.TipoJuegoOnlineId);
                return RedirectToAction("Create", tipoJuegoOnline.Controlador, new { id = viewModel.JuegoOnline.JuegoOnlineId });

            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == viewModel.Subtema.SubTemaId);
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }
      

        private void CrearSopaLetras(JuegoOnlinesCreateViewModel viewModel)
        {
            var sopaLetras = new SopaLetras()
            {
                JuegoOnlineId = viewModel.JuegoOnline.JuegoOnlineId,
                Descripcion = "Sopa de Letras",
                Enunciado = "Sopa de Letras",
                SubTemaId = viewModel.JuegoOnline.SubTemaId,
                TipoJuegoOnlineId = viewModel.JuegoOnline.TipoJuegoOnlineId
            };
            List<CasillaSopaLetras> icollection = new List<CasillaSopaLetras>();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    icollection.Add(new CasillaSopaLetras()
                    {
                        letra = "",
                        PosH = j,
                        PosV = i
                    });
                }
            }
            sopaLetras.CasillaSopaLetras = icollection;
            db.SopaLetras.Add(sopaLetras);
            db.SaveChanges();
        }


        // GET: Admin/JuegoOnlines/Edit/1
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var JuegoOnline = db.JuegoOnlines.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.JuegoOnlineId == id);
            if (JuegoOnline == null)
            {
                return HttpNotFound();
            }


            JuegoOnlinesEditViewModel viewModel = new JuegoOnlinesEditViewModel();
            viewModel.JuegoOnline = JuegoOnline;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }


        // POST: Admin/JuegoOnlines/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JuegoOnlinesEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.JuegoOnline).State = EntityState.Modified;
                db.SaveChanges();
                AccesoClientesHelper.AnyadirJuegoOnlineConHijos(viewModel.JuegoOnline.JuegoOnlineId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                TipoJuegoOnline tipoJuegoOnline = db.TipoJuegoOnlines.Find(viewModel.JuegoOnline.TipoJuegoOnlineId);
                return RedirectToAction("Index", "Bloques", new { id = viewModel.JuegoOnline.SubTemaId, pestanya = (int)PestanyasBloques.JuegosOnline });
            }

            return View(viewModel);
        }

        // GET: Admin/JuegoOnlines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JuegoOnline JuegoOnline = db.JuegoOnlines.Include(s => s.BeatTheGoalies).Include(s => s.Ahorcado)
                .Include(s => s.SopaLetras).Include("SopaLetras.VocabularioSopaLetras").Include("SopaLetras.ComentarioSopaLetras")
                .Include(s => s.MemoryGames).Include("SopaLetras.CasillaSopaLetras")
                        .FirstOrDefault(b => b.JuegoOnlineId == id);
            if (JuegoOnline == null)
            {
                return HttpNotFound();
            }

            //BeatTheGoalie
            if (JuegoOnline.BeatTheGoalies.Count > 0)
            {
                foreach (BeatTheGoalie BeatTheGoalie in JuegoOnline.BeatTheGoalies.ToList())
                {
                    var fullPath = Request.MapPath("~/media/upload/BeatTheGoalie/" + BeatTheGoalie.FicheroAudio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                db.BeatTheGoalies.RemoveRange(JuegoOnline.BeatTheGoalies);
            }


            //MemoryGames
            if (JuegoOnline.MemoryGames.Count > 0)
            {
                foreach (MemoryGame MemoryGame in JuegoOnline.MemoryGames.ToList())
                {
                    if (MemoryGame.UrlImagen != null)
                    {
                        var fullPath = Request.MapPath("~/media/upload/MemoryGame/" + MemoryGame.UrlImagen);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }
                db.MemoryGames.RemoveRange(JuegoOnline.MemoryGames);
            }



            //SopaLetras
            if (JuegoOnline.SopaLetras.Count > 0)
            {
                foreach (SopaLetras SopaLetras in JuegoOnline.SopaLetras.ToList())
                {
                    db.VocabularioSopaLetras.RemoveRange(SopaLetras.VocabularioSopaLetras);
                    db.CasillaSopaLetras.RemoveRange(SopaLetras.CasillaSopaLetras);
                    db.ComentarioSopaLetras.RemoveRange(SopaLetras.ComentarioSopaLetras);
                }
                db.SopaLetras.RemoveRange(JuegoOnline.SopaLetras);
            }


            // ahorcado
            if (JuegoOnline.Ahorcado.Count > 0)
            {
                foreach (Ahorcado Ahorcado in JuegoOnline.Ahorcado.ToList())
                {
                    if (Ahorcado.UrlImagen != null)
                    {
                        var fullPath = Request.MapPath("~/media/upload/Ahorcado/" + Ahorcado.UrlImagen);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    if (Ahorcado.Audio != null)
                    {
                        var fullPath = Request.MapPath("~/media/upload/Ahorcado/Audios/" + Ahorcado.Audio);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }
                db.Ahorcado.RemoveRange(JuegoOnline.Ahorcado);
            }



            db.JuegoOnlines.Remove(JuegoOnline);
            db.SaveChanges();
            return RedirectToAction("Index", "Bloques", new { id = JuegoOnline.SubTemaId, pestanya = (int)PestanyasBloques.JuegosOnline });
        }


        // GET: Admin/JuegoOnlines/Copy
        public ActionResult Copy(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var JuegoOnline = db.JuegoOnlines.Include(s => s.TipoJuegoOnline).SingleOrDefault(s => s.JuegoOnlineId == id);
            if (JuegoOnline == null)
            {
                return HttpNotFound();
            }

            JuegoOnlinesCopyViewModel viewModel = new JuegoOnlinesCopyViewModel();
            viewModel.JuegoOnline = JuegoOnline;
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/JuegoOnlines/Copy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(JuegoOnlinesCopyViewModel viewModel)
        {
            int id = viewModel.JuegoOnline.JuegoOnlineId;
            viewModel.JuegoOnline = db.JuegoOnlines.Include(s => s.TipoJuegoOnline)
                .SingleOrDefault(s => s.JuegoOnlineId == id);
            if (viewModel.JuegoOnline != null)
            {
                viewModel.JuegoOnline.SubTemaId = viewModel.SubTemaCopiarId;

                db.JuegoOnlines.Add(viewModel.JuegoOnline);
                db.SaveChanges();


                //Meter en nuevos clientes 
                var clientes = db.ClienteSubTemas.Where(cb => cb.SubTemaId == viewModel.SubTemaCopiarId);
                foreach (var clienteBloque in clientes)
                {
                    db.ClienteJuegoOnlines.Add(new ClienteJuegoOnline()
                    {
                        JuegoOnlineId = viewModel.JuegoOnline.JuegoOnlineId,
                        ClienteId = clienteBloque.ClienteId
                    });
                }
                db.SaveChanges();


                var JuegoOnline = db.JuegoOnlines.Include(s => s.MemoryGames)
                .Include(s => s.BeatTheGoalies).Include(s => s.Ahorcado).Include("SopaLetras.CasillaSopaLetras")
                .Include("SopaLetras.VocabularioSopaLetras")
                        .SingleOrDefault(s => s.JuegoOnlineId == id);



                if (JuegoOnline.BeatTheGoalies.Count > 0)
                {
                    foreach (BeatTheGoalie BeatTheGoalie in JuegoOnline.BeatTheGoalies.ToList())
                    {
                        BeatTheGoalie.JuegoOnlineId = viewModel.JuegoOnline.JuegoOnlineId;
                        BeatTheGoalie.SubTemaId = viewModel.SubTemaCopiarId;
                        db.BeatTheGoalies.Add(BeatTheGoalie);
                        db.SaveChanges();


                        if (!string.IsNullOrEmpty(BeatTheGoalie.FicheroAudio))
                        {
                            string oldPathAndName = "~/media/upload/BeatTheGoalie/" + BeatTheGoalie.FicheroAudio;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/BeatTheGoalie/" + BeatTheGoalie.Id + ".mp3";

                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                BeatTheGoalie.FicheroAudio = BeatTheGoalie.Id + ".mp3";
                            }
                            else
                            {
                                BeatTheGoalie.FicheroAudio = null;
                            }
                            db.Entry(BeatTheGoalie).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }

                if (JuegoOnline.MemoryGames.Count > 0)
                {
                    foreach (MemoryGame MemoryGame in JuegoOnline.MemoryGames.ToList())
                    {
                        MemoryGame.JuegoOnlineId = viewModel.JuegoOnline.JuegoOnlineId;
                        MemoryGame.SubTemaId = viewModel.SubTemaCopiarId;
                        db.MemoryGames.Add(MemoryGame);
                        db.SaveChanges();


                        if (!string.IsNullOrEmpty(MemoryGame.UrlImagen))
                        {
                            string oldPathAndName = "~/media/upload/MemoryGame/" + MemoryGame.UrlImagen;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/MemoryGame/" + MemoryGame.Id + ".jpg";
                                if (System.IO.File.Exists(Server.MapPath(newPathAndName)))
                                {
                                    System.IO.File.Delete(Server.MapPath(newPathAndName));
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                MemoryGame.UrlImagen = MemoryGame.Id + ".jpg";
                            }
                            else
                            {
                                MemoryGame.UrlImagen = null;
                            }
                            db.Entry(MemoryGame).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }


                if (JuegoOnline.SopaLetras.Count > 0)
                {
                    foreach (SopaLetras SopaLetras in JuegoOnline.SopaLetras.ToList())
                    {

                        var SopaLetrasNuevo = db.SopaLetras
                            .Include(cr => cr.CasillaSopaLetras).Include(cr => cr.VocabularioSopaLetras)
                            .AsNoTracking().FirstOrDefault(cr => cr.Id == SopaLetras.Id);

                        SopaLetrasNuevo.JuegoOnlineId = viewModel.JuegoOnline.JuegoOnlineId;
                        SopaLetrasNuevo.SubTemaId = viewModel.SubTemaCopiarId;
                        db.SopaLetras.Add(SopaLetrasNuevo);
                        db.SaveChanges();
                    }
                }

                if (JuegoOnline.Ahorcado.Count > 0)
                {
                    foreach (Ahorcado Ahorcado in JuegoOnline.Ahorcado.ToList())
                    {
                        Ahorcado.JuegoOnlineId = viewModel.JuegoOnline.JuegoOnlineId;
                        Ahorcado.SubTemaId = viewModel.SubTemaCopiarId;
                        db.Ahorcado.Add(Ahorcado);
                        db.SaveChanges();


                        if (!string.IsNullOrEmpty(Ahorcado.UrlImagen))
                        {
                            string oldPathAndName = "~/media/upload/Ahorcado/" + Ahorcado.UrlImagen;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/Ahorcado/" + Ahorcado.Id + ".jpg";
                                if (System.IO.File.Exists(newPathAndName))
                                {
                                    System.IO.File.Delete(newPathAndName);
                                }
                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                Ahorcado.UrlImagen = Ahorcado.Id + ".jpg";
                            }
                            else
                            {
                                Ahorcado.UrlImagen = null;
                            }
                            db.Entry(Ahorcado).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                        if (!string.IsNullOrEmpty(Ahorcado.Audio))
                        {
                            string oldPathAndName = "~/media/upload/Ahorcado/Audios/" + Ahorcado.Audio;
                            if (System.IO.File.Exists(Server.MapPath(oldPathAndName)))
                            {
                                string newPathAndName = "~/media/upload/Ahorcado/Audios/" + Ahorcado.Id + ".mp3";
                                if (System.IO.File.Exists(newPathAndName))
                                {
                                    System.IO.File.Delete(newPathAndName);
                                }

                                System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                                Ahorcado.Audio = Ahorcado.Id + ".mp3";
                            }
                            else
                            {
                                Ahorcado.Audio = null;
                            }
                            db.Entry(Ahorcado).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                }

                return RedirectToAction("Create", JuegoOnline.TipoJuegoOnline.Controlador, new { id = viewModel.JuegoOnline.JuegoOnlineId });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }
    }

}
