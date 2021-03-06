﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LearnNewWord
{
    static class TextHelper
    {
        private const string pattern = @".*\t.*\t.*";
        public static List<Vocab> LoadAllVocabs(string filePath)
        {
            var listVocab = new List<Vocab>();
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath).ToList().Where(s => IsStringSuitable(s)))
                {

                    listVocab.Add(String2Vocab(line));
                }
            }
            return listVocab;
        }

        public static List<Vocab> LoadAllVocabs(string filePath, bool[] selectedParts)
        {
            var listVocab = new List<Vocab>();
            if (File.Exists(filePath))
            {
                var arr = File.ReadAllLines(filePath);
                var patternPart = @"#.+#";
                
                int countPart = 0;
                bool flag = true;//mark when..
                foreach (var s in arr)
                {
                    if (Regex.IsMatch(s, patternPart))
                    {
                        flag = selectedParts[countPart++];
                    }
                    else if (flag && IsStringSuitable(s))
                    {
                        listVocab.Add(String2Vocab(s));
                    }
                }
            }
            return listVocab;
        }

        public static List<string> LoadAllPart(string filePath)
        {
            var listPart = new List<string>();
            var arr = File.ReadAllLines(filePath);
            var pattern = @"#.+#";
            foreach (var s in arr)
            {
                if (Regex.IsMatch(s, pattern))
                {
                    var match = Regex.Match(s, @"#(.+)#");
                    var value = match.Groups[1].Value;
                    listPart.Add(value);
                }
            }
            return listPart;
        }

        private static bool IsStringSuitable(string s)
        {
            return !string.IsNullOrWhiteSpace(s) && !s.StartsWith("//") && Regex.IsMatch(s, pattern);
        }

        private static Vocab String2Vocab(string s)
        {
            if (!IsStringSuitable(s))
            {
                return null;
            }
            else
            {
                var arr = s.Split('\t');
                return new Vocab()
                {
                    Word = arr[0],
                    Kana = arr[1],
                    Meaning = arr[2]
                };
            }

        }
    }
}
