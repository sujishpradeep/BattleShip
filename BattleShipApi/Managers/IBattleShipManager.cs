using BattleShipApi.Constants;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public interface IBattleShipManager
    {

        public bool CheckIfBoardWillOverFlowWhenShipIsAdded(Board board, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell);
        public BattleShip Setup(string boardID, BattleShipType battleShipType, BattleShipAllignment battleShipAllignment, Cell startingCell);
        public bool WillShipsOverlap(BoardState boardState, BattleShip battleShip);
    }
}