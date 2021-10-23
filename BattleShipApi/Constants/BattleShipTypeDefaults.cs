using System.Collections.Generic;
using BattleShipApi.Models;

namespace BattleShipApi.Constants
{
    public static class BattleShipTypeDefaults
    {
        public static Dictionary<BattleShipType, BattleShipTypeProperty> battleShipProperties = new Dictionary<BattleShipType, BattleShipTypeProperty>
        {
            {BattleShipType.Carrier, new BattleShipTypeProperty(1)},
            {BattleShipType.BattleShip, new BattleShipTypeProperty(2)},
            {BattleShipType.Destroyer, new BattleShipTypeProperty(3)},
            {BattleShipType.Submarine, new BattleShipTypeProperty(4)},
            {BattleShipType.PatrolBat, new BattleShipTypeProperty(5)}
        };


        public static BattleShipTypeProperty GetBattleShipTypeProperty(BattleShipType battleShipType)
        {
            BattleShipTypeProperty battleShipTypeProperty;
            battleShipProperties.TryGetValue(battleShipType, out battleShipTypeProperty);
            return battleShipTypeProperty;
        }

    }

}