using Base.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities
{
    public class Cities
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public int? StateID { get; set; }
        public string CodigoIBGE { get; set; }

        // relacionamentos
        public States State { get; set; }

        public Cities Include(ICityService cityService, string fieldToFill)
        {
            return cityService.Include(this, fieldToFill);
        }
    }
}
