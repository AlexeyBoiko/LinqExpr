# LinqExpr
[NuGet](https://www.nuget.org/packages/LinqExpr/) 

[Article "Creating reusable Linq filters (predicate builders for Where) that can be applied to different types"](https://alexey-boyko.medium.com/creating-reusable-linq-filters-predicate-builders-for-where-that-can-be-applied-to-different-f7b292c6c9a7)

---


.Net library for creating reusable Linq queries. The library allows you to use MemberExpression to specify the fields of an object used in a query. For example: a reusable query to find objects that intersect with a given period of time.

The library fully supports Entity Framework, including Async operations.

The library uses "[A universal PredicateBuilder](https://petemontgomery.wordpress.com/2011/02/10/a-universal-predicatebuilder/ "A universal PredicateBuilder")" created by Pete Montgomery.

## Navigation
* [Introduction](#introduction)
* [Built-in reusable queries](#built-in-reusable-queries)
  * [WhereOverlap](#whereoverlap)
  * [Predicate.WhereOverlap](#predicatewhereoverlap)
  * [WhereInRange](#whereinrange)
  * [Predicate.WhereInRange](#predicatewhereinrange)
* [How to make your reusable linq query](#how-to-make-your-reusable-linq-query)

## Introduction
The library contains [built-in reusable Linq queries](#built-in-reusable-queries). The library also allows you to [make your own reusable Linq queries](#how-to-make-your-reusable-linq-query).

**What is reusable queries?**

For example, you have Order object.
```cs
class Order { 
	public DateTime Start { get; set; }
	public DateTime? End { get; set; }
}
```
You need to find all the Orders that hit the specified time range.

LinqExpr library has "WhereOverlap" method for that:
```cs
using System.Linq;
using LinqExpr;

var ordersFiltered = orders
	.WhereOverlap(
		// set up search fields
		// MemberExpressions (Orders has Start, End fields)
		fromField: oo => oo.Start,
		toField: oo => oo.End,

		// set up search range
		from: DateTime.Now.AddDays(-10),
		to: DateTime.Now)
	.ToList();
```
MemberExpressions is used to set up search fields.

The same "WhereOverlap" method can be applied to Trip:
```cs
class Trip { 
	public DateTime? From { get; set; }
	public DateTime? To { get; set; }
}
```
```cs
var tripsFiltered = trips
	.WhereOverlap(
		// set up search fields
		// MemberExpressions (Trip has From, To fields)
		fromField: oo => oo.From,
		toField: oo => oo.To,

		// set up search range
		from: DateTime.Now.AddDays(-10),
		to: DateTime.Now)
	.ToList();
```
WhereOverlap - is reusable predicate builder. You can applay it to any type of ojects, because WhereOverlap allow to set up search fields.

## Built-in reusable queries
### WhereOverlap
Finds objects in a given range. Ranges of dates, numbers, decimal, float, etc. are supported.

For simplicity, the examples use integer intervals:
```cs
class Interval {
	public Interval(int? start, int? end) {
		Start = start;
		End = end;
	}
	public int? Start{ get; set; }
	public int? End { get; set; }
}
```

Usage:
```cs
var intervals = new[] { 
	new Interval(0,2),
	new Interval(1,4),
	new Interval(5,8),
	new Interval(null,9)
}
.AsQueryable();

var res = intervals
	.WhereOverlap(
		// set up search fields
		fromField: ii => ii.Start,
		toField: ii => ii.End,

		// set up search range
		from: 3,
		to: 7)
	.ToList();
// => [1,4], [5,8], [null,9]
```
```cs
Intervals
0       2
|-------|
    1           4
    |-----------|
                    5           8
                    |-----------|
                                    9
------------------------------------|

WhereOverlap
            3               7
            |---------------|

Result:
    1           4
    |-----------|
                    5           8
                    |-----------|
                                    9
------------------------------------|
```

By default, the ends of the intervals are taken into account. You can exclude the ends:
```cs
var res = intervals
	.WhereOverlap(
		// set up search fields
		fromField: ii => ii.Start,
		toField: ii => ii.End,

		// set up search range
		from: 3,
		to: 7,

		// exclude ends
		excludeFrom: true,
		excludeTo: true)
	.ToList();
```

You can search by a range without end or / and beginning:
```cs
var res = intervals
	.WhereOverlap(
		// set up search fields
		fromField: ii => ii.Start,
		toField: ii => ii.End,

		// search by range without start
		from: null,
		to: 7,

		// exclude ends
		excludeFrom: true,
		excludeTo: true)
	.ToList();
```
### Predicate.WhereOverlap
WhereOverlap predicate can be constructed separately and used in OR conditions:
```cs
var predicateOverlap = Predicate.WhereOverlap<Interval, int>(
	// set up search fields
	fromField: ii => ii.Start,
	toField: ii => ii.End,

	// set up search range
	from: 3,
	to: 7);

// create OR predicate
var predicateOr = PredicateBuilder.Or(
	predicateOverlap, 
	ii => ii.Start == 0);

var res = intervals
	.Where(predicateOr)
	.ToList();
```
Predicate.WhereOverlap supports the same parameters as the [WhereOverlap](#whereoverlap) method.

### WhereInRange
Finds objects in which the specified field is within the range. Ranges of dates, numbers, decimal, float, etc. are supported.

For simplicity, the examples use integer:
```cs
class Point {
	public Point(int position) {
		Position = position;
	}
	public int Position { get; set; }
}
```

Usage:
```cs
var points = new[] {
	new Point(0),
	new Point(2),
	new Point(5),
	new Point(6),
	new Point(9),
}
.AsQueryable();

var res = points
	.WhereInRange(
		// set up search field
		field: pp => pp.Position,

		// set up search range
		from: 1,
		to: 6)
	.ToList();
// => 2, 5, 6
```
```cs
Points
0       2           5   6           9
|       |           |   |           |
Range
    1                   6
    |-------------------|

Result:
        2           5   6
        |           |   |

```
By default, the ends of the range are taken into account. You can exclude the ends:
```cs
var res = points
	.WhereInRange(
		// set up search field
		field: pp => pp.Position,

		// set up search range
		from: 1,
		to: 6,

		// exclude ends
		excludeFrom: true,
		excludeTo: true)
	.ToList();
```
You can search by a range without end or / and beginning:
```cs
var res = points
	.WhereInRange(
		// set up search field
		field: pp => pp.Position,

		// set up search range without start
		from: null,
		to: 6,

		// exclude ends
		excludeFrom: true,
		excludeTo: true)
	.ToList();
```
### Predicate.WhereInRange
WhereInRange predicate can be constructed separately and used in OR conditions:
```cs
var predicateInRange = Predicate.WhereInRange<Point, int>(
	// set up search field
	field: pp => pp.Position,

	// set up search range
	from: 1,
	to: 6);

// create OR predicate
var predicateOr = PredicateBuilder.Or(
	predicateInRange,
	pp => pp.Position == 9);

var res = points
	.Where(predicateOr)
	.ToList();
// => 2, 5, 6, 9
```
Predicate.WhereInRange supports the same parameters as the [WhereInRange](#whereinrange) method.
## How to make your reusable linq query
For example, there are Payout and Premium objects:
```cs
class Payout { 
	public decimal Total { get; set; }
}

class Premium {
	public decimal Sum { get; set; }
}
```
Let's make a reusable query to find Payouts and Premiums greater than a certain limit:
```cs
using LinqExpr;

class BigPayFilter {
	readonly decimal Limit;
	public BigPayFilter(decimal limit) {
		Limit = limit;
	}

	public Expression<Func<TEnt, bool>> Create<TEnt>(
		Expression<Func<TEnt, decimal>> field) {

		// GreaterOrEqual is extension in LinqExpr namespace
		return field.GreaterOrEqual(Limit);
	}
}
```
Usage:
```cs
// filter to find payments greater or equal 1000
//
// you can get limit value from configuration (in this example limit is 1000),
// and put BigPayFilter to IoC-container
var bigPayFilter = new BigPayFilter(1000);


// use BigPayFilter for payouts

var payoutPredicate =
	bigPayFilter.Create<Payout>(pp => pp.Total);

var payouts = new[] {
	new Payout{ Total = 100 },
	new Payout{ Total = 50 },
	new Payout{ Total = 25.5m },
	new Payout{ Total = 1050.67m }
}
.AsQueryable()
.Where(payoutPredicate)
.ToList();


// use BigPayFilter for premiums

var premiumPredicate =
	bigPayFilter.Create<Premium>(pp => pp.Sum);

var premiums = new[] {
	new Premium{ Sum = 2000 },
	new Premium{ Sum = 50.08m },
	new Premium{ Sum = 25.5m },
	new Premium{ Sum = 1070.07m }
}
.AsQueryable()
.Where(premiumPredicate)
.ToList();
```
