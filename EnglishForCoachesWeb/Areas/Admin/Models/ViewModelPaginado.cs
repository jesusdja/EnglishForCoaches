using System;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class ViewModelPaginado
    {
        public int resultadosPorPagina = 20;
        public int paginasMostrar = 5;

        public int Pagina { get; set; }

        public int PaginaMin { get; set; }
        public int PaginaMax { get; set; }
        public int NPaginas { get; set; }


        public void CalcularPaginacion(int resultados)
        {

            double paginas = resultados / resultadosPorPagina;
            int NumPaginas = (int)Math.Round(paginas + 0.51);

            if (Pagina < 1)
            {
                Pagina = 1;
            }

            if (Pagina > NumPaginas)
            {
                Pagina = NumPaginas;
            }

            PaginaMin = Pagina - paginasMostrar;
            PaginaMax = Pagina + paginasMostrar;

            NPaginas = NumPaginas;
            PaginaMin = PaginaMin < 1 ? 1 : PaginaMin;
            PaginaMax = PaginaMax > NPaginas ? NPaginas : PaginaMax;            
        }
    }
}