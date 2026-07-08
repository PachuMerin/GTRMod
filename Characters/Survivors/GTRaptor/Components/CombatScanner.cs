using GTRMod.Survivors.GTRaptor;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GTRMod.Survivors.GTRaptor.Components
{
    //activate scanner skill
        public static class CombatScanner
        {
            public static void Scan(
                CharacterBody scanner,
                float radius,
                float duration)
            {
                if (!scanner)
                    return;

                ScanInteractables(scanner, radius, duration);

                ScanEnemies(scanner, radius, duration);

                SpawnRadarPulse(scanner);
        }
        //mark interactables
            private static void ScanInteractables(
                CharacterBody scanner,
                float radius,
                float duration)
            {
            foreach (PurchaseInteraction purchase in
                     InstanceTracker.GetInstancesList<PurchaseInteraction>())
            {
                if (!purchase)
                    continue;

                float distance = Vector3.Distance(
                    scanner.corePosition,
                    purchase.transform.position);

                if (distance > radius)
                    continue;

                RadarMarker marker = purchase.GetComponent<RadarMarker>();

                if (!marker)
                {
                    marker = purchase.gameObject.AddComponent<RadarMarker>();
                    marker.Initialize(duration);
                }
                else
                {
                    marker.Refresh(duration);
                }
            }
        }
        //mark enemies
            private static void ScanEnemies(
                 CharacterBody scanner,
                float radius,
                float duration)
        {
            foreach (CharacterBody body in CharacterBody.readOnlyInstancesList)
            {
                if (!body)
                    continue;

                if (!body.healthComponent)
                    continue;

                if (!body.teamComponent)
                    continue;

                // Don't mark teammates
                if (body.teamComponent.teamIndex ==
                    scanner.teamComponent.teamIndex)
                    continue;

                float distance = Vector3.Distance(
                    scanner.corePosition,
                    body.corePosition);

                if (distance > radius)
                    continue;

                body.AddTimedBuff(
                    GTRaptorBuffs.TargetAcquired,
                    duration);

                EnemyMarker marker = body.GetComponent<EnemyMarker>();

                if (!marker)
                {
                    marker = body.gameObject.AddComponent<EnemyMarker>();
                    marker.Initialize(scanner, duration);
                }
                else
                {
                    marker.Refresh(duration);
                }
            }
        }


        private static void SpawnRadarPulse(CharacterBody scanner)
        {
            if (!scanner || !GTRaptorAssets.RadarPulseEffect)
                return;

            EffectManager.SpawnEffect(
                GTRaptorAssets.RadarPulseEffect,
                new EffectData
                {
                    origin = scanner.corePosition,
                    scale = 1f
                },
                true);
        }
    }
}