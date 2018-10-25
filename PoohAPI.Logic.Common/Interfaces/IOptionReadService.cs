using PoohAPI.Models.OptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    interface IOptionReadService
    {
        IEnumerable<Major> GetAllMajors(int maxCount, int offset);
    }
}
