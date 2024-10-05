using Microsoft.JSInterop;

namespace SuneDoes.UI.Layout.Main;

public record FileDownloader(IJSRuntime Js) 
{
    public async Task DownloadFile(string fileName, byte[] data)
    {
        var streamRef = new DotNetStreamReference(new MemoryStream(data));
        await Js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    public async Task DownloadFileFromUrl(string fileName, string url)
    {
        await Js.InvokeVoidAsync("downloadFileFromUrl", fileName, url);
    }




}
