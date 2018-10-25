using AutoMapper;
using PoohAPI.Infrastructure.OptionDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Options.Services
{
    public class OptionReadService : IOptionReadService
    {
        private readonly IOptionRepository optionRepository;
        private readonly IMapper _mapper;

        public OptionReadService(IOptionRepository optionRepository, IMapper mapper)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
        }        
    }
}