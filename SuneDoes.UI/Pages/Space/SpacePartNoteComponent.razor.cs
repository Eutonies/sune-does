using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartNoteComponent
{
    [Parameter]
    public SpaceFragment Fragment { get; set; }


    [Parameter]
    public SpaceFragmentSpecification Specification { get; set; }

    [Parameter]
    public Action<SpaceFragmentSpecification, SpaceFragment> OnPartSelected { get; set; }


    private void OnClicked()
    {
        OnPartSelected(Specification, Fragment);
    }

}
