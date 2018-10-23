using PoohAPI.Logic.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Classes
{
    /// <summary>
    /// This class builds SQL queries. It doesn't escape parameters so add placeholders instead and add the
    /// user input parameters at the sql command with AddWithValue.
    /// </summary>
    public class QueryBuilder : IQueryBuilder
    {
        private string from;
        private string limit;
        private string offset;
        private string update;
        private List<string> selectEntries;
        private List<string> joinEntries;
        private List<string> whereEntries;
        private List<string> groupByEntries;
        private List<string> havingEntries;
        private List<string> updateSetEntries;

        public QueryBuilder()
        {
            this.Init();
        }

        private void Init()
        {
            this.from = "";
            this.limit = "";
            this.offset = "";
            this.update = "";
            this.selectEntries = new List<string>();
            this.joinEntries = new List<string>();
            this.whereEntries = new List<string>();
            this.groupByEntries = new List<string>();
            this.havingEntries = new List<string>();
            this.updateSetEntries = new List<string>();
        }
        
        public void AddGroupBy(string groupBy)
        {
            this.groupByEntries.Add(groupBy);
        }

        public void AddHaving(string having)
        {
            this.havingEntries.Add(having);
        }

        public void AddJoinLine(string join)
        {
            this.joinEntries.Add(join);
        }

        public void AddSelect(string select)
        {
            this.selectEntries.Add(select);
        }

        public void AddWhere(string where)
        {
            this.whereEntries.Add(where);
        }
        public void Clear()
        {
            this.Init();
        }

        public void SetFrom(string from)
        {
            this.from = from;
        }

        public void SetLimit(string limit)
        {
            this.limit = limit;
        }

        public void SetOffset(string offset)
        {
            this.offset = offset;
        }

        public string BuildQuery()
        {
            string query = "";

            query = this.BuildSelect(query);
            query += "FROM " + this.from + " ";
            query = this.BuildJoin(query);
            query = this.BuildWhere(query);
            query = this.BuildGroupBy(query);
            query = this.BuildHaving(query);
            
            if (this.limit != "")
            {
                query += "LIMIT " + this.limit + " ";
            }

            if (this.offset != "")
            {
                query += "OFFSET " + this.offset + " ";
            }

            this.Clear();

            return query;
        }

        private string BuildHaving(string query)
        {
            if (this.havingEntries.Count > 0)
            {
                query += "HAVING ";

                for (int i = 0; i < this.havingEntries.Count; i++)
                {
                    query += this.havingEntries[i] + " ";

                    if (i != (this.havingEntries.Count - 1))
                    {
                        query += "AND ";
                    }
                }
            }

            return query;
        }

        private string BuildGroupBy(string query)
        {
            if (this.groupByEntries.Count > 0)
            {
                query += "GROUP BY ";

                for (int i = 0; i < this.groupByEntries.Count; i++)
                {
                    query += this.groupByEntries[i];

                    if (i != (this.groupByEntries.Count - 1))
                    {
                        query += ", ";
                    }
                    else
                    {
                        query += " ";
                    }
                }
            }

            return query;
        }

        private string BuildWhere(string query)
        {
            if (this.whereEntries.Count > 0)
            {
                query += "WHERE ";

                for (int i = 0; i < this.whereEntries.Count; i++)
                {
                    query += this.whereEntries[i] + " ";

                    if (i != (this.whereEntries.Count - 1))
                    {
                        query += "AND ";
                    }
                }
            }

            return query;
        }

        private string BuildJoin(string query)
        {
            if (this.joinEntries.Count > 0)
            {
                foreach (string join in this.joinEntries)
                {
                    query += join + " ";
                }
            }

            return query;
        }

        private string BuildSelect(string query)
        {
            if (this.selectEntries.Count > 0)
            {
                query += "SELECT ";

                for (int i = 0; i < this.selectEntries.Count; i++)
                {
                    query += this.selectEntries[i];

                    if (i != (this.selectEntries.Count - 1))
                    {
                        query += ", ";
                    }
                    else
                    {
                        query += " ";
                    }
                }
            }

            return query;
        }

        public void SetUpdate(string update)
        {
            this.update = update;
        }

        public string BuildUpdate()
        {
            string query = "UPDATE " + this.update + " ";

            query = this.BuildUpdateSet(query);
            query = this.BuildWhere(query);

            this.Clear();

            return query;
        }

        private string BuildUpdateSet(string query)
        {
            if (this.updateSetEntries.Count > 0)
            {
                query += "SET ";

                for (int i = 0; i < this.updateSetEntries.Count; i++)
                {
                    query += this.updateSetEntries[i];

                    if (i != (this.updateSetEntries.Count - 1))
                    {
                        query += ", ";
                    }
                    else
                    {
                        query += " ";
                    }
                }
            }

            return query;
        }

        public void AddUpdateSet(string updateSet)
        {
            this.updateSetEntries.Add(updateSet);
        }
    }
}
