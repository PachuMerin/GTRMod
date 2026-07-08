using GTRMod.Survivors.GTRaptor;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using GTRMod.Characters.Survivors.GTRaptor;

using GTRMod.Characters.Survivors.GTRaptor.Components;
namespace GTRMod.Modules

{
    public static class DamageHooks
    {
        // 20% bonus damage
        public const float ScannerDamageMultiplier = 1.20f;

        public static void Init()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private static void HealthComponent_TakeDamage(
            On.RoR2.HealthComponent.orig_TakeDamage orig,
            HealthComponent self,
            DamageInfo damageInfo)
        {
            if (self &&
                self.body &&
                damageInfo.attacker)
            {
                CharacterBody attackerBody =
                    damageInfo.attacker.GetComponent<CharacterBody>();

                if (attackerBody)
                {
                    EnemyMarker marker =
                        self.body.GetComponent<EnemyMarker>();

                    if (marker &&
                        marker.IsMarkedBy(attackerBody))
                    {
                        damageInfo.damage *= ScannerDamageMultiplier;
                    }
                }
            }

            orig(self, damageInfo);
        }
    }
}
