using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HttpStatuses.Models;
using System.Threading.Tasks;
using HttpStatuses.Repository;

namespace HttpStatuses.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class statusController : ControllerBase
  {
    private readonly ILogger<statusController> _logger;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;

    public statusController(IMongoUnitOfWork mongoUnitOfWork, ILogger<statusController> logger)
    {
      _logger = logger;
      _mongoUnitOfWork = mongoUnitOfWork;
    }

    [HttpGet]
    public ObjectResult Get()
    {
      
      Status status = new Status()
      {
        Code = 401,
        Name = "Unauthorized",
        Description = "Unauthorized!"
      };

      var repo = _mongoUnitOfWork.GetRepository<Status>();
      repo.AddAsync(status);

      return Ok(status);
    }
  }
}