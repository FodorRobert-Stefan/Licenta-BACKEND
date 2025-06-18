using MongoDB.Bson.Serialization.Attributes;

namespace Backend.DataAbstraction
{
  public interface IDatabaseEntityRepresentation 
  {
    public string? Id { get; }
  }
}
