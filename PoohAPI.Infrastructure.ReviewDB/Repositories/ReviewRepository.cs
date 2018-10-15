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

        public IEnumerable<DBReview> GetAllReviews(int maxCount, int offset)
        {
            return GetAll<DBReview>("test");
        }

        public DBReview GetReview(string q)
        {
            return GetSingle<DBReview>(q);
        }

        public DBReview GetReview(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<DBReview>(query, parameters);
        }
    }
}
