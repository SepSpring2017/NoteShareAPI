using IronWebScraper;
using NoteShareAPI.Entities;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NoteShareAPI
{
    class SubjectScraper : WebScraper
    {
        private readonly NoteContext db;

        public SubjectScraper(NoteContext context)
        {
            db = context;
        }

        public override void Init()
        {
            Request("http://www.handbook.uts.edu.au/subjects/alpha.html", Parse);
        }

        public override void Parse(Response response)
        {
            var content = response.Css("#content > div.ie-images")[0];
            var clean = content.InnerHtml.Remove(0, 231);
            clean = Regex.Replace(clean, "<p>.+<\\/p>", "");
            clean = Regex.Replace(clean, "<a href.+\">", "").Replace("</a>", "").Replace("<br />", "").Trim();

            var subjects = clean.Split("\n");
            var subjectList = new List<Subject>();

            foreach (var s in subjects)
            {
                var parts = Regex.Split(s, "\\s+");
                string name = parts[0];

                for (int i = 1; i < parts.Length - 1; i++)
                    name += " " + parts[i];

                var subject = new Subject
                {
                    SubjectId = int.Parse(parts[parts.Length - 1]),
                    Name = name.Trim()
                };

                if (!subjectList.Contains(subject))
                    subjectList.Add(subject);
            }

            db.Subjects.AddRange(subjectList);
            db.SaveChanges();
        }
    }
}