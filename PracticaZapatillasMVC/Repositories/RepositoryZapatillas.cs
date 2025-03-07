using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaZapatillasMVC.Data;
using PracticaZapatillasMVC.Models;
#region PROCEDURES
/*CREATE PROCEDURE SP_PAGINACION_ZAPAS
(@posicion int, @idproducto int, @registros int out)
AS
	SELECT @registros=COUNT(IDPRODUCTO)FROM IMAGENESZAPASPRACTICA
	WHERE IDPRODUCTO=@idproducto;

	SELECT IDIMAGEN, IDPRODUCTO,IMAGEN FROM
		(SELECT CAST(ROW_NUMBER() OVER(ORDER BY IDPRODUCTO)AS INT) AS POSICION,
		IDIMAGEN, IDPRODUCTO,IMAGEN FROM IMAGENESZAPASPRACTICA  
		WHERE IDPRODUCTO=@idproducto)QUERY
	WHERE POSICION=@posicion
GO*/
#endregion
namespace PracticaZapatillasMVC.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapasContext context;

        public RepositoryZapatillas(ZapasContext context)
        {
            this.context=context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            var zapatillas = await this.context.Zapatillas.ToListAsync();
            return zapatillas;
        }

        public async Task<Zapatilla>DetallesZapatilla(int idZapatilla)
        {
            var zapatilla = await this.context.Zapatillas.Where(z => z.IdProducto==idZapatilla).FirstOrDefaultAsync();
            return zapatilla;
        }
        public async Task<(int,Zapatilla, ImagenZapatillas)> PaginacionImagenesZapatillas(int posicion,int prodId)
        {
            Zapatilla zapatilla = await DetallesZapatilla(prodId);
            string sql = "SP_PAGINACION_ZAPAS @posicion, @idproducto, @registros out";
            SqlParameter paramposicion = new SqlParameter("@posicion", posicion);
            SqlParameter paramProdId = new SqlParameter("@idproducto", prodId);
            SqlParameter paramRegistro = new SqlParameter("@registros", 0);
            paramRegistro.Direction=System.Data.ParameterDirection.Output;
            var consulta =  this.context.Imagenes.FromSqlRaw(sql,paramposicion,paramProdId,paramRegistro);
            List<ImagenZapatillas> imagenes = await consulta.ToListAsync();
            ImagenZapatillas imagen =  imagenes.FirstOrDefault();
            int registros = int.Parse(paramRegistro.Value.ToString());
            return (registros, zapatilla, imagen);
        }
    }
}
