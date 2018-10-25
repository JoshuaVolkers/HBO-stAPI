using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IVacancyCommandService
    {
        void AddFavourite(int userid, int vacancyid);
        void DeleteFavourite(int userid, int vacancyid);
    }
}
