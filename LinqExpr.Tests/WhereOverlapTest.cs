using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
 
namespace LinqExpr.Tests {
	[TestClass]
	public class WhereOverlapTest {
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

				new IntervalNullable(20, null,null)
			}
			.AsQueryable();

		[TestMethod]
		public void Where1() {
			var intervals = SrcNullable();
			var res = intervals
				.WhereOverlap(
					fromField: ii => ii.Start,
					toField: ii => ii.End,
					from: 3,
					to: 7)
				.Select(ii => ii.Id)
				.ToArray();

			Assert.IsTrue(res.SequenceEqual(new[] { 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14, 15, 16, 17, 18, 20 }));
		}
	}
}
