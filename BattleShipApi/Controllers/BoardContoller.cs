using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BattleShipApi.Constants;
using BattleShipApi.Managers;
using BattleShipApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BattleShipApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardManager> _logger;
        private readonly IBoardManager _boardManagerManager;

        public BoardController(
            ILogger<BoardManager> logger,
            IBoardManager BoardManagerManager
        )
        {
            _logger = logger;
            _boardManagerManager = BoardManagerManager;
        }
        [HttpPost("{gameID}/{playerID}/{colorPreference}")]
        public IActionResult AddBoard([Required] int gameID, [Required] int playerID, [Required] int colorPreference)
        {

            var AddBoardResult = _boardManagerManager.Add(gameID, playerID, (Color)colorPreference);

            if (AddBoardResult.IsError)
            {
                return BadRequest(AddBoardResult.ErrorMessage);
            }

            return Ok(AddBoardResult.Result);
        }


    }
}
