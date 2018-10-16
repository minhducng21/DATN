namespace CodeExam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        [StringLength(20)]
        public string TaskLevel { get; set; }

        public int? Point { get; set; }

        public string Input { get; set; }

        public int? OutputType { get; set; }

        public int TestCaseId { get; set; }

        public int? TaskStatus { get; set; }
    }
}
