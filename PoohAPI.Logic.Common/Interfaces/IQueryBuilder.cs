using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IQueryBuilder
    {
        void SetFrom(string from);
        void SetLimit(string limit);
        void SetOffset(string offset);
        void AddSelect(string select);
        void AddJoinLine(string join);
        void AddWhere(string where);
        void AddGroupBy(string groupBy);
        void AddHaving(string having);
        string BuildQuery();
        void Clear();
    }
}
