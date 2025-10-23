using AutoMapper;
using TaskFlow.BLL.ModelVM.TaskHistory;
using TaskFlow.DAL.Entites;

namespace TaskFlow.BLL.Mapper.TaskHistoryMapping
{
    public class TaskHistoryProfile : Profile
    {
        public TaskHistoryProfile()
        {
            // Read mapping
            CreateMap<TaskHistory, TaskHistoryReadDto>()
                .ForMember(m => m.ChangedByUser, o => o.MapFrom(th => th.ChangedByUser))
                .ForMember(m => m.OldStatus, o => o.MapFrom(th => (TaskFlow.DAL.Enums.Task.AppTaskStatus)th.OldStatusId))
                .ForMember(m => m.NewStatus, o => o.MapFrom(th => (TaskFlow.DAL.Enums.Task.AppTaskStatus)th.NewStatusId))
                .ForMember(m => m.ChangeSummary, o => o.MapFrom(th => th.ChangeSummary))
                .ForMember(m => m.IsSignificantChange, o => o.MapFrom(th => th.IsSignificantChange));
        }
    }
}