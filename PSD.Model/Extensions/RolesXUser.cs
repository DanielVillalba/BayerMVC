using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class RolesXUser
    {
        public static RolesXUser NewEmpty()
        {
            return new RolesXUser()
            {

            };
        }
        public static IEnumerable<RolesXUser> NewEmptyList()
        {
            return new List<RolesXUser>() { };
        }
    }
}
