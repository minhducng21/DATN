namespace CodeExam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LeaderBoard")]
    public partial class LeaderBoard
    {
        public int LeaderBoardId { get; set; }

        public int UserId { get; set; }

        public int TaskId { get; set; }

        public int Point { get; set; }

        public string SourceCode { get; set; }

        public int LanguageId { get; set; }
    }
}
