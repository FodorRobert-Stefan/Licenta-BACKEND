namespace Backend.DataAbstraction
{
  public interface IDatabaseConfiguration
  {
    string DatabaseName { get; }

    string ConnectionString { get; }
  }
}
