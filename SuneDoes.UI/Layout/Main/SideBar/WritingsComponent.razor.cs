using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SuneDoes.UI.Layout.Main.SideBar;

public partial class WritingsComponent
{
    [CascadingParameter]
    public FileDownloader FileDownloader { get; set; }


    private void OnPdfDownloadClicked(MouseEventArgs ev)
    {
        _ = FileDownloader.DownloadFileFromUrl("Angela and John - Forever.pdf", "docs/Angela-and-John-Forever.pdf");
    }


}
