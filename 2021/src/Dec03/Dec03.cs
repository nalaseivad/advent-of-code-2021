using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AdventOfCode2021
{
  public static class Dec03
  {
    private static readonly string _file = "Dec03/input.txt";
    //private static readonly string _file = "Dec03/test-input.txt";

    public static int Part1()
    {
      return Part1(File.ReadLines(_file));
    }

    public static int Part2()
    {
      return Part2(File.ReadLines(_file));
    }

    public static int Part1(IEnumerable<string> lines)
    {
      var gamma = lines.BitwiseMode(1);
      var epsilon = BitwiseNot(gamma);
      Console.WriteLine($"gamma={gamma}, epsilon={epsilon}");

      return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
    }

    public static int Part2(IEnumerable<string> lines)
    {
      var o2GenRating = Convert.ToInt32(lines.FilterLines(lines => lines.BitwiseMode(1)).First(), 2);
      Console.WriteLine($"o2GenRating={o2GenRating}");

      var co2ScrubberRating = Convert.ToInt32(lines.FilterLines(lines => BitwiseNot(lines.BitwiseMode(1))).First(), 2);
      Console.WriteLine($"co2ScrubberRating={co2ScrubberRating}");

      return o2GenRating * co2ScrubberRating;
    }

    private static IEnumerable<string> FilterLines(
      this IEnumerable<string> lines,
      Func<IEnumerable<string>, string> filterFn,
      int index = -1)
    {
      if(lines.Count() == 1) return lines;
      var filter = filterFn(lines);
      return lines.Where(s => s[index] == filter[index]).FilterLines(filterFn, ++index);
    }

    private static string BitwiseMode(this IEnumerable<string> lines, int modeTieBitValue)
    {
      return lines
        .SelectMany(s => s.Select((c, index) => new { Bit = c - '0', Index = index }))
        .GroupBy(value => value.Index, value => value.Bit)
        .Select(group => {
          var modes = group.AllModes();
          var mode = modes.Count() > 1 ? modeTieBitValue : modes.First();
          return new { Index = group.Key, ModalBit = mode };
        })
        .Aggregate(new StringBuilder(), (working, value) => {
          working.Append(value.ModalBit);
          return working;
        },
        working => working.ToString());
    }

    private static string BitwiseNot(string s)
    {
      return s.Aggregate(new StringBuilder(),
                         (builder, c) => {
                           builder.Append(c == '1' ? '0' : '1');
                           return builder;
                         },
                         builder => builder.ToString());
    }
  }
}