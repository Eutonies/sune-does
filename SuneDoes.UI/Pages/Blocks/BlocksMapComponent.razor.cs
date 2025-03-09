using SuneDoes.UI.Pages.Blocks.Model;

namespace SuneDoes.UI.Pages.Blocks;

public partial class BlocksMapComponent
{

    private static readonly IReadOnlyCollection<MapChapterEntry> Chapters = new List<MapChapterEntry>
    {
        new MapChapterEntry(Chapter: null, "The Woods", 0),
        new MapChapterEntry(Chapter: null, "The Village", 1),
        new MapChapterEntry(Chapter: null, "The Whore", 2),
        new MapChapterEntry(Chapter: null, "The Mother & The Father", 3),
        new MapChapterEntry(Chapter: null, "The Red River", 4)
    };



    private record MapChapterEntry(
        BlocksChapter? Chapter,
        string Title,
        int Index
        )
    {
        public string HtmlId => $"sundo-blocks-map-chapter-{Index}";
    }

}
