using GTRMod.Modules;
using RoR2;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor
{
    public static class GTRaptorBuffs
    {
        // armor buff gained during Dash
        public static BuffDef armorBuff;

        // Radar skill buff
        public static BuffDef TargetAcquired;
        public static void Init()
        {
            TargetAcquired = ScriptableObject.CreateInstance<BuffDef>();

            TargetAcquired.name = "bdTargetAcquired";

            TargetAcquired.buffColor = Color.green;

            TargetAcquired.canStack = false;

            TargetAcquired.isDebuff = true;

            TargetAcquired.iconSprite = GTRaptorAssets.ScannerBuffIcon;

            Content.AddBuffDef(TargetAcquired);
        }

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("GTRaptorArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
