using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;

namespace MC_SVShipBPs
{
	[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
	public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.shipbps";
        public const string pluginName = "SV Ship Blueprints";
        public const string pluginVersion = "1.0.0";

		private static ConfigEntry<bool> includeDreads;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Main));			
			includeDreads = Config.Bind("Config",
				"Allow dreads?",
				false,
				"Allow dread bps?");
        }

        [HarmonyPatch(typeof(Blueprint), nameof(Blueprint.GetRandomBlueprint))]
        [HarmonyPostfix]
        private static void GetRandomBP_Post(ref Blueprint __result, int level, DropLevel dropLevel, int faction, Random rand)
        {
			Blueprint result = null;
			int num = rand.Next(1, 5);
			if (num == 1)
			{
				TWeapon randomWeapon = GameData.data.GetRandomWeapon(99f, 1, level, WeaponType.None, dropLevel, faction, 0, rand);
				result = new Blueprint(1, randomWeapon.index, 1f);
			}
			if (num == 2)
			{
				Equipment randomEquipment = EquipmentDB.GetRandomEquipment(0f, 99f, 1, level, -1, ShipClassLevel.Kraken, faction, false, dropLevel, 0, rand);
				result = new Blueprint(2, randomEquipment.id, 1f);
			}
			if (num == 3)
			{
				Item randomItem = ItemDB.GetRandomItem(level, true, rand);
				result = new Blueprint(3, randomItem.id, 1f);
			}
			if (num == 4)
			{
				int max = 6;
				if (includeDreads.Value)
					max = 7;

				ShipModelData randomModel = ShipDB.GetRandomModel(0, max, faction, level, true, false, true);
				result = new Blueprint(4, randomModel.id, 1f);
			}
			__result = result;
		}


    }
}
