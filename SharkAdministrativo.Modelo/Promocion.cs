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
    using System.Windows.Forms;
    
    public partial class Promocion
    {
        public Promocion()
        {
            this.ProductoPromocion = new HashSet<ProductoPromocion>();
        }
    
        public int id { get; set; }
        public string descripcion { get; set; }
        public string nombre { get; set; }
        public Nullable<double> ultimoPrecio { get; set; }
        public Nullable<double> IVA { get; set; }
        public Nullable<double> precioConImpuesto { get; set; }
        public string areasDisponibles { get; set; }
        public byte[] imagen { get; set; }
        public string diasDisponibles { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public Nullable<System.DateTime> fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
    
        public virtual ICollection<ProductoPromocion> ProductoPromocion { get; set; }

        /// <summary>
        /// Registra un objeto promoción en la base de datos.
        /// </summary>
        /// <param name="promocion">Objeto a reistrar.</param>
        public void registrar(Promocion promocion)
        {
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    db.Promociones.Add(promocion);
                    db.SaveChanges();
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
        }

        /// <summary>
        /// Obtiene todos los registros de objetos de productos.
        /// </summary>
        /// <returns>La lista obtenida.</returns>
        public List<Promocion> obtenerTodos()
        {
            List<Promocion> promociones = new List<Promocion>();
            try{
            bdsharkEntities db = new bdsharkEntities();

                db.Configuration.LazyLoadingEnabled = true;
                var promocionesQuery = from promocion in db.Promociones select promocion;
                foreach (var promocion in promocionesQuery)
                {
                    promociones.Add(promocion);
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
            return promociones;
        }

        /// <summary>
        /// Obtiene un objeto promoción específico.
        /// </summary>
        /// <param name="id">Parámetro de búsqueda.</param>
        /// <returns>EL objeto encontrado.</returns>
        public Promocion obtenerPorId(int id)
        {
            Promocion promocion = new Promocion();
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                promocion = db.Promociones.Find(id);
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
            return promocion;
        }

        /// <summary>
        /// Modifica un objeto promoción específico.
        /// </summary>
        /// <param name="promocion">El objeto a modificar.</param>
        public void modificar(Promocion promocion)
        {

             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    Promocion n_promocion = db.Promociones.Find(promocion.id);
                    n_promocion.descripcion = promocion.descripcion;
                    n_promocion.areasDisponibles = promocion.areasDisponibles;
                    n_promocion.diasDisponibles = promocion.diasDisponibles;
                    n_promocion.IVA = promocion.IVA;
                    n_promocion.nombre = promocion.nombre;
                    n_promocion.precioConImpuesto = promocion.precioConImpuesto;
                    n_promocion.ultimoPrecio = promocion.ultimoPrecio;
                    n_promocion.fecha_inicio = promocion.fecha_inicio;
                    n_promocion.fecha_fin = promocion.fecha_fin;
                    n_promocion.hora_inicio = promocion.hora_inicio;
                    n_promocion.hora_fin = promocion.hora_fin;

                    if (promocion.imagen != null)
                    {
                        n_promocion.imagen = promocion.imagen;
                    }

                    db.Promociones.Attach(n_promocion);
                    db.Entry(n_promocion).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
        }

        /// <summary>
        /// Elimina un objeto promoción de la base de datos.
        /// </summary>
        /// <param name="_promocion">El objeto a elimianr.</param>
        public void eliminar(Promocion _promocion)
        {
             try{
                 using (bdsharkEntities db = new bdsharkEntities())
                 {
                     var promocionQuery = from promocion in db.Promociones where promocion.id == _promocion.id select promocion;

                     foreach (var promocion in promocionQuery)
                     {
                         db.Entry(promocion).State = EntityState.Deleted;
                     }
                     db.SaveChanges();
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error: " + ex + "\nError en la autenticación con la base de datos", "Aviso Shark");
             }
        }
    }
}
