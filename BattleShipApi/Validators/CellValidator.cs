using BattleShipApi.Models;

namespace BattleShipApi.Validators
{
    public static class CellValidator
    {
        public static bool IsValid(Cell cell, BoardState boardState)
        {
            if ((cell.RowID <= 0) || (cell.ColumnID <= 0))
            {
                return false;
            }

            if ((cell.RowID > boardState.Board.MaxRows) || (cell.ColumnID > boardState.Board.MaxRows))
            {
                return false;
            }

            return true;
        }


    }
}