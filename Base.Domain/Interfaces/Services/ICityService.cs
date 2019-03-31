using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Interfaces.Services
{
    public interface ICityService : IServiceBase<Cities>
    {
        Cities Include(Cities city, string fieldToFill);
    }
}
