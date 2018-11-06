using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Data;

namespace PoohAPI.Logic.Reviews.Init
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<DBReview, Common.Models.Review>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.review_id))
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.review_bedrijf_id))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.review_student_id))
                .ForMember(d => d.Stars, o => o.MapFrom(s => s.review_sterren))
                .ForMember(d => d.WrittenReview, o => o.MapFrom(s => s.review_geschreven))
                .ForMember(d => d.Anonymous, o => o.MapFrom(s => s.review_anoniem))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.review_datum))
                .ForMember(d => d.VerifiedReview, o => o.MapFrom(s => s.review_status))
                .ForMember(d => d.VerifiedBy, o => o.MapFrom(s => s.review_status_bevestigd_door))
                //.ForMember(d => d.EmploymentContractPDF, o => o.MapFrom(s => s.blob))
                .ReverseMap();

            CreateMap<IDataReader, DBReview>().ConvertUsing<DataReaderTypeConverter<DBReview>>();

            CreateMap<IDataReader, DBReviewId>().ConvertUsing<DataReaderTypeConverter<DBReviewId>>();

            CreateMap<DBReviewId, int>()
                .ConvertUsing(s => s.review_id);

            CreateMap<DBReview, ReviewPublic>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.review_id))
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.review_bedrijf_id))
                .ForMember(d => d.NameReviewer, o => o.MapFrom(s => s.review_student_name))
                .ForMember(d => d.Stars, o => o.MapFrom(s => s.review_sterren))
                .ForMember(d => d.WrittenReview, o => o.MapFrom(s => s.review_geschreven))
                .ForMember(d =>d.ReviewDate, o => o.MapFrom(s => s.review_datum))
                .ReverseMap();
        }
    }
}
