using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BattleShipApi.Constants;

namespace BattleShipApi.DTOs
{
    public class AddBoardDTO
    {
        [Required(ErrorMessage = "GameID is required")]
        public int? gameID { get; set; }
        [Required(ErrorMessage = "PlayerID is required")]
        public int? playerID { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(Color), ErrorMessage = "Color is invalid")]
        [Required(ErrorMessage = "Color preference is is required")]
        public Color? colorPreference { get; set; }
    }
}