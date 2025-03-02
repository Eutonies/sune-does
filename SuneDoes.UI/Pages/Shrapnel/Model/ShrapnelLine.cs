namespace SuneDoes.UI.Pages.Shrapnel.Model;

public record ShrapnelLine(
    string SaidBy,
    string Line,
    string? Description)
{
    public bool IsWill => SaidBy.ToLower().Contains("will");
    public bool IsJen => SaidBy.ToLower().Contains("jen");

}
