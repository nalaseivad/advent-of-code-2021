using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2021
{
  public static class Dec01
  {
    //private static readonly string _file = "Dec01/input.txt";
    private static readonly string _file = "Dec01/test-input.txt";

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
      return lines
        .Select(s => int.Parse(s))  // Convert string int to int
        .CountPairWiseIncreasing();
    }

    public static int Part2(IEnumerable<string> lines)
    {
      return lines
        .Select(s => int.Parse(s))    // Convert string int to int
        .Partition(3, 1)              // [1, 2, 3, 4, ...] -> [[1, 2, 3], [2, 3, 4], [3, 4, 5], ...]
        .Select(list => list.Sum())   // Sum each partition
        .CountPairWiseIncreasing();
    }

    internal static int CountPairWiseIncreasing(this IEnumerable<int> numbers)
    {
      return numbers
        .Partition(2, 1)                                     // [1, 2, 3, 4, ...] -> [[1, 2], [2, 3], [3, 4], ...]
        .Where(pair => pair.LastOrDefault() > pair.First())  // Discard where pair[1] is not greater than pair[0]
        .Count();                                            // Count the number of pairs that pass
    }
  }
}