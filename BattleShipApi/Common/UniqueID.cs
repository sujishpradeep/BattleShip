using System;

namespace BattleShipApi.Common
{
    public static class UniqueID
    {
        public static string Generate()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}