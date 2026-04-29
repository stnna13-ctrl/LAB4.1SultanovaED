using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppSultanovaED.Model;

namespace WpfAppSultanovaED.Helper
{
    public class FindRole
    {
        private int _roleId;

        public FindRole(int roleId)
        {
            _roleId = roleId;
        }

        public bool RolePredicate(Role role)
        {
            return role.Id == _roleId;
        }
    }
}
