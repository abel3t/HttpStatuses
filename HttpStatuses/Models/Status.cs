using MongoDB.Bson.Serialization.Attributes;

namespace HttpStatuses.Models
{
  public class Status
  {
    [BsonElement("Name")]
    public string Name { get; set; }
    [BsonElement("Code")]
    public int Code { get; set; }
    [BsonElement("Description")]
    public string Description { get; set; }
  }
}