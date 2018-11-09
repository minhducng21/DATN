using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeExam.Models
{
    public class ControllerAction
    {
        [Key]
        public int CtrlId { get; set; }

        public string Ctrl { get; set; }

        public string Action { get; set; }

        public string Area { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }
    }
}