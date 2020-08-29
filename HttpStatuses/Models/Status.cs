using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HttpStatuses.Models
{
  public class Status
  {
    public Status(string name, int code, string description)
    {
      Name = name;
      Code = code;
      Description = description;
    }
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("code")]
    public int Code { get; set; }
    [BsonElement("description")]
    public string Description { get; set; }
  }
}