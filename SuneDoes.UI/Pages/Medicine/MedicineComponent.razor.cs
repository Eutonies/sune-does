
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Medicine;

public partial class MedicineComponent
{
    [Parameter]
    public string MedicineName { get; set; }

    [Parameter]
    public RenderFragment MedicineDescription { get; set; }

    [Parameter]
    public RenderFragment Suggestion { get; set; }

    [Parameter]
    public string? ImageFile { get; set; }

    [Parameter]
    public MedicineTemperature Temperature { get; set; } = MedicineTemperature.Hot;

    [Parameter]
    public Action<double> OnNotifyButtonClicked { get; set; }

    private string TemperatureClass => Temperature switch
    {
        MedicineTemperature.Burning => "sundo-medicine-component-burning",
        _ => "sundo-medicine-component-hot"
    };

    public enum MedicineTemperature
    {
        Burning = 1,
        Hot = 5
    }


}
