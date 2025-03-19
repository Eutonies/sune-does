namespace SuneDoes.UI.Integration.Github;

public record GitHubRepoFile(
    string RepoName,
    string RepoId,
    string FullFileName,
    string FileName,
    string FileContent
    )
{
}
