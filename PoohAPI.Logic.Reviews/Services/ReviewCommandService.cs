using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;

namespace PoohAPI.Logic.Reviews.Services
{
    public class ReviewCommandService : IReviewCommandService
    {
        private readonly IReviewRepository _ReviewRepository;
        private readonly IMapper _mapper;

        public ReviewCommandService(IReviewRepository ReviewRepository, IMapper mapper)
        {
            _ReviewRepository = ReviewRepository;
            _mapper = mapper;
        }
    }
}
