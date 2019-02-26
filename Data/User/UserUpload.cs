// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    public class UserUpload
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public FileStatus Status { get; set; }

        public bool IsLocked { get; set; }

        public bool IsCopyrightViolation { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        [MaxLength(68)]
        public string Hash { get; set; }

        [MaxLength(100)]
        public string OriginalFileName { get; set; }

        public bool IsImage { get; set; }

        [MaxLength(10)]
        public string FileExtension { get; set; }

        [MaxLength(255)]
        public string MimeType { get; set; }

        public int FileSize { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        [MaxLength(39)]
        public string UploadIp { get; set; }

        public DateTime UploadTime { get; set; }
    }

    public enum FileStatus
    {
        Hide = -1,
        Default = 0,
        Feature = 1
    }
}


