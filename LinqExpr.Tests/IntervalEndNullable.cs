using System.Diagnostics;

namespace LinqExpr.Tests {
	[DebuggerDisplay("[{Start},{End}]")]
	class IntervalEndNullable {
		public IntervalEndNullable(int id, int start, int? end) {
			Start = start;
			End = end;
			Id = id;
		}

		public int Id { get; }

		public int Start { get; }
		public int? End { get; }
	}
}
