using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


    }
}
