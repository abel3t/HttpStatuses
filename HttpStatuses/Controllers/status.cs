using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HttpStatuses.Models;
using HttpStatuses.Repository;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace HttpStatuses.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StatusController : ControllerBase
  {
    private readonly ILogger<StatusController> _logger;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public StatusController(IMongoUnitOfWork mongoUnitOfWork, ILogger<StatusController> logger)
    {
      _logger = logger;
      _mongoUnitOfWork = mongoUnitOfWork;
    }

    [HttpGet]
    public ObjectResult Get()
    {
      var repo = _mongoUnitOfWork.GetRepository<Status>();
      var pipeline = new BsonDocument[]
      {
        new BsonDocument{ { "$match", new BsonDocument("code", 400) } }
      };

      var result = repo.Aggregate(pipeline).Select(p
        => BsonSerializer.Deserialize<Status>(p));
      
      return Ok(result);
    }
  }
}