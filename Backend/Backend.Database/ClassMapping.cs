using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

public static class MongoClassMapRegistration
{
  private static bool _registered = false;

  public static void RegisterAll()
  {
    if (_registered) return;
    _registered = true;
    if (BsonClassMap.IsClassMapRegistered(typeof(User)) == false)
    {
      BsonClassMap.RegisterClassMap<User>(m =>
      {
        m.AutoMap();

        m.MapIdMember(mi => mi.Id)
          .SetIdGenerator(new StringObjectIdGenerator())
          .SetSerializer(new StringSerializer(BsonType.ObjectId));
        m.SetIgnoreExtraElements(true);
      });
    }
  }

}
