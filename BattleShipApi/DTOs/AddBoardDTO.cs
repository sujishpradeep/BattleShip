using System.ComponentModel.DataAnnotations;
using BattleShipApi.Constants;

namespace BattleShipApi.DTOs
{
    public class AddBoardDTO
    {
        [Required]
        public int gameID { get; set; }
        [Required]
        public int playerID { get; set; }
        [Required]
        public Color colorPreference { get; set; }
    }
}