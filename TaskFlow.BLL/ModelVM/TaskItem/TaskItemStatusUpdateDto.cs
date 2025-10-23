

namespace TaskFlow.BLL.ModelVM.TaskItem
{
    public class TaskItemStatusUpdateDto
    {
        public int Id { get; set; }
        public TaskFlow.DAL.Enums.Task.AppTaskStatus NewStatus { get; set; }
        public int UpdatedByUserId { get; set; }
        public string ChangeDescription { get; set; }
    }
}
