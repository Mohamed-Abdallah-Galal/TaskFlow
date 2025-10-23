using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.BLL.ModelVM.Attachment
{
    public class AttachmentUpdateDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int UpdatedByUserId { get; set; }
    }
}
