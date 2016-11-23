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
    
    public partial class Presentacion
    {
        public Presentacion()
        {
            this.EntradaPresentacion = new HashSet<EntradaPresentacion>();
        }
    
        public int id { get; set; }
        public string descripcion { get; set; }
        public Nullable<double> ultimo_costo { get; set; }
        public Nullable<double> costo_promedio { get; set; }
        public Nullable<double> IVA { get; set; }
        public Nullable<double> costo_con_impuesto { get; set; }
        public Nullable<double> rendimiento { get; set; }
        public int minimo { get; set; }
        public int proveedor_id { get; set; }
        public int insumo_id { get; set; }
        public int factura_id { get; set; }
        public int almacen_id { get; set; }
        public string noIdentificacion { get; set; }
        public Nullable<double> costo_unitario { get; set; }
        public Nullable<double> cantidad { get; set; }
        public Nullable<double> existencia { get; set; }
        public string codigo { get; set; }
    
        public virtual Almacen Almacen { get; set; }
        public virtual ICollection<EntradaPresentacion> EntradaPresentacion { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public virtual Insumo Insumo { get; set; }
        public virtual Factura Factura { get; set; }
        public virtual Almacen Almacen1 { get; set; }

        /// <summary>
        /// Registra un objeto presentacion en la base de datos.
        /// </summary>
        /// <param name="presentacion">Parámetro de búsqueda.</param>
        public void registrar(Presentacion presentacion)
        {
            using (bdsharkEntities db = new bdsharkEntities())
            {

                db.Configuration.LazyLoadingEnabled = true;

                db.Almacenes.Attach(presentacion.Almacen);
                db.Proveedores.Attach(presentacion.Proveedor);
                db.Insumos.Attach(presentacion.Insumo);
                if (presentacion.Factura != null)
                {
                    db.Facturas.Attach(presentacion.Factura);
                }
                db.Presentaciones.Add(presentacion);
                db.SaveChanges();
            }
        }


        /// <summary>
        /// Obtiene una lista de presentaciones de acuerdo al insumo especificado.
        /// </summary>
        /// <param name="insumo_clave">Parámetro de búsqueda.</param>
        /// <returns>La lista de objetos obtenida.</returns>
        public List<Presentacion> obtenerTodosPorInsumo(int insumo_clave)
        {
            List<Presentacion> presentaciones = new List<Presentacion>();
            bdsharkEntities db = new bdsharkEntities();

            db.Configuration.LazyLoadingEnabled = true;
            var presentacionesQuery = from presentacion in db.Presentaciones where presentacion.insumo_id == insumo_clave select presentacion;
            foreach (var presentacion in presentacionesQuery)
            {
                presentaciones.Add(presentacion);
            }

            return presentaciones;
        }

        /// <summary>
        /// Verifica la existencia o ausencia del objeto insumo.
        /// </summary>
        /// <param name="presentation">Objeto a buscar.</param>
        /// <returns>Variable Bool que afirma o niega la existencia del objeto.</returns>
        public bool verificarRegistro(Presentacion presentation)
        {
            bool registrado = false;

            using (bdsharkEntities db = new bdsharkEntities())
            {
                try
                {
                    db.Configuration.LazyLoadingEnabled = true;
                    var insumoQuery = from presentacion in db.Presentaciones
                                      where presentacion.Proveedor.id == presentation.Proveedor.id
                                      where presentacion.noIdentificacion == presentacion.noIdentificacion
                                      where presentacion.Insumo.id == presentation.Insumo.id
                                      where presentacion.descripcion == presentation.descripcion
                                      select presentacion;
                    // Iterate through the results of the parameterized query.
                    foreach (var presentacionR in insumoQuery)
                    {
                        presentacionR.cantidad = presentation.cantidad + presentacionR.cantidad;
                        presentacionR.existencia = presentacionR.cantidad * presentacionR.rendimiento;
                        db.Entry(presentacionR).State = EntityState.Modified;
                        registrado = true;

                    }
                }
                catch (Exception e)
                {
                    Console.Write("ERROR: " + e);
                }
                db.SaveChanges();
            }
            return registrado;
        }



        /// <summary>
        /// Elimina un objeto presentación especificado.
        /// </summary>
        /// <param name="d_presentacion">Parámetro de búsqueda.</param>
        public void eliminar(Presentacion d_presentacion)
        {
            using (bdsharkEntities db = new bdsharkEntities())
            {
                var presentacionQuery = from presentacion in db.Presentaciones where presentacion.id == d_presentacion.id select presentacion;

                foreach (var presentacion in presentacionQuery)
                {
                    db.Entry(presentacion).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
        }

        public Presentacion obtener(Presentacion presentation)
        {
            Presentacion r_presentacion = new Presentacion();
            using (bdsharkEntities db = new bdsharkEntities())
            {
                db.Configuration.LazyLoadingEnabled = true;
                var insumoQuery = from presentacion in db.Presentaciones
                                  where presentacion.Proveedor.id == presentation.Proveedor.id
                                  where presentacion.noIdentificacion == presentacion.noIdentificacion
                                  where presentacion.Almacen.id == presentation.Almacen.id
                                  where presentacion.Insumo.id == presentation.Insumo.id
                                  select presentacion;
                foreach (var presentacionR in insumoQuery)
                {
                    r_presentacion = presentacionR;

                }
                return r_presentacion;
            }
        }

        public Presentacion obtener(int id)
        {
            Presentacion r_presentacion = new Presentacion();
            using (bdsharkEntities db = new bdsharkEntities())
            {
                db.Configuration.LazyLoadingEnabled = true;
                var insumoQuery = from presentacion in db.Presentaciones
                                  where presentacion.id == id
                                  select presentacion;
                foreach (var presentacionR in insumoQuery)
                {
                    r_presentacion = presentacionR;

                }
                return r_presentacion;
            }
        }


        public Presentacion get(string name)
        {
            Presentacion r_presentacion = new Presentacion();
            using (bdsharkEntities db = new bdsharkEntities())
            {
                db.Configuration.LazyLoadingEnabled = true;
                var insumoQuery = from presentacion in db.Presentaciones where presentacion.descripcion == name select presentacion;
                foreach (var presentacionR in insumoQuery)
                {
                    r_presentacion = presentacionR;

                }
                return r_presentacion;
            }
        }

        public void sumarEntrada(int id, double cantidad)
        {
            Presentacion presentacion = new Presentacion();
            using (bdsharkEntities db = new bdsharkEntities())
            {
                presentacion = db.Presentaciones.Find(id);
                presentacion.cantidad = cantidad + presentacion.cantidad;
                presentacion.existencia = presentacion.cantidad * presentacion.rendimiento;
                db.Presentaciones.Attach(presentacion);
                db.Entry(presentacion).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Presentacion> getAll()
        {
            List<Presentacion> presentaciones = new List<Presentacion>();
            bdsharkEntities db = new bdsharkEntities();

            db.Configuration.LazyLoadingEnabled = true;
            var presentacionesQuery = from presentacion in db.Presentaciones select presentacion;
            foreach (var presentacion in presentacionesQuery)
            {
                presentaciones.Add(presentacion);
            }

            return presentaciones;
        }

    }
}
