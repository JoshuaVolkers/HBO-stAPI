using AutoMapper;
using PoohAPI.Infrastructure.OptionDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.OptionModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Options.Services
{
    public class OptionReadService : IOptionReadService
    {
        private readonly IOptionRepository _optionRepository;
        private readonly IMapper _mapper;
        private readonly IQueryBuilder _queryBuilder;

        public OptionReadService(IOptionRepository optionRepository, IMapper mapper, IQueryBuilder queryBuilder)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
            _queryBuilder = queryBuilder;
        }

        public IEnumerable<Major> GetAllMajors(int maxCount, int offset)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            _queryBuilder.AddSelect(@"*");
            _queryBuilder.SetFrom("reg_opleidingen");            
            _queryBuilder.SetLimit("@limit");
            _queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);
                        
            string query = _queryBuilder.BuildQuery();

            var majors = _optionRepository.GetAllMajors(query, parameters);
            return _mapper.Map<IEnumerable<Major>>(majors);
        }

        public IEnumerable<EducationLevel> GetAllEducationLevels(int maxCount, int offset)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            _queryBuilder.AddSelect(@"*");
            _queryBuilder.SetFrom("reg_opleidingsniveau");
            _queryBuilder.SetLimit("@limit");
            _queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);

            string query = _queryBuilder.BuildQuery();

            var edLevels = _optionRepository.GetAllEducationLevels(query, parameters);
            return _mapper.Map<IEnumerable<EducationLevel>>(edLevels);
        }
    }
}