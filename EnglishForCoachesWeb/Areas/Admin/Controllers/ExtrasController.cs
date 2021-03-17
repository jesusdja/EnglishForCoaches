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

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExtrasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Extras/Copy
        public ActionResult Copy(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Extra = db.Extras.Include(s => s.CategoriaExtra).SingleOrDefault(s => s.ExtraId == id);
            if (Extra == null)
            {
                return HttpNotFound();
            }

            ExtraCopyViewModel viewModel = new ExtraCopyViewModel();
            viewModel.Extra = Extra;
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/Extras/Copy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(ExtraCopyViewModel viewModel)
        {
            int id = viewModel.Extra.ExtraId;
            viewModel.Extra = db.Extras.Include(s => s.CategoriaExtra)
                .SingleOrDefault(s => s.ExtraId == id);
            if (viewModel.Extra != null)
            {
                viewModel.Extra.SubTemaId = viewModel.SubTemaCopiarId;
                viewModel.Extra.Orden = db.Extras.Count(extra => extra.CategoriaExtraId == viewModel.Extra.CategoriaExtraId && extra.SubTemaId == viewModel.Extra.SubTemaId) + 1;

                db.Extras.Add(viewModel.Extra);
                db.SaveChanges();

                //Meter en nuevos clientes 
                var clientes = db.ClienteSubTemas.Where(cb => cb.SubTemaId == viewModel.SubTemaCopiarId);
                foreach (var clienteBloque in clientes)
                {
                    db.ClienteExtras.Add(new ClienteExtra()
                    {
                        ExtraId = viewModel.Extra.ExtraId,
                        ClienteId = clienteBloque.ClienteId
                    });
                }
                db.SaveChanges();



                if (!string.IsNullOrEmpty(viewModel.Extra.Audio))
                        {
                            string oldPathAndName = "~/media/upload/extras_audios/" + viewModel.Extra.Audio;
                            string newPathAndName = "~/media/upload/extras_audios/" + viewModel.Extra.ExtraId + ".mp3";

                            System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                    viewModel.Extra.Audio = viewModel.Extra.ExtraId + ".jpg";
                            db.Entry(viewModel.Extra).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                if (!string.IsNullOrEmpty(viewModel.Extra.Foto))
                {
                    string oldPathAndName = "~/media/upload/extras_imagenes/" + viewModel.Extra.Foto;
                    string newPathAndName = "~/media/upload/extras_imagenes/" + viewModel.Extra.ExtraId + ".mp3";

                    System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                    viewModel.Extra.Foto = viewModel.Extra.ExtraId + ".jpg";
                    db.Entry(viewModel.Extra).State = EntityState.Modified;
                    db.SaveChanges();
                }



                return RedirectToAction("Index", "Bloques", new { id = viewModel.Extra.SubTemaId, pestanya = (int)PestanyasBloques.Extras });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }


        // GET: Admin/Extras/Create
        public ActionResult Create(int id)
        {

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }
            ExtraCreateViewModel viewModel = new ExtraCreateViewModel();
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Extras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExtraCreateViewModel viewModel)
        {
            //fichero pdf
            //"~/media/upload/extras_puntos/"

            if (ModelState.IsValid)
            {
                viewModel.Extra.SubTemaId = viewModel.Subtema.SubTemaId;

                viewModel.Extra.Orden = db.Extras.Count(extra => extra.CategoriaExtraId == viewModel.Extra.CategoriaExtraId && extra.SubTemaId == viewModel.Extra.SubTemaId) + 1;
                db.Extras.Add(viewModel.Extra);
                db.SaveChanges();
                if (viewModel.AudioFile != null)
                {
                    viewModel.Extra.Audio = viewModel.Extra.ExtraId + ".mp3";

                    string nameAndLocation = "~/media/upload/extras_audios/" + viewModel.Extra.Audio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Extra).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.ImageFile != null)
                {
                    viewModel.Extra.Foto = viewModel.Extra.ExtraId + ".jpg";

                    string nameAndLocation = "~/media/upload/extras_imagenes/" + viewModel.Extra.Foto;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Extra).State = EntityState.Modified;
                    db.SaveChanges();
                }

                AccesoClientesHelper.AnyadirExtraConHijos(viewModel.Extra.ExtraId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());
                return RedirectToAction("Index", "Bloques", new { id = viewModel.Extra.SubTemaId, pestanya = (int)PestanyasBloques.Extras });
            }
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == viewModel.Subtema.SubTemaId);
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Extras/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extra Extra = db.Extras.Include(e=>e.SubTema.Tema).FirstOrDefault(a => a.ExtraId == id);
            if (Extra == null)
            {
                return HttpNotFound();
            }


            ExtraEditViewModel viewModel = new ExtraEditViewModel();

            viewModel.InicializarDesplegables();
            viewModel.Extra = Extra;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Extras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExtraEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.AudioFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/extras_audios/" + viewModel.Extra.Audio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Extra.Audio = viewModel.Extra.ExtraId + ".mp3";

                    string nameAndLocation = "~/media/upload/extras_audios/" + viewModel.Extra.Audio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));
                }

                if (viewModel.ImageFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/extras_imagenes/" + viewModel.Extra.Foto);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Extra.Foto = viewModel.Extra.ExtraId + ".jpg";

                    string nameAndLocation = "~/media/upload/extras_imagenes/" + viewModel.Extra.Foto;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));
                }
                db.Entry(viewModel.Extra).State = EntityState.Modified;
                db.SaveChanges();

                AccesoClientesHelper.AnyadirExtraConHijos(viewModel.Extra.ExtraId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                recalcularOrden(viewModel.Extra.CategoriaExtraId, viewModel.Extra.SubTemaId.GetValueOrDefault());
                return RedirectToAction("Index", "Bloques", new { id = viewModel.Extra.SubTemaId, pestanya = (int)PestanyasBloques.Extras });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Extras/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extra Extra = db.Extras.Find(id);
            if (Extra == null)
            {
                return HttpNotFound();
            }
            var subtemaId = Extra.SubTemaId;
            //Ordenar de nuevo todos los subtemas            
            foreach (Extra st in db.Extras.Where(b => b.CategoriaExtraId == Extra.CategoriaExtraId && b.SubTemaId == Extra.SubTemaId && b.Orden >= Extra.Orden).ToList())
            {
                st.Orden = (st.Orden) - 1;

                db.Entry(st).State = EntityState.Modified;
            }
            db.SaveChanges();


            string fullPath = Request.MapPath("~/media/upload/extras_audios/" + Extra.Audio);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            fullPath = Request.MapPath("~/media/upload/extras_imagenes/" + Extra.Foto);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            //Eliminar subtema
            db.Extras.Remove(Extra);
            db.SaveChanges();
            return RedirectToAction("Index", "Bloques", new { id = subtemaId, pestanya = (int)PestanyasBloques.Extras });
        }




        //POST: ModificarOrden
        public ActionResult ModificarOrden(int id, string posicion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extra extra = db.Extras.Find(id);
            if (extra == null)
            {
                return HttpNotFound();
            }

            actualizarOrdenSubtema(id, 1, posicion);


            return RedirectToAction("Index", "Bloques", new { id = extra.SubTemaId, pestanya = (int)PestanyasBloques.Extras });
        }

        private void recalcularOrden(int categoria,int subtemaId)
        {
            var extras = db.Extras.Where(b => b.CategoriaExtraId == categoria && b.SubTemaId == subtemaId).OrderBy(ex=>ex.Orden).ToArray();
            Int32 ordenNuevo=1;
            for (var i = 0; i < extras.Length; i++)
            {
                extras[i].Orden = ordenNuevo;
                db.Entry(extras[i]).State = EntityState.Modified;
                db.SaveChanges();

                ordenNuevo++;
            }
        }

        private void actualizarOrdenSubtema(int id, int posicion, string mov)
        {
            Extra extra = db.Extras.Find(id);
            Int32 ordenNuevo;

            if (mov.Equals("top"))
            {
                //Al subtema seleccionado le restamos 1 posicion
                ordenNuevo = (extra.Orden) - 1;

                //Buscamos el subtema con el orden a modificar para sumarle 1 posicion
                Extra st = db.Extras.Where(b => b.CategoriaExtraId == extra.CategoriaExtraId && b.SubTemaId == extra.SubTemaId && b.Orden == ordenNuevo).FirstOrDefault();
                st.Orden = (st.Orden) + 1;
                db.Entry(st).State = EntityState.Modified;
                db.SaveChanges();

                extra.Orden = ordenNuevo;
                db.Entry(extra).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (mov.Equals("down"))
            {
                //Al subtema seleccionado le sumamos 1 posicion
                ordenNuevo = (extra.Orden) + 1;

                //Buscamos el subtema con el orden a modificar para restarle 1 posicion
                Extra st = db.Extras.Where(b => b.CategoriaExtraId == extra.CategoriaExtraId && b.SubTemaId == extra.SubTemaId && b.Orden == ordenNuevo).FirstOrDefault();
                st.Orden = (st.Orden) - 1;
                db.Entry(st).State = EntityState.Modified;
                db.SaveChanges();

                extra.Orden = ordenNuevo;
                db.Entry(extra).State = EntityState.Modified;
                db.SaveChanges();
            }

        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
