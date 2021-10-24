using BattleShipApi.Models;

namespace BattleShipApi.DTOs
{
    public record BattleShipDTO
    {
        public int BattleShipType { get; set; }
        public int BattleShipAllignment { get; set; }
        public Cell StartingCell { get; set; }
    }
}