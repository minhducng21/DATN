using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeExam.Constant
{
    public static class Status
    {
        public static int Active = 1;
        public static int Deactive = 0;
    }
    public static class TaskLevel
    {
        public static string Easy = "Easy";
        public static string Medium = "Medium";
        public static string Hard = "Hard";
    }

    public class Constant
    {
        public static int GetUserIdByIdentity(string username)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName.Equals(username));
                if (user == null)
                {
                    user = db.Users.FirstOrDefault(u => u.SocialId.Equals(username));
                    return user.UserId;
                }
                return user.UserId;
            }
        }
    }
}