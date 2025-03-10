using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.Blocks.Model;

namespace SuneDoes.UI.Pages.Blocks;

public partial class BlocksPageComponent
{

    [Parameter]
    public int Order { get; set; }

    [Parameter]
    public int Index { get; set; }

    [Parameter]
    public IReadOnlyCollection<BlocksContent> Content { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public int PageNumber { get; set; }

    [Parameter]
    public int TotalNumberOfPages { get; set; }

    [Parameter]
    public Action<int>? OnClickLeft { get; set; }

    [Parameter]
    public Action<int>? OnClickRight { get; set; }

    private bool _showText = false;
    private Task? _activationTask;

    protected override void OnParametersSet()
    {
    }
    protected override Task OnInitializedAsync()
    {
        if (_activationTask == null)
            _activationTask = Task.Run(async () => 
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                _showText = true;
                await InvokeAsync(StateHasChanged);
                _activationTask = null;
            });
        return base.OnInitializedAsync();
    }

}
