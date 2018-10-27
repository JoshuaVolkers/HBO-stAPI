using AutoMapper;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.ReviewDB.Models;
using System.Collections.Generic;

namespace PoohAPI.Infrastructure.ReviewDB.Repositories
{
    public class ReviewRepository : MySQLBaseRepository, IReviewRepository
    {
        private IMapper _mapper;

        public ReviewRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }

        public void DeleteReview(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public IEnumerable<DBReview> GetAllReviews(int maxCount, int offset)
        {
            return GetAll<DBReview>("test");
        }

        public DBReview GetReview(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<DBReview>(query, parameters);
        }

        public void UpdateReview(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public void PostReview(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public IEnumerable<DBReviewId> GetListReviewIds(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBReviewId>(query, parameters);
        }

        public IEnumerable<DBReview> GetListReviews(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBReview>(query, parameters);
        }
    }
}
