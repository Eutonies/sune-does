using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartComponent
{
    [Parameter]
    public IReadOnlyCollection<BookChapterContent> Contents { get; set; }

    [Parameter]
    public SpaceFragmentSpecification Specification { get; set; }

    [Parameter]
    public Action Close { get; set; }

    private const int CharsPerLine = 100;
    private const int PageBreakMinLimit = 70;
    private const int PageBreakMaxLimit = 90;
    private const int PageBreakMiddleLimit = (PageBreakMinLimit + PageBreakMaxLimit) / 2;
    private int LinesUsedOnHeader => 
        30 + 
        (Specification.SubName == null ? 0 : 20);


    private string PartName => Specification.Name;
    private string? PartSubName => Specification.SubName;

    private IReadOnlyCollection<SplitPart> _splitParts = [];
    private int _selectedIndex = 0;

    private string FocusClassFor(SplitPart part) =>
        part.Index == _selectedIndex ?
          "page-focus" :
          "page-not-focus";

    private int ZIndexFor(SplitPart part) => //part.ZIndex;
        part.Index == _selectedIndex ? 1000 : (100 - part.Index);

    private void OnPageClicked(int index)
    {
        _selectedIndex = index;
        _ = InvokeAsync(StateHasChanged);
    }

    protected override void OnParametersSet()
    {
        SplitParts();
        _ = InvokeAsync(StateHasChanged);
    }


    private record SplitPart(IReadOnlyCollection<BookChapterContent> Contents, int Index)
    {
        public int ZIndex;
    }

    private void SplitParts()
    {
        var result = new List<IReadOnlyCollection<BookChapterContent>>();
        var currentCount = LinesUsedOnHeader;
        var current = new List<BookChapterContent>();
        foreach(var (cont, indx) in Contents.Select((_, index) => (_,index)))
        {
            var thisCount = CountLines(cont);
            var countWithThis = currentCount + thisCount;
            var textInThis = ""; // debugging
            if (cont is BookDialog dia)
                textInThis = dia.Entries
                    .Select(_ => _.Line.LineParts.Select(_ => _.PartText).MakeString("\n"))
                    .MakeString("\n");
            else if(cont is BookCharacterLine lin)
                textInThis = lin.LineParts.Select(_ => _.PartText).MakeString("\n");

            if(cont is BookContextBreak && !current.Any()) { }
            else if (!current.Any())
            {
                if(countWithThis <= PageBreakMiddleLimit)
                {
                    current.Add(cont);
                    currentCount = countWithThis;
                }
                else if (cont is BookDialog diag)
                {
                    var splitted = Split(diag, PageBreakMiddleLimit);
                    AddSplitted(splitted, result, ref current, ref currentCount);
                }
                else if (cont is BookCharacterLine lin)
                {
                    var splitted = Split(lin, PageBreakMiddleLimit);
                    AddSplitted(splitted, result, ref current, ref currentCount);
                }
                else if (cont is BookNarration narr)
                {
                    var splitted = Split(narr, PageBreakMiddleLimit);
                    AddSplitted(splitted, result, ref current, ref currentCount);
                }
                else
                {
                    current.Add(cont);
                    currentCount = countWithThis;
                }
            }
            else if (countWithThis <= PageBreakMinLimit)
            {
                current.Add(cont);
                currentCount = countWithThis;
            }
            else if(countWithThis <= PageBreakMaxLimit && countWithThis >= PageBreakMinLimit)
            {
                if(cont is BookContextBreak) { }
                else
                {
                    current.Add(cont);
                }
                result.Add(current);
                current = new List<BookChapterContent>();
                currentCount = 0;
            }
            else
            {
                var linesLeft = (countWithThis > PageBreakMiddleLimit ? PageBreakMaxLimit : PageBreakMiddleLimit)
                     - countWithThis;
                if (cont is BookDialog diag)
                {
                    var splitted = Split(diag, linesLeft);
                    AddSplitted(splitted, result, ref current, ref currentCount);
                }
                else if (cont is BookCharacterLine lin)
                {
                    var splitted = Split(lin, linesLeft);
                    AddSplitted(splitted, result, ref current, ref currentCount);
                }
                else if(cont is BookNarration narr)
                {
                    var splitted = Split(narr, linesLeft);
                    AddSplitted(splitted, result, ref current, ref currentCount);
                }
                else
                {
                    result.Add(current);
                    current = new List<BookChapterContent>();
                    if (cont is BookContextBreak) { }
                    else
                    {
                        current.Add(cont);
                    }
                    currentCount = thisCount;
                }
            }
        }
        if (current.Any())
            result.Add(current);

        _splitParts = result
            .Select((res, indx) => new SplitPart(res, indx) { ZIndex = result.Count - indx})
            .ToReadonlyCollection();

    }



    private int CountLines(BookChapterContent cont) => cont switch
    {
        BookCharacterLine lin => lin.LineParts.Sum(CountLines) + 1,
        BookCharacterStoryTime tim => CountLines(tim.Story) + 4,
        BookDialog dia => dia.Entries
           .Sum(CountLines),
        BookNarrationList lis => lis.Items.Count + 2,
        BookNarration narr => CountLines(narr.NarrationContent),
        BookChapterSection _ => 10,
        BookContextBreak _ => 5,
        BookQuote quot => quot.QuoteString
           .Split('\n')
           .Select(CountLines)
           .Sum() + 1,
        _ => 10
    };

    private int CountLines(BookDialogEntry entry) => CountLines(entry.Line);
    private int CountLines(BookCharacterLinePart part) => CountLines(part.PartText) + (part.Description?.Pipe(_ => 1) ?? 0);

    private int CountLines(string str) => (int) Math.Ceiling(
        str.Length / ((decimal)CharsPerLine)

        );

    private BookCharacterLine[] Split(BookCharacterLine line, int linesInFirst) =>
                Split(
            entry: line,
            contentsOf: _ => _.LineParts,
            countLines: CountLines,
            createEntry: entries => line with { LineParts = entries },
            linesInFirst);


    private BookDialog[] Split(BookDialog dialog, int linesInFirst) =>
        Split(
            entry: dialog,
            contentsOf: _ => _.Entries,
            countLines: CountLines,
            createEntry: entries => dialog with { Entries = entries },
            linesInFirst);

    private BookNarration[] Split(BookNarration narr, int linesInFirst) =>
        Split(
            entry: narr,
            contentsOf: narr => narr
               .NarrationContent
               .Split("."),
            countLines: str => str.Length / CharsPerLine + 1,
            createEntry: lines => narr with { NarrationContent = lines.MakeString(".") },
            linesInFirst: linesInFirst
               );
               
    private void AddSplitted<TEntry>(TEntry[] entries, List<IReadOnlyCollection<BookChapterContent>> finalResult, ref List<BookChapterContent> currentList, ref int countInCurrent) where TEntry : BookChapterContent
    {
        foreach (var first in entries.Take(1))
            currentList.Add(first);
        finalResult.Add(currentList);
        currentList = new List<BookChapterContent>();
        countInCurrent = 0;
        foreach (var rem in entries.Skip(1))
        {
            currentList.Add(rem);
            countInCurrent += CountLines(rem);
        }

    }


    private TEntry[] Split<TEntry, TPart>(
        TEntry entry, 
        Func<TEntry, IEnumerable<TPart>> contentsOf,
        Func<TPart, int> countLines,
        Func<IReadOnlyCollection<TPart>, TEntry> createEntry,
        int linesInFirst)
    {
        var returnee = new List<TEntry>();
        var remainingParts = new Queue<TPart>(contentsOf(entry));
        var currentParts = new List<TPart>();
        var limit = linesInFirst;
        var linesInCurrent = 0;
        while (remainingParts.Any())
        {
            var thisPart = remainingParts.Dequeue();
            var thisAsText = "";
            if (thisPart is BookDialogEntry ent)
                thisAsText = ent.Line.LineParts.Select(_ => _.PartText).MakeString("\n");
            var linesInPart = countLines(thisPart);
            var countWithThisPart = linesInCurrent + linesInPart;
            var didAdd = false;
            if (!currentParts.Any() || countWithThisPart <= limit)
            {
                currentParts.Add(thisPart);
                linesInCurrent = countWithThisPart;
                didAdd = true;
            }
            if (countWithThisPart > limit)
            {
                var inser = createEntry(currentParts.ToReadonlyCollection());
                returnee.Add(inser);
                currentParts = new List<TPart>();
                limit = PageBreakMiddleLimit;
                if (!didAdd)
                {
                    currentParts.Add(thisPart);
                    linesInCurrent = linesInPart;
                }
                else
                    linesInCurrent = 0;
            }
        }
        if (currentParts.Any())
        {
            var inser = createEntry(currentParts.ToReadonlyCollection());
            returnee.Add(inser);
        }
        return returnee.ToArray();
    }






}
