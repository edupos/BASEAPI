using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities
{
    public class States
    {
        public int StateID { get; set; }
        public string Name { get; set; }
        public string UF { get; set; }
        public int? CountryID { get; set; }
    }
}
