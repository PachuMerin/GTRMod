using EntityStates;
using GTRMod.Characters.Survivors.GTRaptor.Components;
using GTRMod.Survivors.GTRaptor;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor.SkillStates
{
    public class Blasting : BaseSkillState
    {
        public static float damageCoefficient = 1.2f;
        public static float force = 120f;
        public static float baseDuration = 0.8f;

        // Number of pellets
        public static int pelletCount = 6;

        // Total spread angle in degrees
        public static float spreadAngle = 10f;

        public static float recoil = 6f;
        public static float bloom = 4f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            FireShotgunBlast();

            AddRecoil(-recoil, -recoil * 2f, -recoil, recoil);
            characterBody.SetSpreadBloom(bloom);

            PlayAnimation("Gesture, Override, Shoot","GTR-Kun_Blasting","ShootGun.playbackRate",duration);
        }

        private void FireShotgunBlast()
        {
            Ray aimRay = GetAimRay();

            if (!isAuthority)
                return;
            int leftPellets = pelletCount / 2;
            int rightPellets = pelletCount - leftPellets;

            FireFromMuzzle("MuzzleLeft", leftPellets);
            FireFromMuzzle("MuzzleRight", rightPellets);

            for (int i = 0; i < pelletCount; i++)
            {
                Vector3 direction = Util.ApplySpread(
                    aimRay.direction,
                    0f,
                    spreadAngle,
                    1f,
                    1f);

                ProjectileManager.instance.FireProjectile(
                    GTRaptorAssets.BlazeBulletPrefab,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(direction),
                    gameObject,
                    damageCoefficient * damageStat,
                    force,
                    RollCrit());
            }

            EffectManager.SimpleMuzzleFlash(
                LegacyResourcesAPI.Load<GameObject>(
                    "Prefabs/Effects/MuzzleFlashes/MuzzleflashMageFireLarge"),
                gameObject,"MuzzleShotgun",false);
        }
        private void FireFromMuzzle(string muzzleName, int pelletCount)
        {
            ChildLocator childLocator = GetModelChildLocator();

            if (!childLocator)
                return;

            Transform muzzle = childLocator.FindChild(muzzleName);

            if (!muzzle)
                return;

            for (int i = 0; i < pelletCount; i++)
            {
                Vector3 direction = Util.ApplySpread(
                    GetAimRay().direction,
                    0f,
                    spreadAngle,
                    1f,
                    1f);

                ProjectileManager.instance.FireProjectile(
                     GTRaptorAssets.BlazeBulletPrefab,
                    muzzle.position,
                    Util.QuaternionSafeLookRotation(direction),
                    gameObject,
                    damageCoefficient * damageStat,
                    force,
                    RollCrit());
            }

            EffectManager.SimpleMuzzleFlash(
                LegacyResourcesAPI.Load<GameObject>(
                    "Prefabs/Effects/MuzzleFlashes/MuzzleflashMageFireLarge"),
                gameObject,
                muzzleName,
                false);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}