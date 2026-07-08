using GTRMod.Modules;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GTRMod.Survivors.GTRaptor
{
    public static class GTRaptorAssets
    {

        // particle effects
            public static GameObject bombExplosionEffect;
            public static GameObject ImpactBlazeBullet;
            public static GameObject RadarPulseEffect;

        //Scanner effects
            public static GameObject TargetReticlePrefab;
            public static Sprite ScannerBuffIcon;

        // networked hit sounds
        //public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
            public static GameObject BlazeBulletPrefab;
            private static AssetBundle _GTRaptorAssetBundle;

        //projectile stats
            private const float BlazeBulletSpeed = 240f;
            private const float BlazeBulletLifetime = 8f;

        //projectile ghosts
            public static GameObject BlazeBulletGhostPrefab;

        #region Asset Loading
        public static void Init(AssetBundle assetBundle)
        {
            _GTRaptorAssetBundle = assetBundle;
            TargetReticlePrefab =
            _GTRaptorAssetBundle.LoadAsset<GameObject>("prefabTargetReticle");
            ScannerBuffIcon =
            _GTRaptorAssetBundle.LoadAsset<Sprite>("texGTRSpecialicon");
            BlazeBulletGhostPrefab =
            _GTRaptorAssetBundle.CreateProjectileGhostPrefab("mdlGTRBlazeBullet");
            ImpactBlazeBullet =
            _GTRaptorAssetBundle.LoadAsset<GameObject>("ImpactBlazeBullet");
            RadarPulseEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Scanner/OmniEffect.prefab").WaitForCompletion();
            CreateEffects();
            CreateProjectiles();
        }
        #endregion

        #region effects
        private static void CreateEffects()
        {
            CreateExplosionEffect();

            ImpactBlazeBullet =
                _GTRaptorAssetBundle.LoadEffect("ImpactBlazeBullet", true);
        }

        private static void CreateExplosionEffect()
        {
            bombExplosionEffect = _GTRaptorAssetBundle.LoadEffect("ExplosionEffect", "ImpactBlazeBullet");

        }
        #endregion effects

        #region projectile
        private static void CreateProjectiles()
        {
            BlazeBulletPrefab = Asset.CloneProjectilePrefab(
               "FMJRamping",
               "GTRBlazeBullet");

            ProjectileController controller =
                BlazeBulletPrefab.GetComponent<ProjectileController>();

            ProjectileSimple simple =
                BlazeBulletPrefab.GetComponent<ProjectileSimple>();

            ProjectileDamage damage =
                BlazeBulletPrefab.GetComponent<ProjectileDamage>();

            ProjectileImpactExplosion impact =
                BlazeBulletPrefab.GetComponent<ProjectileImpactExplosion>();

            // Ghost Model
                controller.ghostPrefab = BlazeBulletGhostPrefab;

            // Speed
                simple.desiredForwardSpeed = BlazeBulletSpeed;
                simple.lifetime = BlazeBulletLifetime;

            // Damage
                damage.damageType = DamageType.Generic;

            // Impact Effect
            if (impact)
            {
                impact.impactEffect = ImpactBlazeBullet;
                impact.destroyOnEnemy = true;
                impact.destroyOnWorld = true;
            }

            ContentAddition.AddProjectile(BlazeBulletPrefab);
        }
        #endregion projectile
    }
}