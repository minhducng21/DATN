namespace CodeExam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DataType")]
    public partial class DataType
    {
        public int DataTypeId { get; set; }

        [StringLength(50)]
        public string DataTypeName { get; set; }

        public int? DataTypeStatus { get; set; }
    }
}
