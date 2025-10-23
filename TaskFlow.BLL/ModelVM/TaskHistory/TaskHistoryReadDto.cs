using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.BLL.ModelVM.AppUser;

namespace TaskFlow.BLL.ModelVM.TaskHistory
{
    public class TaskHistoryReadDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ChangedByUserId { get; set; }
        public int OldStatusId { get; set; }
        public int NewStatusId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeDescription { get; set; }
        public TaskFlow.DAL.Enums.Task.HistoryActionType ActionType { get; set; }

        // Helper properties
        public TaskFlow.DAL.Enums.Task.AppTaskStatus OldStatus { get; set; }
        public TaskFlow.DAL.Enums.Task.AppTaskStatus NewStatus { get; set; }
        public string ChangeSummary { get; set; }
        public bool IsSignificantChange { get; set; }

        // Navigation properties
        public AppUserReadDto ChangedByUser { get; set; }
    }
}
