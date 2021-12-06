using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2021
{
  public static class Dec02
  {
    private static readonly string _file = "Dec02/input.txt";
    //private static readonly string _file = "Dec02/test-input.txt";

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
        .Select(s => {
          var pair = s.Split(' ');
          return new { Direction = pair[0], Value = int.Parse(pair[1]) };
        })
        .Aggregate(new { Forward = 0, Down = 0 }, (working, x) => {
          var forward = working.Forward;
          var down = working.Down;
          switch(x.Direction) {
          case "forward": forward += x.Value; break;
          case "down": down += x.Value; break;
          case "up": down -= x.Value; break;
          }
          return new { Forward = forward, Down = down };
        },
        working => working.Forward * working.Down);
    }

    public static int Part2(IEnumerable<string> lines)
    {
      return lines
        .Select(s => {
          var pair = s.Split(' ');
          return new { Direction = pair[0], Value = int.Parse(pair[1]) };
        })
        .Aggregate(new { Forward = 0, Down = 0, Aim = 0 }, (working, x) => {
          var forward = working.Forward;
          var down = working.Down;
          var aim = working.Aim;
          //Console.WriteLine($"START f={forward}, d={down}, a={aim}");
          switch(x.Direction) {
          case "forward":
            forward += x.Value;
            down += aim * x.Value;
            break;
          case "down":
            aim += x.Value;
            break;
          case "up":
            aim -= x.Value;
            break;
          }
          //Console.WriteLine($"END f={forward}, d={down}, a={aim}");
          return new { Forward = forward, Down = down, Aim = aim };
        },
        working => working.Forward * working.Down);
    }
  }
}