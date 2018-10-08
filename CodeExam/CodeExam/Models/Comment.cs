namespace CodeExam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int CommentId { get; set; }

        public int? UserId { get; set; }

        public string CommentDescription { get; set; }

        public DateTime? CommentTime { get; set; }

        public int? ParentId { get; set; }

        public int? CommentStatus { get; set; }
    }
}
