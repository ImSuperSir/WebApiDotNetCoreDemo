using Microsoft.EntityFrameworkCore;

namespace _002_WebApiAutores.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertaParametrosPaginacionEnCabecera<T>(this HttpContext httpContext
            , IQueryable<T> queryable )
        {
            if (queryable == null) { throw new ArgumentNullException(nameof(HttpContext)); }

            Int64 lCantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Add("cantidadTotalRegistros", lCantidad.ToString());
        }
    }
}
