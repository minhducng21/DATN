using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeExam.ViewModels
{
    public class GoogleAccViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public string ObjectType { get; set; }
        public List<Email> Emails { get; set; }
    }

    public class Email
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}