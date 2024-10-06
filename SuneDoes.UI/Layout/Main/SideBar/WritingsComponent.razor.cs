using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SuneDoes.UI.Layout.Main.SideBar;

public partial class WritingsComponent
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
