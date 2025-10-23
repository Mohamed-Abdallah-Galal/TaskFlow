using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.BLL.ModelVM.Attachment
{
    public class AttachmentCreateDto
    {
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public TaskFlow.DAL.Enums.Attachment.AttachmentType FileType { get; set; }
        public long FileSize { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
