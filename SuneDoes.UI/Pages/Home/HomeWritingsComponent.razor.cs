using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuneDoes.UI.Layout.Main;

namespace SuneDoes.UI.Pages.Home;

public partial class HomeWritingsComponent
{
    [CascadingParameter]
    public FileDownloader FileDownloader { get; set; }

    private void OnJohnAndAngelaPdfDownloadClicked(MouseEventArgs ev)
    {
        _ = FileDownloader.DownloadFileFromUrl("Angela and John - Forever.pdf", "docs/Angela-and-John-Forever.pdf");
    }
    private void OnSimrebogenPdfClicked(MouseEventArgs ev)
    {
        _ = FileDownloader.DownloadFileFromUrl("Simrebogen.pdf", "docs/Simrebogen.pdf");
    }

}
