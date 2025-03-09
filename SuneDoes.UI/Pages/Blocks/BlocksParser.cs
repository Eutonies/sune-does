using SuneDoes.UI.Pages.Blocks.Model;
using System.Text.Json;

namespace SuneDoes.UI.Pages.Blocks;

public static class BlocksParser
{

    public static IReadOnlyCollection<BlockWord> ParseBlockWords(string blocksFolder)
    {
        var fileContents = Directory.GetFiles(blocksFolder)
            .Where(_ => _.ToLower().EndsWith(".words"))
            .Select(File.ReadAllText)
            .ToList();

        var returnee = fileContents
            .SelectMany(cont => JsonSerializer.Deserialize<IReadOnlyCollection<BlockWord>>(cont)!)
            .DistinctBy(_ => _.Words)
            .ToList();
        return returnee;
    }

}
