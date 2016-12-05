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
    using System.Data;
    using System.Linq;
    using SDKCONTPAQi;
    using System.Windows.Forms;
    public partial class Producto
    {
        public Producto()
        {
            this.ProductoPromocion = new HashSet<ProductoPromocion>();
            this.Receta = new HashSet<Receta>();
        }
    
        public int id { get; set; }
        public string descripcion { get; set; }
        public string nombre { get; set; }
        public Nullable<double> ultimoPrecio { get; set; }
        public Nullable<double> IVA { get; set; }
        public Nullable<double> precioConImpuesto { get; set; }
        public string areasPreparacion { get; set; }
        public string disponlibleEn { get; set; }
        public byte[] imagen { get; set; }
        public string codigo { get; set; }
    
        public virtual ICollection<ProductoPromocion> ProductoPromocion { get; set; }
        public virtual ICollection<Receta> Receta { get; set; }


        /// <summary>
        /// Obtiene una lista de todos los productos.
        /// </summary>
        /// <returns>La lista obtenida</returns>
        public List<Producto> obtenerTodos()
        {
            List<Producto> productos = new List<Producto>();
            try{
                bdsharkEntities db = new bdsharkEntities();

                db.Configuration.LazyLoadingEnabled = true;
                var productosQuery = from producto in db.Productos select producto;
                foreach (var producto in productosQuery)
                {
                    productos.Add(producto);
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
            return productos;
        }

        /// <summary>
        /// Registra un objeto producto en la base de datos.
        /// </summary>
        /// <param name="producto">Objeto a registrar.</param>
        public void registrar(Producto producto)
        {
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    db.Productos.Add(producto);
                    db.SaveChanges();
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
        }

        /// <summary>
        /// Modifica un objeto producto en la base de datos.
        /// </summary>
        /// <param name="producto">Objeto a modificar.</param>
        public void modificar(Producto producto)
        {

             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    Producto n_producto = db.Productos.Find(producto.id);
                    n_producto.areasPreparacion = producto.areasPreparacion;
                    n_producto.descripcion = producto.descripcion;
                    n_producto.disponlibleEn = producto.disponlibleEn;
                    n_producto.IVA = producto.IVA;
                    n_producto.nombre = producto.nombre;
                    n_producto.precioConImpuesto = producto.precioConImpuesto;
                    n_producto.ultimoPrecio = producto.ultimoPrecio;
                    if (producto.imagen != null)
                    {
                        n_producto.imagen = producto.imagen;
                    }


                    db.Productos.Attach(n_producto);
                    db.Entry(n_producto).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
        }

        /// <summary>
        /// Obtiene un objeto producto específico.
        /// </summary>
        /// <param name="id">Parámetro de búsqueda.</param>
        /// <returns>El objeto encontrado.</returns>
        public Producto obtenerPorID(int id)
        {
            Producto producto = new Producto();
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    producto = db.Productos.Find(id);
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
            return producto;
        }

        /// <summary>
        /// Obtiene un objeto producto específico.
        /// </summary>
        /// <param name="name">Parámetro de búsqueda.</param>
        /// <returns>El objeto encontrado.</returns>
        public Producto obtener(string name)
        {
            Producto product = new Producto();
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    db.Configuration.LazyLoadingEnabled = true;
                    var productosQuery = from producto in db.Productos where producto.nombre == name select producto;
                    foreach (var producto in productosQuery)
                    {
                        product = producto;
                    }
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
            return product;
        }

        /// <summary>
        /// Elimina un objeto producto en la base de datos.
        /// </summary>
        /// <param name="_producto">Óbjeto a eliminar.</param>
        public void eliminar(Producto _producto)
        {
             try{
                 using (bdsharkEntities db = new bdsharkEntities())
                 {
                     var productosQuery = from producto in db.Productos where producto.id == _producto.id select producto;

                     foreach (var producto in productosQuery)
                     {
                         db.Entry(producto).State = EntityState.Deleted;
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
