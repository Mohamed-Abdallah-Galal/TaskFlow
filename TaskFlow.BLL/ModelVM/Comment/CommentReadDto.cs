using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.BLL.ModelVM.AppUser;

namespace TaskFlow.BLL.ModelVM.Comment
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool IsInternalNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShortMessage { get; set; }
        public bool IsLongMessage { get; set; }

        // Navigation properties
        public AppUserReadDto User { get; set; }
    }
}
