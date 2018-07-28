using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hello.lib.data
{
    public static class QuotesReader
    {
        public static List<string> Get()
        {
            return Directory.GetFiles(FileHelper.QuotesDir).SelectMany(Get).ToList();
        }

        private static List<string> Get(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var items = text.Split(new[] { "@@@@@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            items = items
                .Where(s => s.Contains("#мудрые_мысли"))
                .Select(Process)
                .ToList();
            return items;
        }

        private static string Process(string s)
        {
            s = s.Split(new[] { "#мудрые_мысли" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            s = s.Split('\n')[0].Trim();
            return s;
        }
    }
}