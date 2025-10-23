using AutoMapper;
using TaskFlow.BLL.ModelVM.Attachment;
using TaskFlow.DAL.Entites;

namespace TaskFlow.BLL.Mapper.AttachmentMapping
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            // Read mapping
            CreateMap<Attachment, AttachmentReadDto>()
                .ForMember(m => m.FileSizeDisplay, o => o.MapFrom(a => a.FileSizeDisplay))
                .ForMember(m => m.FileIcon, o => o.MapFrom(a => a.FileIcon));

            // Create mapping using constructor
            CreateMap<AttachmentCreateDto, Attachment>()
                .ConstructUsing(dto => new Attachment(
                    dto.TaskId,
                    dto.FileName,
                    dto.FilePath,
                    dto.ContentType,
                    dto.FileType,
                    dto.FileSize,
                    dto.CreatedByUserId
                ));

            // Update mapping with ReverseMap
            CreateMap<AttachmentUpdateDto, Attachment>().ReverseMap();
        }
    }
}