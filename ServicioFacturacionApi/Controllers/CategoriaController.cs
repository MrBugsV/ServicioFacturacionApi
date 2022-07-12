using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ServicioFacturacionApi.Entidades;
using System.Web.Http.Cors;

namespace ServicioFacturacionApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriaController : ApiController
    {
        #region Servicios REST

        // GET api/Categoria
        public List<CategoriaEntidad> Get()
        {
            return obtenerLista();
        }

        //// GET api/Categoria/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/Categoria
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/Categoria/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/Categoria/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        #region Metodos Base de Datos
        public List<CategoriaEntidad> obtenerLista()
        {
            try
            {
                List<Categoria> categorias;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Categoria
                                    select p;
                    categorias = resultado.ToList();
                }

                List<CategoriaEntidad> categoriaEntidad = new List<CategoriaEntidad>();

                foreach (Categoria item in categorias)
                {
                    categoriaEntidad.Add(new CategoriaEntidad(
                            item.Id,
                            item.Nombre));
                }

                return categoriaEntidad;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        #endregion
    }
}