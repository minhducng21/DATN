using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeExam.ViewModels
{
    public class ControllerViewModel
    {
        public int CtrlId { get; set; }
        public string Ctrl { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        //public string RoleName { get; set; }
        public bool IsChecked { get; set; }

        public ControllerAction GetCtrl()
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                if (this.CtrlId != 0)
                {
                    return db.ControllerActions.Find(CtrlId);
                }
                return null;
            }
        }
    }

    public class RoleControllerViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ControllerViewModel> ControllerViewModels { get; set; }
    }
}