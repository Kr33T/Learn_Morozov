using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_Morozov
{
    public partial class Client
    {
        public string FIO
        {
            get
            {
                return FirstName + " " + LastName + " " + Patronymic;
            }
        }
    }
}
