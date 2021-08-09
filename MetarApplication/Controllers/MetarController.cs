using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MetarApplication.Services;
using MetarApplication.Models;

namespace MetarApplication.Controllers
{
    [Route("metar")]
    [ApiController]
    public class MetarController : ControllerBase
    {
        private IMetarService _metarService;

        public MetarController(IMetarService metarService)
        {
            _metarService = metarService;
        }

        [HttpGet("ping")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string,string>))]
        public IActionResult Ping()
        {
            return Ok(new Dictionary<string,string>()
            {
                {
                    "data",
                    "pong"
                }
            });
        }

        [HttpGet("info")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Dictionary<string,string>))]
        public IActionResult Get([FromHeader] Headers headers,[FromQuery] string scode)
        {
            var result = _metarService.GetData(headers, scode);
            if(result==null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Dictionary<string,string>()
                {
                    {
                        "message",
                        "station code not found."
                    }
                });
            }
            return Ok(result);
        }
    }
}
