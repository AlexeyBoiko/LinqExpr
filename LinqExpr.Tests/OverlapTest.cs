using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LinqExpr.Tests {
	[TestClass]
	public class OverlapTest {

		// nullable	

		IQueryable<IntervalNullable> SrcNullable() =>
			new[] {
				new IntervalNullable(1, 0,2),
				new IntervalNullable(2, 1,3),
				new IntervalNullable(3, 2,4),
				new IntervalNullable(4, 3,5),
				new IntervalNullable(5, 4,6),
				new IntervalNullable(6, 5,7),
				new IntervalNullable(7, 6,8),
				new IntervalNullable(8, 7,9),
				new IntervalNullable(9, 8,10),

				new IntervalNullable(10, null,2),
				new IntervalNullable(11, null,3),
				new IntervalNullable(12, null,4),
				new IntervalNullable(13, null,7),
				new IntervalNullable(14, null,8),

				new IntervalNullable(15, 2,null),
				new IntervalNullable(16, 3,null),
				new IntervalNullable(17, 4,null),
				new IntervalNullable(18, 7,null),
				new IntervalNullable(19, 8,null),

				new IntervalNullable(20, null,null),
			}
			.AsQueryable();

		void NullableIncludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = SrcNullable();

			var predicate = Predicate.WhereOverlap<IntervalNullable, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		void NullableExcludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = SrcNullable();

			var predicate = Predicate.WhereOverlap<IntervalNullable, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to,
				excludeFrom: true,
				excludeTo: true);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}


		[TestMethod]
		public void Overlap1() => NullableIncludeEnds(3, 7, new[] { 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14, 15, 16, 17, 18, 20 });

		[TestMethod]
		public void Overlap2() => NullableIncludeEnds(3, null, new[] { 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

		[TestMethod]
		public void Overlap3() => NullableIncludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20 });

		[TestMethod]
		public void Overlap4() => NullableIncludeEnds(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

		[TestMethod]
		public void Overlap5() => NullableExcludeEnds(3, 7, new[] { 3, 4, 5, 6, 7, 12, 13, 14, 15, 16, 17, 20 });

		[TestMethod]
		public void Overlap6() => NullableExcludeEnds(3, null, new[] { 3, 4, 5, 6, 7, 8, 9, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

		[TestMethod]
		public void Overlap7() => NullableExcludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14, 15, 16, 17, 20 });

		[TestMethod]
		public void Overlap8() => NullableExcludeEnds(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });


		//Not nullable

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

		void IncludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = Src();

			var predicate = Predicate.WhereOverlap<Interval, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		void ExcludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = Src();

			var predicate = Predicate.WhereOverlap<Interval, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to,
				excludeFrom: true,
				excludeTo: true);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		[TestMethod]
		public void Overlap9() => IncludeEnds(3, 7, new[] { 2, 3, 4, 5, 6, 7, 8 });

		[TestMethod]
		public void Overlap10() => IncludeEnds(3, null, new[] { 2, 3, 4, 5, 6, 7, 8, 9 });

		[TestMethod]
		public void Overlap11() => IncludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 8 });

		[TestMethod]
		public void Overlap12() => ExcludeEnds(3, 7, new[] { 3, 4, 5, 6, 7 });

		[TestMethod]
		public void Overlap13() => ExcludeEnds(3, null, new[] { 3, 4, 5, 6, 7, 8, 9 });

		[TestMethod]
		public void Overlap14() => ExcludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7 });


		// Nullable end

		IQueryable<IntervalEndNullable> SrcNullableEnd() =>
			new[] {
				new IntervalEndNullable(1, 0,2),
				new IntervalEndNullable(2, 1,3),
				new IntervalEndNullable(3, 2,4),
				new IntervalEndNullable(4, 3,5),
				new IntervalEndNullable(5, 4,6),
				new IntervalEndNullable(6, 5,7),
				new IntervalEndNullable(7, 6,8),
				new IntervalEndNullable(8, 7,9),
				new IntervalEndNullable(9, 8,10),
				new IntervalEndNullable(15, 2,null),
				new IntervalEndNullable(16, 3,null),
				new IntervalEndNullable(17, 4,null),
				new IntervalEndNullable(18, 7,null),
				new IntervalEndNullable(19, 8,null)
			}
			.AsQueryable();

		void NullableEndIncludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = SrcNullableEnd();

			var predicate = Predicate.WhereOverlap<IntervalEndNullable, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		void NullableEndExcludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = SrcNullableEnd();

			var predicate = Predicate.WhereOverlap<IntervalEndNullable, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to,
				excludeFrom: true,
				excludeTo: true);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		[TestMethod]
		public void Overlap15() => NullableEndIncludeEnds(3, 7, new[] { 2, 3, 4, 5, 6, 7, 8, 15, 16, 17, 18 });

		[TestMethod]
		public void Overlap16() => NullableEndIncludeEnds(3, null, new[] { 2, 3, 4, 5, 6, 7, 8,9, 15, 16, 17, 18, 19 });

		[TestMethod]
		public void Overlap17() => NullableEndIncludeEnds(null, 7, new[] { 1,2, 3, 4, 5, 6, 7, 8, 15, 16, 17, 18});

		[TestMethod]
		public void Overlap25() => NullableEndIncludeEnds(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 15, 16, 17, 18, 19 });

		[TestMethod]
		public void Overlap18() => NullableEndExcludeEnds(3, 7, new[] { 3, 4, 5, 6, 7, 15, 16, 17});

		[TestMethod]
		public void Overlap19() => NullableEndExcludeEnds(3, null, new[] { 3, 4, 5, 6, 7, 8, 9, 15, 16, 17, 18, 19 });

		[TestMethod]
		public void Overlap20() => NullableEndExcludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 15, 16, 17 });

		[TestMethod]
		public void Overlap26() => NullableEndExcludeEnds(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 15, 16, 17, 18, 19 });


		// nullable start

		IQueryable<IntervalStartNullable> SrcNullableStart() =>
			new[] {
				new IntervalStartNullable(1, 0,2),
				new IntervalStartNullable(2, 1,3),
				new IntervalStartNullable(3, 2,4),
				new IntervalStartNullable(4, 3,5),
				new IntervalStartNullable(5, 4,6),
				new IntervalStartNullable(6, 5,7),
				new IntervalStartNullable(7, 6,8),
				new IntervalStartNullable(8, 7,9),
				new IntervalStartNullable(9, 8,10),

				new IntervalStartNullable(10, null,2),
				new IntervalStartNullable(11, null,3),
				new IntervalStartNullable(12, null,4),
				new IntervalStartNullable(13, null,7),
				new IntervalStartNullable(14, null,8),
			}
			.AsQueryable();

		void NullableStartIncludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = SrcNullableStart();

			var predicate = Predicate.WhereOverlap<IntervalStartNullable, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		void NullableStartExcludeEnds(int? from, int? to, int[] expectedIds) {
			var intervals = SrcNullableStart();

			var predicate = Predicate.WhereOverlap<IntervalStartNullable, int>(
				fromField: ii => ii.Start,
				toField: ii => ii.End,
				from: from,
				to: to,
				excludeFrom: true,
				excludeTo: true);

			var res = intervals.Where(predicate).Select(ii => ii.Id).ToArray();
			Assert.IsTrue(res.SequenceEqual(expectedIds));
		}

		[TestMethod]
		public void Overlap21() => NullableStartIncludeEnds(3, 7, new[] { 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14 });

		[TestMethod]
		public void Overlap22() => NullableStartIncludeEnds(3, null, new[] { 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14 });

		[TestMethod]
		public void Overlap23() => NullableStartIncludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14 });

		[TestMethod]
		public void Overlap24() => NullableStartIncludeEnds(null, null, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });

		[TestMethod]
		public void Overlap27() => NullableStartExcludeEnds(3, 7, new[] { 3, 4, 5, 6, 7, 12, 13, 14 });

		[TestMethod]
		public void Overlap28() => NullableStartExcludeEnds(3, null, new[] { 3, 4, 5, 6, 7, 8, 9, 12, 13, 14 });

		[TestMethod]
		public void Overlap29() => NullableStartExcludeEnds(null, 7, new[] { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14 });
	}
}
