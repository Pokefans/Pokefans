using Pokefans.Data.Comments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Pokefans.Data.ViewModels
{
    public class CommentViewModel : IComment
    {
        public CommentViewModel()
        {
            Children = new List<CommentViewModel>();
        }

        public CommentViewModel(Comment c)
        {
            Context = c.Context;
            CommentedObjectId = c.CommentedObjectId;
            ParentCommentId = c.ParentCommentId;
            CommentId = c.Id;
            Author = c.Author.UserName;
            AuthorUserUrl = c.Author.Url;
            AuthorId = c.AuthorId;
            AuthorColor = c.Author.Color;
            AvatarFileName = c.Author.MiniAvatarFileName;
            DisplayPublic = c.DisplayPublic;
            Text = c.ParsedComment;
            SubmitTime = c.SubmitTime;
            Level = c.Level;

            Children = new List<CommentViewModel>();
            foreach (var child in c.Children)
            {
                Children.Add(new CommentViewModel(child));
            }
        }

        public int Context { get; set; }
        public int CommentedObjectId { get; set; }
        public int? ParentCommentId { get; set; }
        public int CommentId { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public string AvatarFileName { get; set; }
        public bool GravatarEnabled { get; set; }
        [JsonIgnore]
        protected string Email { get; set; }

        public string AuthorUserUrl { get; set; }
        public string AuthorColor { get; set; }

        public string AvatarUrl 
        {
			get
			{
				if (GravatarEnabled)
				{
					byte[] bytemail = new UTF8Encoding().GetBytes(Email);
					byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytemail);
					return "https://www.gravatar.com/avatar/" + BitConverter.ToString(hash).Replace("-", "").ToLower() + "?d=identicon&s=40";
				}
				else
				{
					return "//files." + ConfigurationManager.AppSettings["Domain"] + "/user/avatare/" + AvatarFileName;
				}
			}
        }
        public bool DisplayPublic { get; set; }

        private string rawText { get; set; }
        public string Text
        {
            get { return DisplayPublic ? rawText : "<p class=\"help-block\">Der Inhalt dieses Kommentars wurde von der Moderation versteckt.</p>"; }
            set { rawText = value; }
        }
        public DateTime SubmitTime { get; set; }
        public string SubmitTimeString
        {
            get
            {
                return SubmitTime.ToString("d.M.yyyy H:m");
            }
        }
        public bool IsDeletable { get; set; }
        public short Level { get; set; }

        public List<CommentViewModel> Children { get; set; }

        public bool HasChildren
        {
            get { return Children.Count > 0; }
        }
    }
}
