using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppSultanovaED.ViewModel;

namespace WpfAppSultanovaED.Model

{
    public class Person
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public Person() { }

        public Person CopyFromPersonDPO(PersonDpo p, RoleViewModel vmRole)
        {
            int roleId = 0;

            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == p.RoleName)
                {
                    roleId = r.Id;
                    break;
                }
            }

            if (roleId != 0)
            {
                this.Id = p.Id;
                this.RoleId = roleId;
                this.FirstName = p.FirstName;
                this.LastName = p.LastName;
                this.Birthday = p.Birthday;
            }

            return this;
        }



        public Person(int id, int roleId, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            RoleId = roleId;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }
    }
}