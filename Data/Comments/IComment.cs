using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Comments
{
    public interface IComment
    {
        int AuthorId { get; set; }
        int CommentedObjectId { get; set; }
        DateTime SubmitTime { get; set; }
        short Level { get; set; }
        int? ParentCommentId { get; set; }

        bool HasChildren { get; }
    }
}
