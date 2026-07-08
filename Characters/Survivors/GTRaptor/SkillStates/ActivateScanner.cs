using EntityStates;
using GTRMod.Survivors.GTRaptor;
using GTRMod.Survivors.GTRaptor.Components;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using GTRMod.Characters.Survivors.GTRaptor.Components;

namespace GTRMod.Survivors.GTRaptor.SkillStates
{
    public class ActivateScanner : BaseSkillState
    {
        public static float baseDuration = 0.8f;

        // How long enemies/interactables stay marked.
        public static float scanDuration = 10f;

        // Maximum scan range.
        // Set to Mathf.Infinity if you want the whole stage.
        public static float scanRadius = Mathf.Infinity;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            // Face the direction we're aiming while activating.
            StartAimMode(duration + 1f);

            // Stop sprinting.
            characterBody.SetAimTimer(duration);
            characterBody.isSprinting = false;

            // Animation
            PlayAnimation(
                "FullBody, Override, Scan",
                "Special",
                "Emote.playbackRate",
                duration);

            // Scanner sound
            Util.PlaySound("Play_item_use_radar", gameObject);

            if (isAuthority)
            {
                CombatScanner.Scan(
                    characterBody,
                    scanRadius,
                    scanDuration);
            }
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