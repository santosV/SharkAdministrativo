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
    
    public partial class Grupo
    {
        public Grupo()
        {
            this.Insumo = new HashSet<Insumo>();
            this.InsumoElaborado = new HashSet<InsumoElaborado>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public int categoria_id { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<Insumo> Insumo { get; set; }
        public virtual ICollection<InsumoElaborado> InsumoElaborado { get; set; }

        /// <summary>
        /// Registra un grupo en la base de datos.
        /// </summary>
        /// <param name="grupo">El objeto a registrar.</param>
        public void registrar(Grupo grupo)
        {
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    db.Configuration.LazyLoadingEnabled = true;
                    db.Categorias.Attach(grupo.Categoria);
                    db.Grupos.Add(grupo);
                    db.SaveChanges();
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
        }

        /// <summary>
        /// Modifica un objeto grupo.
        /// </summary>
        /// <param name="grupo"></param>
        public void Modify(Grupo grupo)
        {

             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    Grupo group = db.Grupos.Find(grupo.id);
                    group.nombre = grupo.nombre;
                    group.categoria_id = grupo.categoria_id;
                    db.Grupos.Attach(group);
                    db.Entry(group).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
        }

        /// <summary>
        /// Obtiene de la base de datos un objeto grupo a través del ID.
        /// </summary>
        /// <param name="id">parametro con el que se busca en la base de datos.</param>
        /// <returns>El objeto encontrado en la base de datos.</returns>
        public Grupo getForID(int id)
        {
            Grupo grupo = new Grupo();
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                grupo = db.Grupos.Find(id);
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
            return grupo;
        }

        /// <summary>
        /// Obtiene todos los registros de grupo.
        /// </summary>
        /// <returns>La lista de grupos.</returns>
        public List<Grupo> obtenerTodos()
        {
            List<Grupo> grupos = new List<Grupo>();
            try{
                bdsharkEntities db = new bdsharkEntities();

                db.Configuration.LazyLoadingEnabled = true;
                var gruposQuery = from grupo in db.Grupos select grupo;
                foreach (var grupo in gruposQuery)
                {
                    grupos.Add(grupo);
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }

            return grupos;
        }

        /// <summary>
        /// Obtiene un objeto grupo de la base de datos.
        /// </summary>
        /// <param name="name">Parámetro de búsqueda.</param>
        /// <returns>El objeto encontrado.</returns>
        public Grupo obtener(string name)
        {
            Grupo group = new Grupo();
             try{ 
                using(bdsharkEntities db = new bdsharkEntities())
                {
                    db.Configuration.LazyLoadingEnabled = true;
                    var grupoQuery = from grupo in db.Grupos where grupo.nombre == name select grupo;
                    foreach (var grupo in grupoQuery)
                    {
                        group = grupo;
                    }
                }
            }catch(Exception ex){
                MessageBox.Show("Error: "+ex+"\nError en la autenticación con la base de datos", "Aviso Shark" );
            }
           
            return group;
        }

        /// <summary>
        /// Elimina un grupo creado en shark.
        /// </summary>
        /// <param name="d_grupo"></param>
        public void delete(Grupo d_grupo)
        {
             try{
                 using (bdsharkEntities db = new bdsharkEntities())
                 {
                     var Query = from grupo in db.Grupos where grupo.id == d_grupo.id select grupo;
                     foreach (var grupo in Query)
                     {
                         db.Entry(grupo).State = EntityState.Deleted;
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
