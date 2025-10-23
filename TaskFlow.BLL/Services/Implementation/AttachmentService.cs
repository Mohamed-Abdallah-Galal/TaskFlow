using AutoMapper;
using TaskFlow.BLL.ModelVM.Attachment;
using TaskFlow.BLL.Services.Abstraction;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.BLL.Services.Implementation
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepo _attachmentRepo;
        private readonly ITaskItemRepo _taskRepo;
        private readonly IMapper _mapper;

        public AttachmentService(IAttachmentRepo attachmentRepo, ITaskItemRepo taskRepo, IMapper mapper)
        {
            _attachmentRepo = attachmentRepo;
            _taskRepo = taskRepo;
            _mapper = mapper;
        }

        public async Task<AttachmentReadDto> GetAttachmentByIdAsync(int id)
        {
            var attachment = await _attachmentRepo.GetByIdAsync(id);
            return _mapper.Map<AttachmentReadDto>(attachment);
        }

        public async Task<List<AttachmentReadDto>> GetAttachmentsByTaskAsync(int taskId)
        {
            var attachments = await _attachmentRepo.GetAttachmentsByTaskAsync(taskId);
            return _mapper.Map<List<AttachmentReadDto>>(attachments);
        }

        public async Task<List<AttachmentReadDto>> GetAttachmentsByTypeAsync(TaskFlow.DAL.Enums.Attachment.AttachmentType fileType)
        {
            var attachments = await _attachmentRepo.GetAttachmentsByTypeAsync(fileType);
            return _mapper.Map<List<AttachmentReadDto>>(attachments);
        }

        public async Task<int> AddAttachmentAsync(AttachmentCreateDto dto)
        {
            // Validate task exists
            var task = await _taskRepo.GetByIdAsync(dto.TaskId);
            if (task == null)
                throw new ArgumentException($"Task with ID {dto.TaskId} not found.");

            // Validate file size
            if (!await IsFileSizeWithinLimitAsync(dto.FileSize))
                throw new InvalidOperationException("File size exceeds the allowed limit (100MB).");

            // Validate filename uniqueness
            if (await _attachmentRepo.FileNameExistsAsync(dto.FileName))
                throw new InvalidOperationException($"File name '{dto.FileName}' already exists.");

            var attachment = _mapper.Map<Attachment>(dto);
            await _attachmentRepo.AddAsync(attachment);
            await _attachmentRepo.SaveChangesAsync();

            return attachment.Id;
        }



        public async Task UpdateAttachmentAsync(AttachmentUpdateDto dto)
        {
            var attachment = await _attachmentRepo.GetByIdAsync(dto.Id);
            if (attachment == null)
                throw new ArgumentException($"Attachment with ID {dto.Id} not found.");

            // Validate filename uniqueness (excluding current attachment)
            if (await _attachmentRepo.FileNameExistsAsync(dto.FileName, dto.Id))
                throw new InvalidOperationException($"File name '{dto.FileName}' already exists.");

            _mapper.Map(dto, attachment);
            _attachmentRepo.Update(attachment);
            await _attachmentRepo.SaveChangesAsync();
        }

        public async Task DeleteAttachmentAsync(int attachmentId, int deletedByUserId)
        {
            var attachment = await _attachmentRepo.GetByIdAsync(attachmentId);
            if (attachment == null)
                throw new ArgumentException($"Attachment with ID {attachmentId} not found.");

            await _attachmentRepo.SoftDeleteAsync(attachmentId, deletedByUserId);
            await _attachmentRepo.SaveChangesAsync();
        }

        public async Task<int> GetAttachmentCountForTaskAsync(int taskId)
        {
            return await _attachmentRepo.GetAttachmentCountByTaskAsync(taskId);
        }

        public async Task<long> GetTotalStorageUsedAsync()
        {
            return await _attachmentRepo.GetTotalStorageUsedAsync();
        }

        public async Task<bool> IsFileSizeWithinLimitAsync(long fileSize)
        {
            const long maxFileSize = 100 * 1024 * 1024; // 100MB
            return fileSize <= maxFileSize;
        }

        public async Task<List<AttachmentReadDto>> GetLargeAttachmentsAsync(long minSize)
        {
            var attachments = await _attachmentRepo.GetLargeAttachmentsAsync(minSize);
            return _mapper.Map<List<AttachmentReadDto>>(attachments);
        }
    }
}