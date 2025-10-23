using AutoMapper;
using TaskFlow.BLL.ModelVM.TaskItem;
using TaskFlow.DAL.Entites;

namespace TaskFlow.BLL.Mapper.TaskMapping
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            // List mapping
            CreateMap<TaskItem, TaskItemReadDto>()
                .ForMember(m => m.AssignedTo, o => o.MapFrom(t => t.AssignedTo))
                .ForMember(m => m.CreatedBy, o => o.MapFrom(t => t.CreatedBy))
                .ForMember(m => m.IsOverdue, o => o.MapFrom(t => t.IsOverdue))
                .ForMember(m => m.IsUrgent, o => o.MapFrom(t => t.IsUrgent))
                .ForMember(m => m.DaysRemaining, o => o.MapFrom(t => t.DaysRemaining));

            // Create mapping using constructor
            CreateMap<TaskItemCreateDto, TaskItem>()
                .ConstructUsing(dto => new TaskItem(
                    dto.Title,
                    dto.Description,
                    dto.CreatedById,
                    dto.AssignedToId,
                    dto.Priority,
                    dto.Category,
                    dto.Deadline,
                    dto.EstimatedHours
                ));

            // Update mapping with ReverseMap
            CreateMap<TaskItemUpdateDto, TaskItem>().ReverseMap();

            // Progress update mapping
            CreateMap<TaskItemProgressUpdateDto, TaskItem>().ReverseMap();

            // Status update mapping  
            CreateMap<TaskItemStatusUpdateDto, TaskItem>().ReverseMap();
        }
    }
}