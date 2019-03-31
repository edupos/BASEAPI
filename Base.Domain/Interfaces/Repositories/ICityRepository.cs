using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Interfaces.Repositories
{
    public interface ICityRepository : IRepositoryBase<Cities>
    {
        Cities Include(Cities city, string fieldToFill);
    }
}

