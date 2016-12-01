//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SharkAdministrativo.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using SDKCONTPAQi;
    
    public partial class ProductoPromocion
    {
        public int id { get; set; }
        public int producto_id { get; set; }
        public int promocion_id { get; set; }
        public Nullable<double> cantidad { get; set; }
    
        public virtual Producto Producto { get; set; }
        public virtual Promocion Promocion { get; set; }

        /// <summary>
        /// Registra un objeto ProductoPromcion en la base de datos.
        /// </summary>
        /// <param name="detalle">el objeto a registrar.</param>
        public void registrar(ProductoPromocion detalle)
        {
            using (bdsharkEntities db = new bdsharkEntities(SDK.companyConnection))
            {
                db.Configuration.LazyLoadingEnabled = true;
                db.Productos.Attach(detalle.Producto);
                db.Promociones.Attach(detalle.Promocion);
                db.ProductoPromocion.Add(detalle);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene un objeto ProductoPromocion específico.
        /// </summary>
        /// <param name="id">Parámetro de búsqueda.</param>
        /// <returns>El objeto encontrado.</returns>
        public ProductoPromocion obtener(int id)
        {
            ProductoPromocion detalle = new ProductoPromocion();
            using (bdsharkEntities db = new bdsharkEntities(SDK.companyConnection))
            {
                detalle = db.ProductoPromocion.Find(id);

            }
            return detalle;
        }

        /// <summary>
        /// Obtiene todos los registros de ProductoPromoción de la base de datos.
        /// </summary>
        /// <param name="id">Parámetro de búsqueda.</param>
        /// <returns>La lista obtenida.</returns>
        public List<ProductoPromocion> obtenerTodos(int id)
        {
            List<ProductoPromocion> detalles = new List<ProductoPromocion>();
            bdsharkEntities db = new bdsharkEntities(SDK.companyConnection);

            db.Configuration.LazyLoadingEnabled = true;
            var detallesQuery = from detalle in db.ProductoPromocion where detalle.promocion_id == id select detalle;
            foreach (var detalle in detallesQuery)
            {
                detalles.Add(detalle);
            }

            return detalles;
        }

        /// <summary>
        /// Modifíca un objeto producto en la base de datos.
        /// </summary>
        /// <param name="detalle">El objeto a modificar.</param>
        public void modificar(ProductoPromocion detalle)
        {

            using (bdsharkEntities db = new bdsharkEntities(SDK.companyConnection))
            {
                ProductoPromocion n_detalle = db.ProductoPromocion.Find(detalle.id);
                n_detalle.cantidad = detalle.cantidad;
                n_detalle.producto_id = detalle.producto_id;
                n_detalle.promocion_id = detalle.promocion_id;

                db.Entry(n_detalle).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        /// <summary>
        /// Elimina un objeto ProductoPromocion en la base de datos.
        /// </summary>
        /// <param name="_detalle">El objeto a eliminar.</param>
        public void eliminar(ProductoPromocion _detalle)
        {
            using (bdsharkEntities db = new bdsharkEntities(SDK.companyConnection))
            {
                var Query = from detalle in db.ProductoPromocion where detalle.id == _detalle.id select detalle;

                foreach (var detalle in Query)
                {
                    db.Entry(detalle).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
        }
    }
}
