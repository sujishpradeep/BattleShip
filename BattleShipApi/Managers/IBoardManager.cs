using BattleShipApi.Constants;
using BattleShipApi.DTOs;
using BattleShipApi.Models;

namespace BattleShipApi.Managers
{
    public interface IBoardManager
    {
        public ResultDTO<Board> Add(int gameID, int playerID, Color colorPreference);

        public ResultDTO<BoardState> PlaceBattleShip(string boardID, BattleShipDTO battleShipDTO);

        public ResultDTO<AttackResponseDTO> Attack(string boardID, Cell cell);

    }
}