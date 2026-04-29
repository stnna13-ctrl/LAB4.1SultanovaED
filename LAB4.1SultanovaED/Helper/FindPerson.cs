using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppSultanovaED.Model;

namespace WpfAppSultanovaED.Helper
{
    public class FindPerson
    {
        private int _id;

        /// <summary>
        /// Конструктор для поиска по идентификатору
        /// </summary>

        public FindPerson(int id)
        {
            _id = id;
        }

        /// <summary>
        /// Метод предиката для поиска по идентификатору
        /// </summary>

        public bool PersonPredicate(Person person)
        {
            return person.Id == _id;
        }
    }
}

