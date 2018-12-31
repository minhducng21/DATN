using CodeExam.Models;
using CodeExam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Controllers
{
    public class LeaderController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();
        // GET: Leader
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLanguage()
        {
            return Json(db.LanguagePrograms.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaderBoardByLanguageId(int id, int page, int PageSize)
        {
            var listLeaders = db.LeaderBoards.Where(d => d.LanguageId.Equals(id)).GroupBy(d => d.UserId);

            List < LeaderBoardViewModel > leaderBoardViewModels = new List<LeaderBoardViewModel>();

            foreach (var itemLeader in listLeaders)
            {
                LeaderBoardViewModel leader = new LeaderBoardViewModel();
                leader.User = db.Users.FirstOrDefault(u => u.UserId.Equals(itemLeader.Key));
                foreach (var point in itemLeader)
                {
                    leader.TotalPoint += point.Point;
                }

                leaderBoardViewModels.Add(leader);
            }

            var results = leaderBoardViewModels.OrderByDescending(p => p.TotalPoint)
                                               .Skip((page - 1) * PageSize)
                                               .Take(PageSize);
            var count = leaderBoardViewModels.Count();
            return Json(new { results, count }, JsonRequestBehavior.AllowGet);
        }
    }
}