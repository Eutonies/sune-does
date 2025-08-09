using Microsoft.AspNetCore.Components;
using System.Text;

namespace SuneDoes.UI.Components;

public partial class UpdatedComponent
{
    public string? Style { get; set; }

    [Parameter]
    public DateTime UpdateDate { get; set; }
    private static long _currentId = 0;
    private static object _idLock = new object();
    private static long NextId()
    {
        lock (_idLock)
            return ++_currentId;
    }

    private readonly long _id = NextId();
    private string _stringId => $"sundo-updated-{_id}";


    private IReadOnlyCollection<string>? _texts;
    private IReadOnlyCollection<string> Texts => _texts ??= ["Updated", UpdateDate.ToString("yyyy-MM-dd")];


}
