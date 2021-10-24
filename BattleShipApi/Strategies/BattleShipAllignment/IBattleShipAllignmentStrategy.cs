using System.Collections.Generic;
using BattleShipApi.Constants;
using BattleShipApi.Models;

namespace BattleShipApi.Strategies
{
    public interface IBattleShipAllignmentStrategy
    {
        public BattleShipAllignment GetBattleShipAllignment();
        public bool CheckIfBoardWillOverFlowWhenShipIsAdded(Board board, BattleShipType battleShipType, Cell startingCell);
        public List<Cell> AddBattleShipToCells(BattleShip battleShip, Cell startingCell);
    }
}