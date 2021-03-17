using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Helpers
{
    public static class AccesoClientesHelper
    {
        /*
        1.	Vocabulario
        2.	Frase
        3.	Tema
            a.	Subtema
                i.	Gramática
                    1.	Contenido
                    2.	Juego offline
                    3.	Juego online
                    4.	Video
                    5.	Extra

         * */
         //Tema
        public static void AnyadirTemaConHijos(int temaId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteTemasActuales = db.ClienteTemas.Where(acc => acc.TemaId == temaId).ToList();
            var accesosQuitar = clienteTemasActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteTemas.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteTemasActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirTema(temaId, clienteSeleccionado);
            }
            var subTemas = db.SubTemas.Where(sub => sub.TemaId == temaId);
            foreach (var subtema in subTemas)
            {
                AnyadirSubTemaConHijos(subtema.SubTemaId, clientesSeleccionados);
            }
        }

        private static void AnyadirTema(int temaId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteTema clienteTema = new ClienteTema();
            clienteTema.ClienteId = clienteId;
            clienteTema.TemaId = temaId;

            db.ClienteTemas.Add(clienteTema);
            db.SaveChanges();
        }

        //Subtema
        public static void AnyadirSubTemaConHijos(int subTemaId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteSubTemasActuales = db.ClienteSubTemas.Where(acc => acc.SubTemaId == subTemaId).ToList();
            var accesosQuitar = clienteSubTemasActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteSubTemas.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteSubTemasActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirSubTema(subTemaId, clienteSeleccionado);
            }
            var gramaticas = db.Gramaticas.Where(sub => sub.SubTemaId == subTemaId);
            foreach (var gramatica in gramaticas)
            {
                AnyadirGramaticaConHijos(gramatica.GramaticaId, clientesSeleccionados);
            }
            var bloques = db.Bloques.Where(sub => sub.SubTemaId == subTemaId);
            foreach (var bloque in bloques)
            {
                AnyadirBloqueConHijos(bloque.BloqueId, clientesSeleccionados);
            }
            var extras = db.Extras.Where(sub => sub.SubTemaId == subTemaId);
            foreach (var extra in extras)
            {
                AnyadirExtraConHijos(extra.ExtraId, clientesSeleccionados);
            }
            var juegos = db.Juegos.Where(sub => sub.SubTemaId == subTemaId);
            foreach (var juego in juegos)
            {
                AnyadirJuegoConHijos(juego.JuegoId, clientesSeleccionados);
            }
            var juegoOnlines = db.JuegoOnlines.Where(sub => sub.SubTemaId == subTemaId);
            foreach (var juegoOnline in juegoOnlines)
            {
                AnyadirJuegoOnlineConHijos(juegoOnline.JuegoOnlineId, clientesSeleccionados);
            }
            var subTemaVideos = db.SubTemaVideos.Where(sub => sub.SubTemaId == subTemaId);
            foreach (var subTemaVideo in subTemaVideos)
            {
                AnyadirSubTemaVideoConHijos(subTemaVideo.SubTemaVideoId, clientesSeleccionados);
            }
        }
        
        private static void AnyadirSubTema(int subTemaId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteSubTema clienteSubTema = new ClienteSubTema();
            clienteSubTema.ClienteId = clienteId;
            clienteSubTema.SubTemaId = subTemaId;

            db.ClienteSubTemas.Add(clienteSubTema);
            db.SaveChanges();

            var subtema = db.SubTemas.Find(subTemaId);
            if (db.ClienteTemas.Count(ct => ct.TemaId == subtema.TemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirTema(subtema.TemaId, clienteId);
            }
        }

        //Gramatica
        public static void AnyadirGramaticaConHijos(int gramaticaId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteGramaticasActuales = db.ClienteGramaticas.Where(acc => acc.GramaticaId == gramaticaId).ToList();
            var accesosQuitar = clienteGramaticasActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteGramaticas.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteGramaticasActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirGramatica(gramaticaId, clienteSeleccionado);
            }

        }

        private static void AnyadirGramatica(int gramaticaId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteGramatica clienteGramatica = new ClienteGramatica();
            clienteGramatica.ClienteId = clienteId;
            clienteGramatica.GramaticaId = gramaticaId;

            db.ClienteGramaticas.Add(clienteGramatica);
            db.SaveChanges();

            var gramatica = db.Gramaticas.Find(gramaticaId);
            if (db.ClienteSubTemas.Count(ct => ct.SubTemaId == gramatica.SubTemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirSubTema(gramatica.SubTemaId, clienteId);
            }
        }

        //bloques
        public static void AnyadirBloqueConHijos(int bloqueId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteBloquesActuales = db.ClienteBloques.Where(acc => acc.BloqueId == bloqueId).ToList();
            var accesosQuitar = clienteBloquesActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteBloques.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteBloquesActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirBloque(bloqueId, clienteSeleccionado);
            }
        }

            private static void AnyadirBloque(int bloqueId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteBloque clienteBloque = new ClienteBloque();
            clienteBloque.ClienteId = clienteId;
            clienteBloque.BloqueId = bloqueId;

            db.ClienteBloques.Add(clienteBloque);
            db.SaveChanges();

            var bloque = db.Bloques.Find(bloqueId);
            if (db.ClienteSubTemas.Count(ct => ct.SubTemaId == bloque.SubTemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirSubTema(bloque.SubTemaId, clienteId);
            }
        }

        //extras
        public static void AnyadirExtraConHijos(int extraId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteExtrasActuales = db.ClienteExtras.Where(acc => acc.ExtraId == extraId).ToList();
            var accesosQuitar = clienteExtrasActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteExtras.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteExtrasActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirExtra(extraId, clienteSeleccionado);
            }
        }

        private static void AnyadirExtra(int extraId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteExtra clienteExtra = new ClienteExtra();
            clienteExtra.ClienteId = clienteId;
            clienteExtra.ExtraId = extraId;

            db.ClienteExtras.Add(clienteExtra);
            db.SaveChanges();

            var extra = db.Extras.Find(extraId);
            if (db.ClienteSubTemas.Count(ct => ct.SubTemaId == extra.SubTemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirSubTema(extra.SubTemaId.Value, clienteId);
            }
        }

        //juegos
        public static void AnyadirJuegoConHijos(int juegoId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteJuegosActuales = db.ClienteJuegos.Where(acc => acc.JuegoId == juegoId).ToList();
            var accesosQuitar = clienteJuegosActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteJuegos.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteJuegosActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirJuego(juegoId, clienteSeleccionado);
            }
        }

        private static void AnyadirJuego(int juegoId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteJuego clienteJuego = new ClienteJuego();
            clienteJuego.ClienteId = clienteId;
            clienteJuego.JuegoId = juegoId;

            db.ClienteJuegos.Add(clienteJuego);
            db.SaveChanges();

            var juego = db.Juegos.Find(juegoId);
            if (db.ClienteSubTemas.Count(ct => ct.SubTemaId == juego.SubTemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirSubTema(juego.SubTemaId, clienteId);
            }
        }

        //juegoOnlines
        public static void AnyadirJuegoOnlineConHijos(int juegoOnlineId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteJuegoOnlinesActuales = db.ClienteJuegoOnlines.Where(acc => acc.JuegoOnlineId == juegoOnlineId).ToList();
            var accesosQuitar = clienteJuegoOnlinesActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteJuegoOnlines.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteJuegoOnlinesActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirJuegoOnline(juegoOnlineId, clienteSeleccionado);
            }
        }

        private static void AnyadirJuegoOnline(int juegoOnlineId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteJuegoOnline clienteJuegoOnline = new ClienteJuegoOnline();
            clienteJuegoOnline.ClienteId = clienteId;
            clienteJuegoOnline.JuegoOnlineId = juegoOnlineId;

            db.ClienteJuegoOnlines.Add(clienteJuegoOnline);
            db.SaveChanges();

            var juegoOnline = db.JuegoOnlines.Find(juegoOnlineId);
            if (db.ClienteSubTemas.Count(ct => ct.SubTemaId == juegoOnline.SubTemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirSubTema(juegoOnline.SubTemaId, clienteId);
            }
        }

        //subTemaVideos
        public static void AnyadirSubTemaVideoConHijos(int subTemaVideoId, List<int> clientesSeleccionados)
        {
            AuthContext db = new AuthContext();

            var clienteSubTemaVideosActuales = db.ClienteSubTemaVideos.Where(acc => acc.SubTemaVideoId == subTemaVideoId).ToList();
            var accesosQuitar = clienteSubTemaVideosActuales.Where(acc => !clientesSeleccionados.Contains(acc.ClienteId));
            db.ClienteSubTemaVideos.RemoveRange(accesosQuitar);
            db.SaveChanges();

            var clientesActuales = clienteSubTemaVideosActuales.Select(acc => acc.ClienteId);
            var clientesAnyadir = clientesSeleccionados.Where(cli => !clientesActuales.Contains(cli));
            foreach (var clienteSeleccionado in clientesAnyadir)
            {
                AnyadirSubTemaVideo(subTemaVideoId, clienteSeleccionado);
            }
        }

        private static void AnyadirSubTemaVideo(int subTemaVideoId, int clienteId)
        {
            AuthContext db = new AuthContext();
            ClienteSubTemaVideo clienteSubTemaVideo = new ClienteSubTemaVideo();
            clienteSubTemaVideo.ClienteId = clienteId;
            clienteSubTemaVideo.SubTemaVideoId = subTemaVideoId;

            db.ClienteSubTemaVideos.Add(clienteSubTemaVideo);
            db.SaveChanges();

            var subTemaVideo = db.SubTemaVideos.Find(subTemaVideoId);
            if (db.ClienteSubTemas.Count(ct => ct.SubTemaId == subTemaVideo.SubTemaId && ct.ClienteId == clienteId) == 0)
            {
                AnyadirSubTema(subTemaVideo.SubTemaId, clienteId);
            }
        }
        
    }
}