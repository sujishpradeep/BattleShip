using System.ComponentModel.DataAnnotations;

namespace BattleShipApi.Models
{
    public record Cell
    {
        [Required]
        public int RowID { get; set; }
        [Required]
        public int ColumnID { get; set; }

        public Cell(int rowID, int columnID)
        {
            this.RowID = rowID;
            this.ColumnID = columnID;
        }

    }
}