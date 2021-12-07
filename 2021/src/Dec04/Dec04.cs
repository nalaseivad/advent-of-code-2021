using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AdventOfCode2021
{
 
  public static class Dec04
  {
    private enum FirstOrLast { First, Last }

    private static readonly string _file = "Dec04/input.txt";
    //private static readonly string _file = "Dec04/test-input.txt";

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
      return lines.CommonPart(FirstOrLast.First);
    }

    public static int Part2(IEnumerable<string> lines)
    {
      return lines.CommonPart(FirstOrLast.Last);
    }

    private static int CommonPart(this IEnumerable<string> lines, FirstOrLast firstOrLast)
    {
      // The full list of called numbers ...
      //   [ n0, n1, n2, n3, ... ]
      var calledNumbers = lines.First().Split(',').Select(value => int.Parse(value));

      // A list of lists of calls up to each new number ...
      //   [ [ n0 ], [ n0, n1 ], [ n0, n1, n2 ], [n0, n1, n2, n3 ], ... ]
      var calls = calledNumbers.Select((n, index) => new { Index = index, Calls = calledNumbers.Take(index + 1) });

      // A list of Bingo cards with all the numbers as a single flattened list ...
      //  [ { Index = 0, Numbers = [ n0, n1, ..., n24 ] }, { Index = 1, Numbers = [ n0, n1, ..., n24 ] }, ... ]
      var cards = lines
        // Skip the line of called numbers
        .Skip(1)
        // Add row-index
        .Select((row, index) => new { Row = row, Index = index })
        // Group by card index (row-index / 6 where integer division truncates)
        .GroupBy(value => value.Index / 6, value => value.Row)
        // Flatten each card block into one line
        .Select(group => new {
          Index = group.Key,
          Value = group.Aggregate("", (working, line) => working + " " + line)  // Concatenate all lines together
        })
        // Trim leading and trailing whitespace and then split on whitespace
        .Select(x => new {
          Index = x.Index,
          Numbers = Regex.Split(x.Value.Trim(), @"\s+").Select(s => int.Parse(s)).ToList()
        });

      // Add in the rows from each card as sets of 5 "winning numbers" (row-index = num-index / 5)
      var rows = cards
        .SelectMany(value => value.Numbers
                                .Select((number, index) => new { Number = number, Index = index })
                                .GroupBy(x => x.Index / 5, x => x.Number)
                                .Select(group => new {
                                  Index = value.Index,
                                  AllNumbers = value.Numbers,
                                  WinningNumbers = group
                                }));
      // Same for the columns (column-index = num-index % 5)
      var columns = cards
        .SelectMany(value => value.Numbers
                                .Select((number, index) => new { Number = number, Index = index })
                                .GroupBy(x => x.Index % 5, x => x.Number)
                                .Select(group => new {
                                  Index = value.Index,
                                  AllNumbers = value.Numbers,
                                  WinningNumbers = group
                                }));
      // Combine
      var wins = rows.Union(columns);

      // Do the cartesian product of all the call sequences with all the card winning combinations
      var winningCards = wins
        .SelectMany(win => calls.Select(call => new { Win = win, Call = call }))
        // Then filter these entries to only include those where the card winning numbers are in the call sequence
        .Where(x => x.Win.WinningNumbers.All(number => x.Call.Calls.Contains(number)))
        // Then order the entries by call sequence number
        .OrderBy(x => x.Call.Index)
        // And only include each card once, the first time that card wins
        .GroupBy(x => x.Win.Index)
        .Select(group => group.First());

      // We want either the card that will win first (the one with the lowest call sequence number) or the one that will
      // win last (the oen with the highest call sequence number)
      var winningCard = firstOrLast == FirstOrLast.First ? winningCards.First() : winningCards.Last();

      // Compute the card score: the sum of all the numbers on the card that were not called ...
      var cardScore = winningCard.Win.AllNumbers.Except(winningCard.Call.Calls).Sum();
      // ... and the last number called ...
      var lastNumberCalled = winningCard.Call.Calls.Last();

      Console.WriteLine();
      Console.WriteLine($"cardScore={cardScore}");
      Console.WriteLine($"lastNumberCalled={lastNumberCalled}");

      return cardScore * lastNumberCalled;
    }
  }
}