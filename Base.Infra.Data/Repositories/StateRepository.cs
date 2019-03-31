using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Entities;
using Base.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Base.Infra.Data.Repositories
{
    public class StateRepository : RepositoryBase<States>, IStateRepository
    {
        public StateRepository(IConfiguration config) : base(config, "StateID")
        {

        }

    }
}
