using Backend.DataAbstraction;

namespace Backend.Database
{
  public class DatabaseConfiguration : IDatabaseConfiguration
  {
    public string DatabaseName { get; set; } = string.Empty;

    public string ConnectionString { get; set; } = string.Empty;
  }
}
