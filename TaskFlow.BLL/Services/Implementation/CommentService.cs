using AutoMapper;
using TaskFlow.BLL.ModelVM.Comment;
using TaskFlow.BLL.Services.Abstraction;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.BLL.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo _commentRepo;
        private readonly ITaskItemRepo _taskRepo;
        private readonly IAppUserRepo _userRepo;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepo commentRepo, ITaskItemRepo taskRepo, IAppUserRepo userRepo, IMapper mapper)
        {
            _commentRepo = commentRepo;
            _taskRepo = taskRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<CommentReadDto> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            return _mapper.Map<CommentReadDto>(comment);
        }

        public async Task<List<CommentReadDto>> GetCommentsByTaskAsync(int taskId)
        {
            var comments = await _commentRepo.GetCommentsByTaskAsync(taskId);
            return _mapper.Map<List<CommentReadDto>>(comments);
        }

        public async Task<List<CommentReadDto>> GetCommentsByUserAsync(int userId)
        {
            var comments = await _commentRepo.GetCommentsByUserAsync(userId);
            return _mapper.Map<List<CommentReadDto>>(comments);
        }

        public async Task<List<CommentReadDto>> GetRecentCommentsAsync(int count = 10)
        {
            var comments = await _commentRepo.GetRecentCommentsAsync(count);
            return _mapper.Map<List<CommentReadDto>>(comments);
        }

        public async Task<int> AddCommentAsync(CommentCreateDto dto)
        {
            // Validate task exists
            var task = await _taskRepo.GetByIdAsync(dto.TaskId);
            if (task == null)
                throw new ArgumentException($"Task with ID {dto.TaskId} not found.");

            // Validate user exists
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new ArgumentException($"User with ID {dto.UserId} not found.");

            // Validate message
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ArgumentException("Comment message cannot be empty.");

            var comment = _mapper.Map<Comment>(dto);
            await _commentRepo.AddAsync(comment);
            await _commentRepo.SaveChangesAsync();

            return comment.Id;
        }

        public async Task UpdateCommentAsync(CommentUpdateDto dto)
        {
            var comment = await _commentRepo.GetByIdAsync(dto.Id);
            if (comment == null)
                throw new ArgumentException($"Comment with ID {dto.Id} not found.");

            // Validate message
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new ArgumentException("Comment message cannot be empty.");

            _mapper.Map(dto, comment);
            _commentRepo.Update(comment);
            await _commentRepo.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId, int deletedByUserId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment == null)
                throw new ArgumentException($"Comment with ID {commentId} not found.");

            await _commentRepo.SoftDeleteAsync(commentId, deletedByUserId);
            await _commentRepo.SaveChangesAsync();
        }

        public async Task MarkCommentAsInternalAsync(int commentId, int updatedByUserId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment == null)
                throw new ArgumentException($"Comment with ID {commentId} not found.");

            comment.MarkAsInternal(updatedByUserId);
            _commentRepo.Update(comment);
            await _commentRepo.SaveChangesAsync();
        }

        public async Task MarkCommentAsPublicAsync(int commentId, int updatedByUserId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment == null)
                throw new ArgumentException($"Comment with ID {commentId} not found.");

            comment.MarkAsPublic(updatedByUserId);
            _commentRepo.Update(comment);
            await _commentRepo.SaveChangesAsync();
        }

        public async Task<int> GetCommentCountForTaskAsync(int taskId)
        {
            return await _commentRepo.GetCommentCountByTaskAsync(taskId);
        }

        public async Task<int> GetCommentCountForUserAsync(int userId)
        {
            var comments = await _commentRepo.GetCommentsByUserAsync(userId);
            return comments.Count();
        }
    }
}