using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.Blocks.Model;

namespace SuneDoes.UI.Pages.Blocks;

public partial class BlocksMapComponent
{

    [Parameter]
    public IReadOnlyCollection<BlocksChapter> AllChapters { get; set; }

    [Parameter]
    public Action<BlocksChapter?> OnChapterChanged { get; set; }

    private static readonly IReadOnlyCollection<MapChapterEntry> ChapterEntryDefinitions = new List<MapChapterEntry>
    {
        new MapChapterEntry(Chapter: null, "The Woods", 0),
        new MapChapterEntry(Chapter: null, "The Village", 1),
        new MapChapterEntry(Chapter: null, "The Whore", 2),
        new MapChapterEntry(Chapter: null, "The Mother & The Father", 3),
        new MapChapterEntry(Chapter: null, "The Red River", 4)
    };

    private IReadOnlyCollection<MapChapterEntry> MapChapterEntries = [];

    protected override void OnParametersSet()
    {
        if(!MapChapterEntries.Any())
        {
            var chapArr = AllChapters.ToArray();
            MapChapterEntries = ChapterEntryDefinitions
                .Select(en => en with
                {
                    Chapter = en.Index < chapArr.Length ? chapArr[en.Index] : null
                }).ToList();
        }
    }


    private record MapChapterEntry(
        BlocksChapter? Chapter,
        string Title,
        int Index
        )
    {
        public string HtmlId => $"sundo-blocks-map-chapter-{Index}";
    }

}
