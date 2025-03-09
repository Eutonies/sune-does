namespace SuneDoes.UI.Pages.Blocks.Model;

public record BlockWord(
    string Words,
    string? Color,
    bool IsBold,
    bool IsItalic,
    string? FontFamily,
    int? FontSize
    );
