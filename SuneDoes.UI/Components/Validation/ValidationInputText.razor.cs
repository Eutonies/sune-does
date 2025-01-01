using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Components.Validation;

public partial class ValidationInputText
{
    private static object _idLockObject = new { };
    private static long _currentId = 0;
    private static long NextId()
    {
        lock(_idLockObject)
            return ++_currentId;
    }
    private readonly long _inputId = NextId();
    private bool _isInitialized = false;
    private string ElementId => $"sundo-validation-input-text-{_inputId}";

    [Parameter]
    public ValidationState State { get; set; }

    [Parameter]
    public string? ValidationInfo { get; set; }

    [Parameter]
    public string Label { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public string InputType { get; set; }

    [Parameter]
    public Func<string?, (ValidationState State, string? ValidationInfo)>? OnUpdate { get; set; }


    [Parameter]
    public Func<string?, Task<(ValidationState State, string? ValidationInfo)>>? OnUpdateAsync { get; set; }

    private string? _currentText;
    private string? CurrentText { 
        get => _currentText; 
        set 
        { 
            _currentText = value;
            if (OnUpdate != null)
            {

                (State, ValidationInfo) = OnUpdate(_currentText);
                _ = InvokeAsync(StateHasChanged);
            }
            else if (OnUpdateAsync != null)
                _ = Task.Run(async () => 
                    { 
                        (State, ValidationInfo) = await OnUpdateAsync(_currentText);
                        _ = InvokeAsync(StateHasChanged);
                    });
        } 
    }

    private string InputClass => $"sundo-component-validation-input-text {(State == ValidationState.Invalid || State == ValidationState.Info ? "sundo-validation-infoed" : "")} {StateClass}";

    private string StateClass => State switch
    {
        ValidationState.Valid => "sundo-validation-valid",
        ValidationState.Invalid => "sundo-validation-invalid",
        ValidationState.Info => "sundo-validation-unvalid",
        _ => ""
    };

    protected override void OnParametersSet()
    {
        if(!_isInitialized)
        {
            CurrentText = Text;
            _isInitialized = true;
        }
    }




}
