using Base.Domain.Entities;
using Base.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Infra.Data.Repositories
{
    public class CityRepository : RepositoryBase<Cities>, ICityRepository
    {
        private readonly IConfiguration _config;
        public CityRepository(IConfiguration config) : base(config, "CityID")
        {
            _config = config;
        }

        public Cities Include(Cities city, string fieldToFill)
        {
            foreach (var field in fieldToFill.Split("|"))
            {
                if (field.ToUpper() == "State".ToUpper() || fieldToFill == "ALL")
                {
                    StateRepository stateRepository = new StateRepository(_config);
                    if(city.StateID != null) city.State = stateRepository.GetById((int)city.StateID);
                }
            }
            return city;
        }
    }
}
