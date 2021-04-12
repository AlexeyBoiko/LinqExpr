using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LinqExpr.Tests {
	[TestClass]
	public class InRangeTest {

		IQueryable<Interval> Src() =>
			new[] {
				new Interval(1, 0,2),
				new Interval(2, 1,3),
				new Interval(3, 2,4),
				new Interval(4, 3,5),
				new Interval(5, 4,6),
				new Interval(6, 5,7),
				new Interval(7, 6,8),
				new Interval(8, 7,9),
				new Interval(9, 8,10)
			}
			.AsQueryable();

		void InRange(int? from, int? to, int[] expectedIds) {
			var intervals = Src();

			var predicate = Predicate.WhereInRange<Interval, int>(
				field: ii => ii.Start,
				from: from,
				to: to,
				excludeFrom: false,
				excludeTo: false);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		void InRangeExcludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = Src();

			var predicate = Predicate.WhereInRange<Interval, int>(
				field: ii => ii.Start,
				from: from,
				to: to,
				excludeFrom: true,
				excludeTo: true);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		[TestMethod]
		public void InRange1() => InRange(3, 7, new[] { 4, 5, 6, 7, 8, });

		[TestMethod]
		public void InRange2() => InRange(3, null, new[] { 4, 5, 6, 7, 8,9 });

		[TestMethod]
		public void InRange3() => InRange(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 8 });

		[TestMethod]
		public void InRange4() => InRange(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

		[TestMethod]
		public void InRange5() => InRangeExcludeEnds(3, 7, new[] { 5, 6, 7 });

		[TestMethod]
		public void InRange6() => InRangeExcludeEnds(3, null, new[] { 5, 6, 7, 8, 9 });

		[TestMethod]
		public void InRange7() => InRangeExcludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7 });

		[TestMethod]
		public void InRange8() => InRangeExcludeEnds(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

		[TestMethod]
		public void InRange9() {
			var intervals = Src();
			var res = intervals
				.WhereInRange(
					field: ii => ii.Start,
					from: 3,
					to: 7)
				.Select(ii => ii.Id)
				.ToArray();
			Assert.IsTrue(res.SequenceEqual(new[] { 4, 5, 6, 7, 8, }));
		}
	}
}
