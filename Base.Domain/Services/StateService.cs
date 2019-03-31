using Base.Domain.Entities;
using Base.Domain.Interfaces.Repositories;
using Base.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Services
{
    public class StateService : ServiceBase<States>, IStateService
    {
        private IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository) : base(stateRepository)
        {
            _stateRepository = stateRepository;
        }

    }
}
