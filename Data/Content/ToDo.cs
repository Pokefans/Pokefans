// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// self explanatory
    /// </summary>
    public enum ToDoPriority { Normal, High, Low }

    /// <summary>
    /// self explanatory
    /// </summary>
    public enum ToDoStatus { Waiting, InProgress, Done, Cancelled }

    /// <summary>
    /// 
    /// </summary>
    [Table("todos")]
    public partial class ToDo
    {
        /// <summary>
        /// Unique Id for the ToDoObject
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ToDo Priority
        /// </summary>
        [DefaultValue(ToDoPriority.Normal)]
        public ToDoPriority Priority { get; set; }

        /// <summary>
        /// Id of the Author
        /// </summary>
        [Required]
        public int AuthorId { get; set; }

        /// <summary>
        /// Time the ToDo was submitted
        /// </summary>
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Time the ToDo was last edited
        /// </summary>
        public DateTime Edited { get; set; }

        /// <summary>
        /// Id of the User who is assigned to the ToDo
        /// </summary>
        public int? AssigneeId { get; set; }

        /// <summary>
        /// ToDo Title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Note for the ToDo
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// ToDo Status
        /// </summary>
        [DefaultValue(ToDoStatus.Waiting)]
        public ToDoStatus Status { get; set; }

        /// <summary>
        /// Author User
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        /// <summary>
        /// Assigned User
        /// </summary>
        [ForeignKey("AssigneeId")]
        public virtual User Assignee { get; set; }
    }
}
