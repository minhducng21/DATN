using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeExam.Models
{
    public class RoleController
    {
        public int ID { get; set; }
        public int RoleId { get; set; }
        public int CtrlId { get; set; }
        public string Description { get; set; }
    }
}