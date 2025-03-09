
using Malarkey.Abstractions.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Blocks.Model;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Blocks;

public partial class BlocksPage
{

    [Inject]
    public IOptions<SuneDoesConfiguration> Config { get; set; }

    private IReadOnlyCollection<BlocksChapter> AllChapters = [];
    private BlocksChapter? _currentChapter;

    private IReadOnlyCollection<ContentPage> Pages = [];

    protected override Task OnInitializedAsync()
    {
        AllChapters = BlocksParser.LoadChapters(Config.Value.BlocksFolder);
        return base.OnInitializedAsync();
    }


    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null)
        {
            SessionState.SelectedPage = SessionSelectedPage.Blocks;
        }
        return base.OnParametersSetAsync();
    }

    private void OnChapterSelectionChanged(BlocksChapter? chap)
    {
        _currentChapter = chap;
        if(chap != null)
           SplitToPages();
        InvokeAsync(StateHasChanged);
    }

    private const int LinesPerPage = 20;
    private const int WordsPerLine = 15;
    private const int ExtraLinesPerList = 2;
    private const int TotalWordsPerPage = LinesPerPage * WordsPerLine;
    private const int TotalExtraWordsPerList = ExtraLinesPerList * WordsPerLine;

    private Action<int>? ClickLeftHandlerFor(ContentPage page)
    {
        if (page.Index == Pages.Min(_ => _.Index))
            return null;
        var returnee = (int indx) =>
        {
            var allPages = Pages
               .OrderBy(_ => _.Index)
               .ToList();
            var next = allPages
               .Where(_ => _.Index >= indx - 1)
               .ToList();
            next.AddRange(allPages.Where(_ => _.Index < indx -1));
            Pages = next;
            InvokeAsync(StateHasChanged);
        };
        return returnee;
    }

    private Action<int>? ClickRightHandlerFor(ContentPage page)
    {
        if (page.Index == Pages.Max(_ => _.Index))
            return null;
        var returnee = (int indx) =>
        {
            var allPages = Pages
               .OrderBy(_ => _.Index)
               .ToList();
            var next = allPages
               .Where(_ => _.Index > indx)
               .ToList();
            next.AddRange(allPages.Where(_ => _.Index <= indx));
            Pages = next;
            InvokeAsync(StateHasChanged);
        };
        return returnee;
    }


    private void SplitToPages()
    {
        if(_currentChapter == null)
        {
            Pages = [];
            return;
        }
        var nextPages = new List<ContentPage>();
        var contents = new List<BlocksContent>();
        var wordsOnCurrentPage = 0;
        string? title = _currentChapter.Title;
        foreach(var cont in _currentChapter.Content)
        {
            contents.Add(cont);
            if(cont is BlocksListContent lis)
            {
                wordsOnCurrentPage += TotalExtraWordsPerList;
                wordsOnCurrentPage += lis.Items.SelectMany(it => it.Content.Split(' ')).Count();
            }
            else if(cont is BlocksTextContent tex)
            {
                wordsOnCurrentPage += tex.Contents.SelectMany(_ => _.StringContent().Split(' ')).Count();
            }
            else
            {
                wordsOnCurrentPage += WordsPerLine;
            }
            if(wordsOnCurrentPage > TotalWordsPerPage)
            {
                var addee = new ContentPage(nextPages.Count, contents.ToArray(), title);
                nextPages.Add(addee);
                contents.Clear();
                title = null;
                wordsOnCurrentPage = 0;
            }
        }
        if (contents.Any())
        {
            var addee = new ContentPage(nextPages.Count, contents.ToArray(), title);
            nextPages.Add(addee);
            contents.Clear();
        }
        Pages = nextPages;
    }



    private record ContentPage(
        int Index,
        IReadOnlyCollection<BlocksContent> Content,
        string? Title
        );


}
