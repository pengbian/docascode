---
namespace: Microsoft.CodeAnalysis.Text
---

---
class: Microsoft.CodeAnalysis.Text.TextLineCollection
---

---
property: Microsoft.CodeAnalysis.Text.TextLineCollection.Count
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.GetPosition(Microsoft.CodeAnalysis.Text.LinePosition)
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.GetLinePosition(System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.GetLinePositionSpan(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.IndexOf(System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.GetTextSpan(Microsoft.CodeAnalysis.Text.LinePositionSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.GetLineFromPosition(System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextLineCollection.GetEnumerator
---

---
class: Microsoft.CodeAnalysis.Text.SourceText
---

---
property: Microsoft.CodeAnalysis.Text.SourceText.ChecksumAlgorithm
---

---
property: Microsoft.CodeAnalysis.Text.SourceText.Encoding
---

---
property: Microsoft.CodeAnalysis.Text.SourceText.Lines
---

---
property: Microsoft.CodeAnalysis.Text.SourceText.Container
---

---
property: Microsoft.CodeAnalysis.Text.SourceText.Length
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.#ctor(System.Collections.Immutable.ImmutableArray{System.Byte},Microsoft.CodeAnalysis.Text.SourceHashAlgorithm,Microsoft.CodeAnalysis.Text.SourceTextContainer)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.Replace(Microsoft.CodeAnalysis.Text.TextSpan,System.String)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.GetTextChanges(Microsoft.CodeAnalysis.Text.SourceText)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.CopyTo(System.Int32,System.Char[],System.Int32,System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.ToString(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.WithChanges(Microsoft.CodeAnalysis.Text.TextChange[])
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.ContentEqualsImpl(Microsoft.CodeAnalysis.Text.SourceText)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.ToString
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.From(System.IO.Stream,System.Text.Encoding,Microsoft.CodeAnalysis.Text.SourceHashAlgorithm)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.ContentEquals(Microsoft.CodeAnalysis.Text.SourceText)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.Write(System.IO.TextWriter,System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.Replace(System.Int32,System.Int32,System.String)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.GetSubText(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.GetChangeRanges(Microsoft.CodeAnalysis.Text.SourceText)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.WithChanges(System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.Text.TextChange})
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.GetSubText(System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.From(System.String,System.Text.Encoding,Microsoft.CodeAnalysis.Text.SourceHashAlgorithm)
---

---
method: Microsoft.CodeAnalysis.Text.SourceText.Write(System.IO.TextWriter,Microsoft.CodeAnalysis.Text.TextSpan,System.Threading.CancellationToken)
---

---
class: Microsoft.CodeAnalysis.Text.TextChangeEventArgs
---

---
property: Microsoft.CodeAnalysis.Text.TextChangeEventArgs.Changes
---

---
property: Microsoft.CodeAnalysis.Text.TextChangeEventArgs.NewText
---

---
property: Microsoft.CodeAnalysis.Text.TextChangeEventArgs.OldText
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeEventArgs.#ctor(Microsoft.CodeAnalysis.Text.SourceText,Microsoft.CodeAnalysis.Text.SourceText,System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.Text.TextChangeRange})
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeEventArgs.#ctor(Microsoft.CodeAnalysis.Text.SourceText,Microsoft.CodeAnalysis.Text.SourceText,Microsoft.CodeAnalysis.Text.TextChangeRange[])
---

---
class: Microsoft.CodeAnalysis.Text.SourceTextContainer
---

---
property: Microsoft.CodeAnalysis.Text.SourceTextContainer.CurrentText
---

---
event: Microsoft.CodeAnalysis.Text.SourceTextContainer.TextChanged
---

---
class: Microsoft.CodeAnalysis.Text.TextChange
---

---
field: Microsoft.CodeAnalysis.Text.TextChange.NoChanges
---

---
property: Microsoft.CodeAnalysis.Text.TextChange.NewText
---

---
property: Microsoft.CodeAnalysis.Text.TextChange.Span
---

---
method: Microsoft.CodeAnalysis.Text.TextChange.#ctor(Microsoft.CodeAnalysis.Text.TextSpan,System.String)
---

---
method: Microsoft.CodeAnalysis.Text.TextChange.GetHashCode
---

---
method: Microsoft.CodeAnalysis.Text.TextChange.Equals(System.Object)
---

---
method: Microsoft.CodeAnalysis.Text.TextChange.ToString
---

---
method: Microsoft.CodeAnalysis.Text.TextChange.Equals(Microsoft.CodeAnalysis.Text.TextChange)
---

---
class: Microsoft.CodeAnalysis.Text.TextLine
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.Span
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.End
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.Start
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.LineNumber
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.EndIncludingLineBreak
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.Text
---

---
property: Microsoft.CodeAnalysis.Text.TextLine.SpanIncludingLineBreak
---

---
method: Microsoft.CodeAnalysis.Text.TextLine.FromSpan(Microsoft.CodeAnalysis.Text.SourceText,Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextLine.GetHashCode
---

---
method: Microsoft.CodeAnalysis.Text.TextLine.Equals(System.Object)
---

---
method: Microsoft.CodeAnalysis.Text.TextLine.ToString
---

---
method: Microsoft.CodeAnalysis.Text.TextLine.Equals(Microsoft.CodeAnalysis.Text.TextLine)
---

---
class: Microsoft.CodeAnalysis.Text.TextSpan
---

---
property: Microsoft.CodeAnalysis.Text.TextSpan.IsEmpty
---

---
property: Microsoft.CodeAnalysis.Text.TextSpan.End
---

---
property: Microsoft.CodeAnalysis.Text.TextSpan.Length
---

---
property: Microsoft.CodeAnalysis.Text.TextSpan.Start
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.#ctor(System.Int32,System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.Intersection(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.IntersectsWith(System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.FromBounds(System.Int32,System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.Equals(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.Equals(System.Object)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.GetHashCode
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.CompareTo(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.ToString
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.IntersectsWith(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.Contains(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.OverlapsWith(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.Overlap(Microsoft.CodeAnalysis.Text.TextSpan)
---

---
method: Microsoft.CodeAnalysis.Text.TextSpan.Contains(System.Int32)
---

---
class: Microsoft.CodeAnalysis.Text.TextChangeRange
---

---
field: Microsoft.CodeAnalysis.Text.TextChangeRange.NoChanges
---

---
property: Microsoft.CodeAnalysis.Text.TextChangeRange.Span
---

---
property: Microsoft.CodeAnalysis.Text.TextChangeRange.NewLength
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeRange.#ctor(Microsoft.CodeAnalysis.Text.TextSpan,System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeRange.Collapse(System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.Text.TextChangeRange})
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeRange.GetHashCode
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeRange.Equals(Microsoft.CodeAnalysis.Text.TextChangeRange)
---

---
method: Microsoft.CodeAnalysis.Text.TextChangeRange.Equals(System.Object)
---

---
class: Microsoft.CodeAnalysis.Text.LinePositionSpan
---

---
property: Microsoft.CodeAnalysis.Text.LinePositionSpan.End
---

---
property: Microsoft.CodeAnalysis.Text.LinePositionSpan.Start
---

---
method: Microsoft.CodeAnalysis.Text.LinePositionSpan.#ctor(Microsoft.CodeAnalysis.Text.LinePosition,Microsoft.CodeAnalysis.Text.LinePosition)
---

---
method: Microsoft.CodeAnalysis.Text.LinePositionSpan.ToString
---

---
method: Microsoft.CodeAnalysis.Text.LinePositionSpan.Equals(Microsoft.CodeAnalysis.Text.LinePositionSpan)
---

---
method: Microsoft.CodeAnalysis.Text.LinePositionSpan.Equals(System.Object)
---

---
method: Microsoft.CodeAnalysis.Text.LinePositionSpan.GetHashCode
---

---
class: Microsoft.CodeAnalysis.Text.LinePosition
---

---
field: Microsoft.CodeAnalysis.Text.LinePosition.Zero
---

---
property: Microsoft.CodeAnalysis.Text.LinePosition.Line
---

---
property: Microsoft.CodeAnalysis.Text.LinePosition.Character
---

---
method: Microsoft.CodeAnalysis.Text.LinePosition.#ctor(System.Int32,System.Int32)
---

---
method: Microsoft.CodeAnalysis.Text.LinePosition.Equals(Microsoft.CodeAnalysis.Text.LinePosition)
---

---
method: Microsoft.CodeAnalysis.Text.LinePosition.CompareTo(Microsoft.CodeAnalysis.Text.LinePosition)
---

---
method: Microsoft.CodeAnalysis.Text.LinePosition.ToString
---

---
method: Microsoft.CodeAnalysis.Text.LinePosition.Equals(System.Object)
---

---
method: Microsoft.CodeAnalysis.Text.LinePosition.GetHashCode
---

---
class: Microsoft.CodeAnalysis.Text.SourceHashAlgorithm
---

