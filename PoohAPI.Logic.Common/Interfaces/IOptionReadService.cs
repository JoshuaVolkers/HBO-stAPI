using PoohAPI.Logic.Common.Models.OptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IOptionReadService
    {
        IEnumerable<Major> GetAllMajors(int maxCount, int offset);
        IEnumerable<EducationLevel> GetAllEducationLevels(int maxCount, int offset);
    }
}
