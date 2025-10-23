using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.BLL.ModelVM.Comment
{
    public class CommentUpdateDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UpdatedByUserId { get; set; }
        public bool? IsInternalNote { get; set; }
    }
}
