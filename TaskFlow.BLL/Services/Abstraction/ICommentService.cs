using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.BLL.ModelVM.Comment;

namespace TaskFlow.BLL.Services.Abstraction
{
    public interface ICommentService
    {
        Task<CommentReadDto> GetCommentByIdAsync(int id);
        Task<List<CommentReadDto>> GetCommentsByTaskAsync(int taskId);
        Task<List<CommentReadDto>> GetCommentsByUserAsync(int userId);
        Task<List<CommentReadDto>> GetRecentCommentsAsync(int count = 10);

        Task<int> AddCommentAsync(CommentCreateDto dto);
        Task UpdateCommentAsync(CommentUpdateDto dto);
        Task DeleteCommentAsync(int commentId, int deletedByUserId);
        Task MarkCommentAsInternalAsync(int commentId, int updatedByUserId);
        Task MarkCommentAsPublicAsync(int commentId, int updatedByUserId);

        Task<int> GetCommentCountForTaskAsync(int taskId);
        Task<int> GetCommentCountForUserAsync(int userId);

    }
}
