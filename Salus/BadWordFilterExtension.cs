using System.Text.RegularExpressions;

namespace Salus
{
    public class BadWordFilterExtension
    {

        List<string> badWordList = new();
        public BadWordFilterExtension()
        {
            new StreamReader("./Templates/badwords.txt").ReadToEnd().Split("\n").ToList().ForEach(b => badWordList.Add(b.Trim()));
        }

        public bool CheckStringContainsBadWords(string data)
        {
            bool contains = false;
            int count = 0;
            Regex r;
            foreach (var word in badWordList)
            {
                var expword = ExpandBadWordToIncludeIntentionalMisspellings(word);
                r = new Regex(@"(?<Pre>\s+)(?<Word>" + expword + @")(?<Post>\s+|\!\?|\.)");
                var matches = r.Matches(data);
                foreach (Match match in matches)
                {
                    count++;
                }
            }
            return contains;
        }

        //src: https://stackoverflow.com/questions/18324054/filtering-bad-words-and-all-permutations-of-intentionally-misspelled-words

        public string ExpandBadWordToIncludeIntentionalMisspellings(string word)
        {
            var chars = word
                .ToCharArray();

            var op = "[" + string.Join("][", chars) + "]";

            return op
                .Replace("[a]", "[a A @ á Á ä]")
                .Replace("[b]", "[b B I3 l3 i3 ß]")
                .Replace("[c]", "(?:[c C \\(]|[k K])")
                .Replace("[d]", "[d D]")
                .Replace("[e]", "[e E 3 é É]")
                .Replace("[f]", "(?:[f F]|[ph pH Ph PH])")
                .Replace("[g]", "[g G 6]")
                .Replace("[h]", "[h H]")
                .Replace("[i]", "[i I l ! 1 Í í j J]")
                .Replace("[j]", "[j J]")
                .Replace("[k]", "(?:[c C \\(]|[k K])")
                .Replace("[l]", "[l L 1 ! i I]")
                .Replace("[m]", "[m M]")
                .Replace("[n]", "[n N]")
                .Replace("[o]", "[o O 0 ó Ó ö Ö ő Ő]")//todo: folytatni magyar betűkkel
                .Replace("[p]", "[p P]")
                .Replace("[q]", "[q Q 9]")
                .Replace("[r]", "[r R]")
                .Replace("[s]", "[s S $ 5]")
                .Replace("[t]", "[t T 7]")
                .Replace("[u]", "[u U v V ú Ú ü Ü ű Ű]")
                .Replace("[v]", "[v V u U]")
                .Replace("[w]", "[w W vv VV]")
                .Replace("[x]", "[x X]")
                .Replace("[y]", "[y Y]")
                .Replace("[z]", "[z Z 2]")
                ;
        }
    }
}
