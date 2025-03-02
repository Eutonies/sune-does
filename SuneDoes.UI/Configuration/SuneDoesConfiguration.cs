using SuneDoes.UI.Configuration;

namespace SuneDoes.UI.Configuration;

public class SuneDoesConfiguration {
    public string? HostingBasePath { get; set; }
    public SuneDoesDbConfiguration Db { get; set; }

    public SuneDoesEmailConfiguration Email { get; set; }
    public string ShrapnelFolder { get; set; }
}