using BattleShipApi.Constants;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public interface IGameManager
    {
        public ResultDTO<Board> AddBoard(int gameID, int playerID, Color colorPreference);

        // Assumption - Vertical - Top to Bottom , Horizontal - Left To Right
        public ResultDTO<BoardState> PlaceBattleShipToBoard(string boardID, BattleShipType battleShipType, BattleShipAllignment BattleShipAllignment, Cell StartingCell);

        public AttackResponse Attack(string boardID, Cell cell);

    }
}