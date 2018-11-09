using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeExam.ViewModels
{
    public class AccountViewModel
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }

        [Display(Name = "Tên hiển thị")]
        public string DisplayName { get; set; }

        [Display(Name = "Nhớ mật khẩu")]
        public bool IsPersistent { get; set; }

        public int Active { get; set; }

        public string Email { get; set; }

        public string SocialId { get; set; }

        public RoleUser Role { get; set; }

        public int RoleId { get; set; }

        public RoleUser GetRole()
        {
            if (RoleId > 0)
            {
                CodeWarDbContext db = new CodeWarDbContext();
                return db.RoleUsers.Find(RoleId);
            }
            return null;
        }

        public AccountViewModel()
        {
            Role = GetRole();
        }
    }
}