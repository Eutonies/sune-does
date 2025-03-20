using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartComponent
{
    [Parameter]
    public IReadOnlyCollection<BookChapterContent> Contents { get; set; }

    private IReadOnlyCollection<IReadOnlyCollection<BookChapterContent>> _splitParts = [[]];


    protected override void OnParametersSet()
    {
        SplitParts();
        _ = InvokeAsync(StateHasChanged);
    }


    private void SplitParts()
    {
        var result = new List<IReadOnlyCollection<BookChapterContent>>();
        var currentCount = 0;
        var current = new List<BookChapterContent>();
        foreach(var cont in Contents)
        {
            var newSection = false;
            var includeContent = true;
            if(cont is BookChapterSection || cont is BookContextBreak)
            {
                newSection = true;
                includeContent = false;
            }
            else
            {
                currentCount += CountLines(cont);
                if (currentCount > LinesPerPage)
                    newSection = true;
            }
            if (newSection)
            {
                if(current.Any())
                    result.Add(current);
                current = new List<BookChapterContent>();
                currentCount = 0;
            }
            if (includeContent)
                current.Add(cont);
        }
        if (current.Any())
            result.Add(current);

        _splitParts = result.ToReadonlyCollection();

    }

    private const int CharsPerLine = 40;
    private const int LinesPerPage = 50;

    private int CountLines(BookChapterContent cont) => cont switch
    {
        BookCharacterLine lin => lin.LineParts.Count + 1,
        BookCharacterStoryTime tim => (tim.Story
            .Length / CharsPerLine) + 4,
        BookDialog dia => dia.Entries
           .Select(ent => CountLines(ent.Line) + 1)
           .Sum() + 1,
        BookNarrationList lis => lis.Items.Count + 3,
        BookNarration narr => (narr.NarrationContent.Length / CharsPerLine) + 3,
        _ => 10
    };






}
