using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SharkAdministrativo.Modelo
{
    public partial class bdsharkEntities : DbContext
    {
        public bdsharkEntities(string cadenaConexion)
            : base(cadenaConexion)
        {

        }
    }
}
