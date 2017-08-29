using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoteShareAPI
{
    public static class Services
    {
        public static string GetUniqueSlug(this string srcString, List<string> allItems)
        {
            var regex = new Regex(@"[^a-zA-Z 0-9\.\-]+");

            var slug = regex.Replace(srcString.ToLower(), string.Empty);

            slug = ReplaceMatches(slug, @"[ ]{2,}").Replace(" ", "-");
            slug = ReplaceMatches(slug, @"[\.]{2,}").Replace(".", "-");
            slug = ReplaceMatches(slug, @"[\-]{2,}");

            if (slug.StartsWith("-") && !slug.EndsWith("-"))
                slug = string.Format("0{0}", slug);

            if (!slug.StartsWith("-") && slug.EndsWith("-"))
                slug = string.Format("{0}0", slug);

            return allItems.Any(s => s == slug) ? GetUniqueSlugInternal(slug, allItems) : slug;
        }

        private static string GetUniqueSlugInternal(string srcString, List<string> srcList)
        {
            var slugRegex = new Regex(string.Format(@"^{0}-([0-9]+)$", srcString));
            var matchingSlugs = new List<int>();
            srcList.ForEach(s =>
            {
                var match = slugRegex.Match(s);
                if (match.Success)
                {
                    var number = int.Parse(match.Groups[1].Captures[0].Value);
                    matchingSlugs.Add(number);
                }
            });
            if (matchingSlugs.Any())
            {
                var max = matchingSlugs.Max();
                return string.Format("{0}-{1}", srcString, max + 1);
            }
            return string.Format("{0}-2", srcString);
        }

        private static string ReplaceMatches(string srcString, string pattern)
        {
            var removalPattern = new Regex(pattern);
            return removalPattern.Replace(srcString, "-");
        }
    }
}