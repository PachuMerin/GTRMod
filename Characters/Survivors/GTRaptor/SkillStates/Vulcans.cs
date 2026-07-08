using EntityStates;
using GTRMod.Characters.Survivors.GTRaptor.Components;
using GTRMod.Survivors.GTRaptor;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor.SkillStates
{
    public class Vulcans : BaseSkillState
    {
        public static float damageCoefficient = 2.4f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.18f;
        public static float recoil = 1.5f;
        public static float bloom = 0.8f;
        public static float force = 120f;

        // Assign during plugin initialization
        public static GameObject projectilePrefab;

        private float duration;

public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            VulcanContoller controller =
                characterBody.GetComponent<VulcanContoller>();

            bool fireLeft = controller.nextShotLeft;

            controller.nextShotLeft = !controller.nextShotLeft;

            string muzzleName = fireLeft? "GTRMuzzle.L": "GTRMuzzle.R";

            FireProjectile(muzzleName);

            AddRecoil(-recoil, -recoil * 2f, -recoil, recoil);

            characterBody.SetSpreadBloom(bloom);

            PlayAnimation("Gesture, Override, Shoot", fireLeft ? "GTR-Kun_ShootingL" : "GTR-Kun_ShootingR", "ShootGun.playbackRate",duration);
        }

        private void FireProjectile(string muzzleName)
        {
            Ray aimRay = GetAimRay();

            if (isAuthority)
            {
                ProjectileManager.instance.FireProjectile(
                    projectilePrefab,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(aimRay.direction),
                    gameObject,
                    damageCoefficient * damageStat,
                    force,
                    RollCrit(),
                    DamageColorIndex.Default,
                    null,
                    -1f
                );
            }

            EffectManager.SimpleMuzzleFlash(
                LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashMageFireLarge"),
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