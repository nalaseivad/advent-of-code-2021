using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2021
{

  public static class Dec05
  {
    private enum Slope { Zero, Infinity, One, NegativeOne, Other };

    private class Line
    {
      public int X1 { get; set; }
      public int Y1 { get; set; }
      public int X2 { get; set; }
      public int Y2 { get; set; }

      // x axis points right
      // y axis points down
      public Slope Slope {
        get {
          // We know that X2 - X1 >= 0 by design
          if(X1 == X2) return Slope.Infinity;
          if(Y1 == Y2) return Slope.Zero;
          if((Y2 - Y1) / (X2 - X1) == 1) return Slope.NegativeOne;
          if((Y2 - Y1) / (X2 - X1) == -1) return Slope.One;
          return Slope.Other;
        }
      }
    }

    private class Point : IEquatable<Point>
    {
      public int X { get; set; }
      public int Y { get; set; }

      public bool Equals([AllowNull] Point other) { return X == other.X && Y == other.Y; }

      public override int GetHashCode() { return X.GetHashCode() ^ Y.GetHashCode(); }
    }

    private static readonly string _file = "Dec05/input.txt";
    //private static readonly string _file = "Dec05/test-input.txt";

    public static int Part1()
    {
      return Part1(File.ReadLines(_file));
    }

    public static int Part2()
    {
      return Part2(File.ReadLines(_file));
    }

    public static int Part1(IEnumerable<string> strings)
    {
      var pointCounts = strings
        // Generate all lines ...
        .Lines()
        // ... that are horizontal or vertical
        .Where(line =>  line.Slope == Slope.Zero ||
                        line.Slope == Slope.Infinity)
        // Interpolate the lines into points on the lines
        .Points()
        // Then group by point to count the number of lines that cross each point
        .GroupBy(p => p)
        .Select(group => new { Point = group.Key, Count = group.Count() });

      //PrintTable(pointCounts.ToDictionary(pc => pc.Point, pc => pc.Count));

      // How many of these points have at least one line crossing it?
      return pointCounts.Where(pc => pc.Count > 1).Count();
    }

    public static int Part2(IEnumerable<string> strings)
    {      
      var pointCounts = strings
        // Generate all lines ...
        .Lines()
        // ... that are horizontal, vertical or 45 degrees
        .Where(line =>  line.Slope == Slope.Zero ||
                        line.Slope == Slope.Infinity ||
                        line.Slope == Slope.One ||
                        line.Slope == Slope.NegativeOne)
        // Interpolate the lines into points on the lines
        .Points()
        // Then group by point to count the number of lines that cross each point
        .GroupBy(p => p)
        .Select(group => new { Point = group.Key, Count = group.Count() });

      //PrintTable(pointCounts.ToDictionary(pc => pc.Point, pc => pc.Count));

      // How many of these points have at least one line crossing it?
      return pointCounts.Where(pc => pc.Count > 1).Count();
    }

    //
    // Parse the input into a list of lines
    //
    private static IEnumerable<Line> Lines(this IEnumerable<string> strings)
    {
      return strings.Select(line => {
        var match = Regex.Match(line, @"(\d+),(\d+) -> (\d+),(\d+)");
        var x1 = int.Parse(match.Groups[1].Value);
        var x2 = int.Parse(match.Groups[3].Value);
        var y1 = int.Parse(match.Groups[2].Value);
        var y2 = int.Parse(match.Groups[4].Value);
        // Standardize so that X1 <= X2
        if(x2 < x1) {
          var tmp = x1;
          x1 = x2;
          x2 = tmp;
          tmp = y1;
          y1 = y2;
          y2 = tmp;
        }
        return new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
      });
    }

    //
    // Enumerate all the points along a given line
    //
    private static IEnumerable<Point> Points(this IEnumerable<Line> lines)
    {
      var ret = new List<Point>();
      foreach(var line in lines) {
        var x1 = line.X1;
        var x2 = line.X2;
        var y1 = line.Y1;
        var y2 = line.Y2;
        // x2 - x1 >= 0 by design
        switch(line.Slope) {
        case Slope.Zero:
          // (x1,y1)---(x2,y2)
          ret.AddRange(Enumerable.Range(0, x2 - x1 + 1).Select(n => new Point { X = x1 + n, Y = y1 }));
          break;
        case Slope.Infinity:
          if(y1 > y2)
            // (x1,y1)
            //    |
            // (x2,y2)
            ret.AddRange(Enumerable.Range(0, y1 - y2 + 1).Select(n => new Point { X = x1, Y = y2 + n }));
          else
            // (x2,y2)
            //    |
            // (x1,y1)
            ret.AddRange(Enumerable.Range(0, y2 - y1 + 1).Select(n => new Point { X = x1, Y = y1 + n }));
          break;
        case Slope.One:
          //   (x2,y2)
          //     /
          // (x1,y1)
          ret.AddRange(Enumerable.Range(0, y1 - y2 + 1).Select(n => new Point { X = x1 + n, Y = y1 - n }));
          break;
        case Slope.NegativeOne:
          // (x1,y1)
          //     \
          //   (x2,y2)
          ret.AddRange(Enumerable.Range(0, y2 - y1 + 1).Select(n => new Point { X = x1 + n, Y = y1 + n }));
          break;
        }
      }
      return ret;
    }

    private static void PrintLines(IEnumerable<Line> lines, string label)
    {
      Console.WriteLine($"{label}: [");
      foreach(var line in lines) Console.WriteLine($"  [{line.X1},{line.Y1}] -> [{line.X2},{line.Y2}] : {line.Slope}");
      Console.WriteLine("]");
    }

    private static void PrintPoints(IEnumerable<Point> points, string label)
    {
      Console.WriteLine($"{label}: [");
      foreach(var point in points) Console.WriteLine($"  [{point.X},{point.Y}]");
      Console.WriteLine("]");
    }

    private static void PrintTable(IDictionary<Point, int> dict)
    {
      Console.WriteLine();
      for(var y = 0; y < 10; ++y) {
        for(var x = 0; x < 10; ++x) {
          var key = new Point { X = x, Y = y };
          Console.Write(dict.ContainsKey(key) ? dict[key].ToString() : ".");
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}