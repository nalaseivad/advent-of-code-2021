using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace AdventOfCode2021.Tests.EnumerableExtensions
{
  public class PartitionTests
  {
    [Fact]
    public void InputNull()
    {
      IEnumerable<int> input = null;
      Assert.Throws<ArgumentNullException>(() => input.Partition(2).ToArray());
    }

    [Fact]
    public void ZeroBucketSize()
    {
      var input = new[] { 1, 2, 3, 4 };
      Assert.Throws<ArgumentOutOfRangeException>(() => input.Partition(0).ToArray());
    }

    [Fact]
    public void NegativeBucketSize()
    {
      var input = new[] { 1, 2, 3, 4 };
      Assert.Throws<ArgumentOutOfRangeException>(() => input.Partition(-1).ToArray());
    }

    [Fact]
    public void ZeroOffset()
    {
      var input = new[] { 1, 2, 3, 4 };
      Assert.Throws<ArgumentOutOfRangeException>(() => input.Partition(2, 0).ToArray());
    }

    [Fact]
    public void NegativeOffset()
    {
      var input = new[] { 1, 2, 3, 4 };
      Assert.Throws<ArgumentOutOfRangeException>(() => input.Partition(2, -1).ToArray());
    }

    [Fact]
    public void NonOverlappingBuckets()
    {
      var input = new [] { 1, 2, 3, 4 };
      var expected = new [] { new [] { 1, 2 }, new [] { 3, 4 } };
      Assert.Equal(expected, input.Partition(2).ToArray());
    }

    [Fact]
    public void PartialFinalBucket()
    {
      var input = new [] { 1, 2, 3, 4, 5 };
      var expected = new [] { new [] { 1, 2 }, new [] { 3, 4 }, new [] { 5 } };
      Assert.Equal(expected, input.Partition(2).ToArray());
    }

    [Fact]
    public void UnitBuckets()
    {
      var input = new [] { 1, 2, 3, 4 };
      var expected = new [] { new [] { 1 }, new [] { 2 }, new [] { 3 }, new [] { 4 } };
      Assert.Equal(expected, input.Partition(1).ToArray());
    }

    [Fact]
    public void OverlappingBuckets()
    {
      var input = new [] { 1, 2, 3, 4 };
      var expected = new [] { new [] { 1, 2 }, new [] { 2, 3 }, new [] { 3, 4 }, new [] { 4 } };
      Assert.Equal(expected, input.Partition(2, 1).ToArray());
    }

    [Fact]
    public void EmptyInputNonOverlapping()
    {
      var input = new int[] { };
      var expected = new int[][] { };
      Assert.Equal(expected, input.Partition(2).ToArray());
    }

    [Fact]
    public void EmptyInputOverlapping()
    {
      var input = new int[] { };
      var expected = new int[][] { };
      Assert.Equal(expected, input.Partition(2, 1).ToArray());
    }
  }
}
