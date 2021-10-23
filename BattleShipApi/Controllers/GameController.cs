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
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameManager _gameManager;

        public GameController(
            ILogger<GameController> logger,
            IGameManager gameManager
        )
        {
            _logger = logger;
            _gameManager = gameManager;
        }
        [HttpPost("board/{gameID}/{playerID}/{colorPreference}")]
        public IActionResult AddBoard([Required] int gameID, [Required] int playerID, [Required] int colorPreference)
        {

            var AddBoardResult = _gameManager.AddBoard(gameID, playerID, (Color)colorPreference);

            if (AddBoardResult.IsError)
            {
                return BadRequest(AddBoardResult.ErrorMessage);
            }

            return Ok(AddBoardResult.Result);
        }


    }
}
