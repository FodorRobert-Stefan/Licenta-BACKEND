using MongoDB.Bson;

namespace Backend.DataAbstraction.Extensions
{
  public static class MongoDbValidObjectIdExtension
  {
    public static bool IsValidObjectId(this string id)
    {
      return ObjectId.TryParse(id, out _);
    }
  }
}
