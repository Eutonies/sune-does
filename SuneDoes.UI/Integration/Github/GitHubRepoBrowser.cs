using Booktex.Domain.Util;
using Microsoft.Extensions.Options;
using Octokit;
using SuneDoes.UI.Configuration;
using System.IO.Compression;
using System.Text;

namespace SuneDoes.UI.Integration.Github;

public class GitHubRepoBrowser : IGitHubRepoBrowser
{
    private readonly IOptions<SuneDoesConfiguration> _configOption;

    public GitHubRepoBrowser(IOptions<SuneDoesConfiguration> configOption)
    {
        _configOption = configOption;
    }

    private SuneDoesConfiguration Config => _configOption.Value;


    public async Task<IReadOnlyCollection<GitHubRepoFile>> DownloadFilesWithEnding(string repoName, string fileEnding)
    {
        var octoClient = new GitHubClient(new ProductHeaderValue("sune-does"));
        octoClient.Credentials = new Credentials(Config.GitHubToken, AuthenticationType.Bearer);
        var repos = await octoClient.Repository.GetAllForCurrent();
        var relRepo = repos
            .First(_ => _.Name.ToLower().Contains(repoName.ToLower()));
        var archive = await octoClient.Repository.Content.GetArchive(relRepo.Id, ArchiveFormat.Zipball);
        using var byteStream = new MemoryStream(archive);
        using var zipArchive = new ZipArchive(byteStream);
        var returnee = zipArchive.Entries
            .Where(_ => _.Name.ToLower().Pipe(nam => nam.EndsWith(fileEnding.ToLower())))
            .Select(_ => (_.Name, _.FullName, Content: StreamToString(_)))
            .Select(_ => new GitHubRepoFile(
                RepoName: relRepo.Name,
                RepoId: relRepo.Id.ToString(),
                FullFileName: _.FullName,
                FileName: _.Name,
                FileContent: _.Content
                ))
            .ToList();
        return returnee;
    }


    private static string StreamToString(ZipArchiveEntry ent)
    {
        var byts = ReadStream(ent);
        var returnee = UTF8Encoding.UTF8.GetString(byts);
        return returnee;
    }

    private static byte[] ReadStream(ZipArchiveEntry ent)
    {
        using var readStream = ent.Open();
        using var buffer = new MemoryStream();
        readStream.CopyTo(buffer);
        var returnee = buffer.ToArray();
        return returnee;

    }


}
