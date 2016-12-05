using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Modelo
{
    public partial class bdsharkEntities : DbContext
    {
        public bdsharkEntities(string cadenaConexion)
            : base(SDK.companyConnection)
        {

        }
    }

}
