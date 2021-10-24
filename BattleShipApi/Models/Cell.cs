namespace BattleShipApi.Models
{
    public record Cell
    {
        public int RowID { get; set; }
        public int ColumnID { get; set; }

        public Cell(int rowID, int columnID)
        {
            this.RowID = rowID;
            this.ColumnID = columnID;
        }

    }
}