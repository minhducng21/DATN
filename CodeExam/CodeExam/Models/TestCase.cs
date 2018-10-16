namespace CodeExam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestCase")]
    public partial class TestCase
    {
        public int TestCaseId { get; set; }

        public string Input { get; set; }

        public string Output { get; set; }
    }
}
