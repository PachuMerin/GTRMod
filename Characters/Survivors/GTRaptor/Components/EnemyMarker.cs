using GTRMod.Survivors.GTRaptor;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GTRMod.Survivors.GTRaptor.Components
{
    public class EnemyMarker : MonoBehaviour
    {
        /// <summary>
        /// Character that applied the mark.
        /// Useful later for smart bullets, achievements, etc.
        /// </summary>
        public CharacterBody scanner;

        /// <summary>
        /// Remaining lifetime of the mark.
        /// </summary>
        public float duration;

        private CharacterBody body;
        private Highlight highlight;
        private Renderer targetRenderer;
        private CharacterModel characterModel;
        private GameObject reticleInstance;
        private void Awake()
        {
            body = GetComponent<CharacterBody>();
            characterModel = GetComponentInChildren<CharacterModel>();
            // Cache the renderer once.
            CharacterModel model = GetComponentInChildren<CharacterModel>();

            if (model)
            {
                targetRenderer = model.GetComponentInChildren<Renderer>();
            }
        }

        /// <summary>
        /// Returns true if this enemy is currently marked by the scanner.
        /// </summary>
        public bool IsMarkedBy(CharacterBody attacker)
        {
            return scanner == attacker && duration > 0f;
        }
        /// <summery>
        /// Refreshes the mark duration
        /// </summery>
        public void Refresh(float duration)
        {
            this.duration = duration;
        }
        /// <summary>
        /// Called whenever an enemy is scanned.
        /// If already marked, simply refreshes the timer.
        /// </summary>
        public void Initialize(CharacterBody scannerBody, float markDuration)
        {
            scanner = scannerBody;
            duration = markDuration;

            CreateHighlight();
            CreateReticle();
        }
        private void CreateReticle()
        {
            if (reticleInstance)
                return;

            if (!GTRaptorAssets.TargetReticlePrefab)
                return;

            Transform target = body.coreTransform;

            reticleInstance = Object.Instantiate(
                GTRaptorAssets.TargetReticlePrefab,
                target);

            reticleInstance.transform.localPosition =
                new Vector3(0f, 2.2f, 0f);

            reticleInstance.transform.localRotation =
                Quaternion.identity;
        }
        private void FixedUpdate()
        {
            duration -= Time.fixedDeltaTime;

            if (duration <= 0f)
            {
                RemoveMarker();
                if (reticleInstance)
                {
                    Destroy(reticleInstance);
                }
            }
        }

        private void CreateHighlight()
        {
            if (!highlight)
            {
                highlight = GetComponent<Highlight>();

                if (!highlight)
                {
                    highlight = gameObject.AddComponent<Highlight>();
                }
            }

            if (targetRenderer)
            {
                highlight.targetRenderer = targetRenderer;
            }

            highlight.highlightColor = Highlight.HighlightColor.teleporter;
            highlight.strength = 1;
            highlight.isOn = true;
        }

        private void RemoveMarker()
        {
            if (highlight)
            {
                highlight.isOn = false;
            }

            if (body &&
                body.HasBuff(GTRaptorBuffs.TargetAcquired))
            {
                body.RemoveBuff(GTRaptorBuffs.TargetAcquired);
            }

            Destroy(this);
        }
    }
}