namespace SuneDoes.UI.Integration.Github;

public interface IGitHubRepoBrowser
{
    Task<IReadOnlyCollection<GitHubRepoFile>> DownloadFilesWithEnding(string repoName, string fileEnding);

}
