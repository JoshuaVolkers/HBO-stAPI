using PoohAPI.Infrastructure.CompanyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PoohAPI.Infrastructure.CompanyDB.Respositories
{
    public interface ICompanyRepository
    {
        IEnumerable<DBCompany> GetListCompanies(string query);
        IEnumerable<DBCompany> GetListCompanies(string query, Dictionary<string, object> parameters);
        DBCompany GetCompany(string query);
        DBCompany GetCompany(string query, Dictionary<string,object> parameters);
    }
}
