using Backend.Domain.UserDomain;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using static User;
using Backend.DataAbstraction;
using MongoDB.Bson;

public class UserSerializer : IBsonSerializer<User>
{
  private readonly IHashingGenerator _hashingGenerator;
  public UserSerializer(IHashingGenerator hashingGenerator)
  {
    _hashingGenerator = hashingGenerator;
  }

  public Type ValueType => typeof(User);

  public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, User user)
  {
    var writer = context.Writer;
    writer.WriteStartDocument();

    writer.WriteName(AvailableFields.Id);
    if(user.Id == null)
    {
      user.Id = ObjectId.GenerateNewId().ToString();
    }
    writer.WriteString(user.Id);

    writer.WriteName(AvailableFields.Email);
    writer.WriteString(user.Email);

    var salt = _hashingGenerator.GenerateSalt();
    var hashed = _hashingGenerator.HashPassword(user.Password, salt);

    writer.WriteName(AvailableFields.Password);
    writer.WriteString(hashed);

    writer.WriteName(AvailableFields.Security.Root);
    writer.WriteStartDocument();

    writer.WriteName(AvailableFields.Security.Salt);
    writer.WriteString(salt);
    writer.WriteName(AvailableFields.Security.AccessToken);
    writer.WriteString(user.Security?.AccessToken ?? "");
    writer.WriteName(AvailableFields.Security.RefreshToken);
    writer.WriteString(user.Security?.RefreshToken ?? "");
    writer.WriteName(AvailableFields.Security.LoginRetryCount);
    writer.WriteInt32(user.Security?.LoginRetryCount ?? 0);
    writer.WriteEndDocument();

    writer.WriteEndDocument();
  }

  public User Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
  {
    var reader = context.Reader;

    reader.ReadStartDocument();

    string email = string.Empty;
    string password = string.Empty;
    string salt = string.Empty;
    string? accessToken = null;
    string? refreshToken = null;
    ushort retryCount = 0;
    string? id = null;

    while (reader.ReadBsonType() != MongoDB.Bson.BsonType.EndOfDocument)
    {
      var name = reader.ReadName();

      switch (name)
      {
        case AvailableFields.Id:
          id = reader.ReadString();
          break;
        case AvailableFields.Email:
          email = reader.ReadString();
          break;
        case AvailableFields.Password:
          password = reader.ReadString();
          break;
        case AvailableFields.Security.Root:
          reader.ReadStartDocument();
          while (reader.ReadBsonType() != MongoDB.Bson.BsonType.EndOfDocument)
          {
            var secName = reader.ReadName();
            switch (secName)
            {
              case AvailableFields.Security.Salt:
                salt = reader.ReadString();
                break;
              case AvailableFields.Security.AccessToken:
                accessToken = reader.ReadString();
                break;
              case AvailableFields.Security.RefreshToken:
                refreshToken = reader.ReadString();
                break;
              case AvailableFields.Security.LoginRetryCount:
                retryCount = (ushort)reader.ReadInt32();
                break;
              default:
                reader.SkipValue();
                break;
            }
          }
          reader.ReadEndDocument();
          break;
        default:
          reader.SkipValue();
          break;
      }
    }

    reader.ReadEndDocument();

    var user = new User
    {
      Email = email,
      Security = new Security
      {
        Salt = salt,
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        LoginRetryCount = retryCount
      }
    };

    typeof(User)
        .GetProperty(nameof(User.Password), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
        ?.SetValue(user, password);

    return user;
  }

  object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
      => Deserialize(context, args);

  void IBsonSerializer.Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
      => Serialize(context, args, (User)value);
}
