
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


}
