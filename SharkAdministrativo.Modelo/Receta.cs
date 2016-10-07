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
    
    public partial class Receta
    {
        public int id { get; set; }
        public int insumo_id { get; set; }
        public int producto_id { get; set; }
        public int insumoElaborado_id { get; set; }
        public Nullable<double> cantidad { get; set; }
        public string almacenes_id { get; set; }
    
        public virtual Insumo Insumo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual InsumoElaborado InsumoElaborado { get; set; }

        /// <summary>
        /// Registra un objeto receta en la base de datos.
        /// </summary>
        /// <param name="ingrediente">Objeto a registrar.</param>
        public void registrar(Receta ingrediente)
        {
            using (bdsharkEntities db = new bdsharkEntities())
            {
                db.Configuration.LazyLoadingEnabled = true;
                if (ingrediente.InsumoElaborado != null)
                {
                    db.InsumosElaborados.Attach(ingrediente.InsumoElaborado);
                }
                else
                {
                    db.Productos.Attach(ingrediente.Producto);
                }
                db.Recetas.Add(ingrediente);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Modifíca un objeto receta específico.
        /// </summary>
        /// <param name="ingrediente">Objeto a modificar.</param>
        public void modificar(Receta ingrediente)
        {

            using (bdsharkEntities db = new bdsharkEntities())
            {
                Receta n_ingrediente = db.Recetas.Find(ingrediente.id);
                n_ingrediente.almacenes_id = ingrediente.almacenes_id;
                n_ingrediente.cantidad = ingrediente.cantidad;
                n_ingrediente.insumo_id = ingrediente.insumo_id;
                n_ingrediente.insumoElaborado_id = ingrediente.insumoElaborado_id;
                n_ingrediente.producto_id = ingrediente.producto_id;

                db.Recetas.Attach(n_ingrediente);
                db.Entry(n_ingrediente).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene una lista de objetos para formar la receta.
        /// </summary>
        /// <param name="indicador">Indica si obtendremos la receta de un insumo elaborado o de un producto.</param>
        /// <param name="id">Parámetro de búsqueda.</param>
        /// <returns></returns>
        public List<Receta> obtenerIngredientesDeReceta(string indicador, int id)
        {
            List<Receta> ingredientes = new List<Receta>();
            bdsharkEntities db = new bdsharkEntities();
            var Query = from receta in db.Recetas select receta;
            if (indicador == "IE")
            {
                Query = from receta in db.Recetas where receta.insumoElaborado_id == id select receta;
            }
            else
            {
                Query = from receta in db.Recetas where receta.producto_id == id select receta;
            }
            foreach (var ingrediente in Query)
            {
                ingredientes.Add(ingrediente);
            }
            return ingredientes;
        }


        /// <summary>
        /// Elimina un objeto receta de la base de datos.
        /// </summary>
        /// <param name="_ingrediente">El objeto a eliminar</param>
        public void eliminarIngrediente(Receta _ingrediente)
        {
            using (bdsharkEntities db = new bdsharkEntities())
            {
                var RecetaQuery = from ingrediente in db.Recetas where ingrediente.id == _ingrediente.id select ingrediente;

                foreach (var ingrediente in RecetaQuery)
                {
                    db.Entry(ingrediente).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
        }

    }
}
