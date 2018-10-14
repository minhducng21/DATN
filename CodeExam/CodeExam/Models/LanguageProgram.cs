namespace CodeExam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LanguageProgram")]
    public partial class LanguageProgram
    {
        [Key]
        public int LanguageId { get; set; }

        [StringLength(50)]
        public string LanguageName { get; set; }

        public int? LanguageStatus { get; set; }
    }
}
