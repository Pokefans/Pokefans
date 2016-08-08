using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Util.Comments
{
    class CommentException : Exception
    {

        public CommentException(string message) : base(message)
        {

        }

        public CommentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
