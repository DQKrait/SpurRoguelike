using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Generators
{
    internal class NameGenerator
    {
        public NameGenerator(int seed)
        {
            random = new Random(seed);
        }

        public string Generate()
        {
            var syllables = random.Next(1, 6);
            var name = string.Join("", Enumerable.Range(0, syllables).Select(i => GenerateSyllable()));
            foreach (var pair in BadComboReplacements)
                name = name.Replace(pair.Key, pair.Value);
            if (random.NextDouble() < 0.3)
                name += random.Select(Suffixes);
            return char.ToUpper(name[0]) + name.Substring(1);
        }

        private string GenerateSyllable()
        {
            if (random.NextDouble() < 0.5)
                return random.NextDouble() < 0.3 ? random.Select(Vowels).ToString() : random.Select(Consonants).ToString() + random.Select(Vowels);

            return random.Select(Vowels).ToString() + random.Select(Consonants);
        }

        private readonly Random random;

        private static readonly char[] Vowels = "aiueo".ToCharArray();

        private static readonly char[] Consonants = "bdfgklmnprstvz".ToCharArray();

        private static readonly string[] Suffixes =
        {
            "ard",
            "arian",
            "ary",
            "ator",
            "cule",
            "ox",
            "ee",
            "er",
            "ess",
            "ian",
            "oid",
            "or"
        };

        private static readonly Dictionary<string, string> BadComboReplacements = new Dictionary<string, string>()
        {
            { "bd", "dd" },
            { "bf", "ph" },
            { "bg", "g" },
            { "bk", "k" },
            { "bl", "" },
            { "bm", "m" },
            { "bn", "n" },
            { "bp", "p" },
            { "br", "" },
            { "bs", "ss" },
            { "bt", "t" },
            { "bv", "v" },
            { "bz", "zz" },
            { "db", "b" },
            { "df", "f" },
            { "dg", "g" },
            { "dm", "m" },
            { "dp", "p" },
            { "ds", "s" },
            { "dt", "t" },
            { "fb", "ff" },
            { "fd", "d" },
            { "fg", "fk" },
            { "fm", "m" },
            { "fp", "p" },
            { "fs", "s" },
            { "fv", "v" },
            { "fz", "zz" },
            { "gb", "gg" },
            { "gf", "ff" },
            { "gk", "gg" },
            { "gp", "pp" },
            { "gs", "ss" },
            { "gt", "tt" },
            { "gv", "ff" },
            { "gz", "zz" },
            { "kb", "bb" },
            { "kd", "dd" },
            { "kf", "ff" },
            { "kg", "kk" },
            { "km", "m" },
            { "kp", "pp" },
            { "ks", "ss" },
            { "kv", "v" },
            { "kz", "zz" },
            { "lf", "l" },
            { "lg", "l" },
            { "lp", "pp" },
            { "lr", "r" },
            { "lv", "v" },
            { "md", "dd" },
            { "mf", "ff" },
            { "mg", "mk" },
            { "ml", "ll" },
            { "mn", "mm" },
            { "mr", "rr" },
            { "ms", "ss" },
            { "mt", "tt" },
            { "mv", "v" },
            { "mz", "z" },
            { "nb", "mb" },
            { "nf", "f" },
            { "ng", "nk" },
            { "nl", "l" },
            { "nm", "nn" },
            { "np", "pp" },
            { "nr", "r" },
            { "ns", "ss" },
            { "nv", "v" },
            { "nz", "z" },
            { "pb", "pp" },
            { "pd", "dd" },
            { "pf", "ph" },
            { "pg", "pk" },
            { "pm", "mm" },
            { "pn", "nn" },
            { "ps", "ss" },
            { "pt", "tt" },
            { "pv", "pp" },
            { "pz", "zz" },
            { "rf", "rr" },
            { "rl", "l" },
            { "rm", "rn" },
            { "rp", "pp" },
            { "rs", "ss" },
            { "sb", "bb" },
            { "sd", "dd" },
            { "sf", "ff" },
            { "sg", "sk" },
            { "sl", "l" },
            { "sn", "sm" },
            { "sr", "ss" },
            { "sv", "s" },
            { "sz", "ss" },
            { "tb", "bb" },
            { "td", "dd" },
            { "tf", "ff" },
            { "tg", "tk" },
            { "tk", "" },
            { "tl", "ll" },
            { "tm", "mm" },
            { "tn", "nn" },
            { "tp", "pp" },
            { "tz", "ts" },
            { "vb", "bb" },
            { "vd", "ft" },
            { "vf", "ff" },
            { "vg", "fk" },
            { "vk", "fk" },
            { "vl", "ll" },
            { "vm", "vn" },
            { "vp", "pp" },
            { "vr", "rr" },
            { "vs", "fs" },
            { "vt", "ft" },
            { "vz", "zz" },
            { "zb", "bb" },
            { "zf", "ff" },
            { "zk", "zg" },
            { "zp", "pp" },
            { "zs", "ss" },
            { "zt", "tt" },
            { "zv", "v" }
        };
    }
}