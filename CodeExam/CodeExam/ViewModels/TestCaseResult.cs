using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeExam.ViewModels
{
    public class TestCaseResult
    {
        public bool CompareExpection { get; set; }
        public string Result { get; set; }
    }
    public class RunResult
    {
        public RunResult()
        {
            detail = new List<TestCaseResult>();
            isSuccess = true;
        }
        public bool isSuccess { get; set; }

        public string errMsg { get; set; }

        public int successTestCase { get; set; }

        public int totalTestCase { get; set; }

        public int successPoint { get; set; }

        public int totalPoint { get; set; }

        public int totalHiddenTestCase { get; set; }

        public int successHiddenTestCase { get; set; }

        public int totalShowTestCase { get; set; }

        public int successShowTestCase { get; set; }

        public List<TestCaseResult> detail { get; set; }
    }
}