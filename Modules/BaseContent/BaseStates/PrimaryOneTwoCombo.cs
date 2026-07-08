using EntityStates;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GTRMod.Modules.BaseStates
{
    public abstract class PrimaryOneTwoCombo : BaseSkillState, SteppedSkillDef.IStepSetter
    {
        public static float damageCoefficient = 1.6f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.18f;
        public static float force = 50f;
        public static float range = 300f;
        public int ShootIndex;
        // Shared between casts
        private static bool fireLeft = false;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            FireBullet();
        }

        private void FireBullet()
        {
            string muzzle = fireLeft ? "GTRMuzzle.L" : "GTRMuzzle.R";

            PlayAnimation(
                "Gesture, Override",
                fireLeft ? "GTR-Kun_ShootingL" : "GTR-Kun_ShootingR",
                "ShootGun.playbackRate",
                duration);

            EffectManager.SimpleMuzzleFlash(
                LegacyResourcesAPI.Load<GameObject>(
                    "Prefabs/Effects/MuzzleFlashes/MuzzleflashPistol"),
                gameObject,
                muzzle,
                false);

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();

                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageCoefficient * damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    force = force,
                    maxDistance = range,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    muzzleName = muzzle,
                    procCoefficient = procCoefficient,
                    radius = 0.5f,
                    smartCollision = true
                }.Fire();
            }

            // Swap pistols for next shot
            fireLeft = !fireLeft;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }
        public void SetStep(int i)
        {
            ShootIndex = i;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}