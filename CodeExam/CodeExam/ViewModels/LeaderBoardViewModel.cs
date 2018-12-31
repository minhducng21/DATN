using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeExam.ViewModels
{
    public class LeaderBoardViewModel
    {
        
        public int LeaderBoardId { get; set; }

        public int UserId { get; set; }

        public int TaskId { get; set; }

        public int Point { get; set; }

        public string SourceCode { get; set; }

        public int LanguageId { get; set; }

        public User User { get; set; }

        public Task Task { get; set; }

        public int TotalPoint { get; set; }

        public LanguageProgram Language { get; set; }

        public LeaderBoardViewModel()
        {
            TotalPoint = 0;
        }
    }
}