using BattleShipApi.Constants;
using BattleShipApi.Models;

namespace BattleShipApi.DTOs
{
    public class AttackResponseDTO
    {
        public AttackResponse AttackResponse { get; set; }
        public BoardState BoardState { get; set; }
    }
}