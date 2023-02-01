using Newtonsoft.Json;

namespace Salus
{
    class Word
    {
        public string word { get; set; } = string.Empty;
    }
    public class BadWordFilterExtension
    {
        public List<string> badWordList = new();
        public string json = string.Empty;
        public BadWordFilterExtension()
        {
            string json = File.ReadAllText("./Templates/badwords.json");
            JsonConvert
                .DeserializeObject<List<Word>>(json)?
                .ForEach
                (
                    b => GetVariationsOfWord(b.word)
                        .ForEach
                        (
                            w => badWordList.Add(w)
                        )
                );
        }

        public bool CheckStringContainsBadWords(string data)
        {
            var clearWord = string.Empty;
            List<char> separators = new List<char>();
            for (char i = ' '; i < '@'; i++)
                separators.Add(i);

            for (int j = 0; j < data.Length; j++)
                if (!separators.Contains(data[j]))
                    clearWord += data[j];

            if (badWordList.Contains(clearWord.ToLower()))
                return true;
            foreach (var badWord in badWordList)
                if (clearWord.ToLower().Contains(badWord))
                    return true;

            return false;
        }

        public List<string> GetVariationsOfWord(string word)
        {
            var optionalWords = new List<string>() { word }; //phasz fa5z || pha5z
            Dictionary<char, List<string>> charMisspelling = new()
            {
                { 'a', new() {"@", "á", "ä"} },
                { 'b', new() { "B", "I3", "l3", "i3", "ß" } },
                { 'c', new() {"("} },
                { 'd', new() {"Đ", "đ"} },
                { 'e', new() {"3", "é"} },
                { 'f', new() {"ph", "|="} },
                { 'g', new() {"6"} },
                { 'h', new() {"|n"} },
                { 'i', new() {"í", "l", "!", "1"} },
                { 'j', new() {"ly"} },
                { 'k', new() {"|<"} },
                { 'l', new() {"ly", "1", "!"} },
                { 'n', new() {"ny"} },
                { 'o', new() {"0", "ó", "ö", "ő"} },
                //{ 'p', new() {""} },
                { 'q', new() {"9"} },
                //{ 'r', new() {""} },
                { 's', new() {"5", "$", "sz"} },
                { 't', new() {"ty", "7"} },
                { 'u', new() {"ú", "ü", "ű"} },
                { 'v', new() {"\\/"} },
                { 'x', new() {"×", "ksz", "gz"} },
                //{ 'y', new() {""} },
                { 'z', new() {"sz", "2"} },
            };

            List<List<string>> charOptions = new();

            foreach (var _char in word)
                foreach (var pair in charMisspelling)
                    if (_char == pair.Key)
                        charOptions.Add(pair.Value);

            for (int i = 0; i < charOptions.Count(); i++)
                foreach (var charOption in charOptions[i])
                {
                    var temp = string.Empty;
                    for (int j = 0; j < word.Length; j++)
                        if (j != i)
                            temp += word[j];
                        else
                            temp += charOption;
                    optionalWords.Add(temp);
                }
            return optionalWords;
        }
    }
}
