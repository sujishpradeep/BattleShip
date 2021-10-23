using BattleShipApi.Constants;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public interface IGameManager
    {
        public Board AddBoard(int gameID, int playerID, Color colorPreference);

        // Assumption - Vertical - Top to Bottom , Horizontal - Left To Right
        public BattleShip AddBattleShipToBoard(int boardID, BattleShipAllignment BattleShipAllignment, Cell StartingCell);

        public AttackResponse Attack(int boardID, Cell cell);

    }
}