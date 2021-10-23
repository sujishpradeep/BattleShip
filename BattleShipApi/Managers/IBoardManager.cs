using BattleShipApi.Constants;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public interface IBoardManager
    {
        public ResultDTO<Board> Add(int gameID, int playerID, Color colorPreference);

        // Assumption - Vertical - Top to Bottom , Horizontal - Left To Right
        public ResultDTO<BoardState> PlaceBattleShip(string boardID, BattleShipType battleShipType, BattleShipAllignment BattleShipAllignment, Cell StartingCell);

        public AttackResponse Attack(string boardID, Cell cell);

    }
}