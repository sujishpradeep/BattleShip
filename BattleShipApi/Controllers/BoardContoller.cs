using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BattleShipApi.Constants;
using BattleShipApi.DTOs;
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
        [HttpPost()]
        public IActionResult AddBoard(AddBoardDTO model)
        {
            // TODO: Authorize request if user claims has permission to add board in game.

            var AddBoardResult = _boardManagerManager.Add(model);

            if (AddBoardResult.IsError)
            {
                return BadRequest(AddBoardResult.ErrorMessage);
            }

            return Ok(AddBoardResult.Result);
        }

        [HttpPost("battleShip/{boardID}")]
        public IActionResult PlaceBattleShip(string boardID, BattleShipDTO model)
        {
            // TODO: Authorize requests user claims has permission place battleShip in board

            var placeBattleShipResult = _boardManagerManager.PlaceBattleShip(boardID, model);

            if (placeBattleShipResult.IsError)
            {
                return BadRequest(placeBattleShipResult.ErrorMessage);
            }

            return Ok(placeBattleShipResult.Result);
        }
        [HttpPost("attack/{boardID}")]
        public IActionResult Attack(string boardID, Cell model)
        {
            // TODO: Authorize requests user claims has permission attack the board

            var attackResultResult = _boardManagerManager.Attack(boardID, model);

            if (attackResultResult.IsError)
            {
                return BadRequest(attackResultResult.ErrorMessage);
            }

            return Ok(attackResultResult.Result);
        }


    }
}
