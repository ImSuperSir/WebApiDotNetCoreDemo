namespace _002_WebApiAutores.Entidades
{
    public class AutorLibro
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }

        public int Orden { get; set; }


        public Libro Libro { get; set; } /*Esta es una propiedad de navegacion*/

        public Autor Autor { get; set; }    /*Esta es una propiedad de navegacion*/

    }
}
