using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BattleShipApi.Constants;
using BattleShipApi.Models;

namespace BattleShipApi.DTOs
{
    public record BattleShipDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(BattleShipType), ErrorMessage = "Battleship type is invalid")]
        [Required(ErrorMessage = "Battleship type is required")]
        public BattleShipType? BattleShipType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(BattleShipAllignment), ErrorMessage = "Battleship allignment is invalid")]

        [Required(ErrorMessage = "Battleship allignment is required")]
        public BattleShipAllignment? BattleShipAllignment { get; set; }

        [Required(ErrorMessage = "Battleship starting cell is required")]

        public Cell StartingCell { get; set; }
    }
}