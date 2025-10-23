using AutoMapper;
using TaskFlow.BLL.ModelVM.Comment;
using TaskFlow.DAL.Entites;

namespace TaskFlow.BLL.Mapper.CommentMapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // Read mapping
            CreateMap<Comment, CommentReadDto>()
                .ForMember(m => m.User, o => o.MapFrom(c => c.User))
                .ForMember(m => m.ShortMessage, o => o.MapFrom(c => c.ShortMessage))
                .ForMember(m => m.IsLongMessage, o => o.MapFrom(c => c.IsLongMessage));

            // Create mapping using constructor
            CreateMap<CommentCreateDto, Comment>()
                .ConstructUsing(dto => new Comment(
                    dto.TaskId,
                    dto.UserId,
                    dto.Message,
                    dto.CreatedByUserId,
                    dto.IsInternalNote
                ));

            // Update mapping with ReverseMap
            CreateMap<CommentUpdateDto, Comment>().ReverseMap();
        }
    }
}