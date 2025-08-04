namespace Tamro.Madam.Models.Configuration;

public class DatabaseSettings : IDatabaseSettings
{
    public string Madam { get; set; }
    public string E1Gateway { get; set; }
    public string WhRawLt { get; set; }
    public string E1 { get; set; }
    public string Jira { get; set; }
}
