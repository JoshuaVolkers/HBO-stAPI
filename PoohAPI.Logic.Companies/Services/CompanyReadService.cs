using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Infrastructure.CompanyDB.Respositories;
using AutoMapper;

namespace PoohAPI.Logic.Companies.Services
{
    public class CompanyReadService : ICompanyReadService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompanyReadService(ICompanyRepository companyRepository, IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }

        public Company GetCompanyById(int id)
        {
            //var query = string.Format("SELECT b.*, l.land_naam " +
            //    "FROM reg_bedrijven b " +
            //    "INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id " +
            //    "WHERE b.bedrijf_id = {0}", id);

            var query = "SELECT b.*, l.land_naam " +
                "FROM reg_bedrijven b " +
                "INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id " +
                "WHERE b.bedrijf_id = @id";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            var dbCompany = this.companyRepository.GetCompany(query, parameters);
            
            return this.mapper.Map<Company>(dbCompany);
        }

        public IEnumerable<Company> GetListCompanies(int maxCount, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
