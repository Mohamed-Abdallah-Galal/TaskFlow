

namespace TaskFlow.BLL.ModelVM.Comment
{
    public class CommentCreateDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsInternalNote { get; set; }

    }
}
