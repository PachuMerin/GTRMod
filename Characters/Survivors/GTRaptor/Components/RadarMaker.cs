using GTRMod.Survivors.GTRaptor;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GTRMod.Survivors.GTRaptor.Components
{
    //marks interactables for the scanner skill
    public class RadarMarker : MonoBehaviour
    {
        public float duration;
        private Highlight highlight;
        private Renderer targetRenderer;

        private void Awake()
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        public void Initialize(float markDuration)
        {
            duration = markDuration;

            CreateHighlight();
        }

        private void FixedUpdate()
        {
            duration -= Time.fixedDeltaTime;

            if (duration <= 0f)
            {
                RemoveMarker();
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

            // Same color used by interactables in vanilla
            highlight.highlightColor = Highlight.HighlightColor.interactive;
            highlight.strength = 1;
            highlight.isOn = true;
        }
        public void Refresh(float markDuration)
        {
            duration = markDuration;

            if (highlight)
            {
                highlight.isOn = true;
            }
        }

        private void RemoveMarker()
        {
            if (highlight)
            {
                highlight.isOn = false;
            }

            Destroy(this);
        }
    }
}