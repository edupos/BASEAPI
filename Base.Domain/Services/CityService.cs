using Base.Domain.Entities;
using Base.Domain.Interfaces.Repositories;
using Base.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Services
{
    public class CityService : ServiceBase<Cities>, ICityService
    {
        private ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository) : base(cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public Cities Include(Cities city, string fieldToFill)
        {
            return _cityRepository.Include(city, fieldToFill);
        }

    }
}
