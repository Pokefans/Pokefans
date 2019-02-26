using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public enum BoardType { Board  = 0, Category = 1, Link = 2}

    public class Board
    {
        public Board() {
            Children = new List<Board>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        [Index]
        public string Url { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [DefaultValue(BoardType.Board)]
        public BoardType Type { get; set; }

        public int ThreadCount { get; set; }

        public int PostCount { get; set; }

        public int? ParentBoardId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Pokefans.Data.Forum.Board"/> is Question &amp; Answer enabled.
        /// If so, Users are able to mark a Post as the solution.
        /// </summary>
        /// <value><c>true</c> if is Q&amp;A enabled; otherwise, <c>false</c>.</value>
        public bool IsQAEnabled { get; set; }

        public bool ShowInParentBoard { get; set; }

        public int Order { get; set; }

        [NotMapped]
        public List<Board> Children { get; set; }

        public int? LastPostId { get; set; }

        [ForeignKey("LastPostId")]
        public Post LastPost { get; set; }

        public string Rules { get; set; }
    }
}
