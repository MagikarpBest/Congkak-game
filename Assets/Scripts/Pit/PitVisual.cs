using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PitVisual : MonoBehaviour
{
    [SerializeField] private RectTransform seedParent;
    [SerializeField] private SeedVisualPool pool;
    [SerializeField] private bool isStore = false;

    private int previousSeedCount = 7;
    private List<GameObject> activeSeeds = new List<GameObject>();

    public void SetVisualSeedCount(int amount)
    {
        // Remove extras
        while (activeSeeds.Count > amount)
        {
            var seed = activeSeeds[activeSeeds.Count - 1];
            pool.Return(seed); // Return extras back to the pool
            activeSeeds.RemoveAt(activeSeeds.Count - 1);
        }

        // Add missing
        while (activeSeeds.Count < amount)
        {
            var seed = pool.Get();

            seed.transform.SetParent(seedParent);
            if (isStore)
            {
                seed.transform.SetSiblingIndex(2);
            }
            else
            {
                seed.transform.SetSiblingIndex(1);
            }

            FlashEffect flashEffect = seed.GetComponent<FlashEffect>();
            if (flashEffect != null)
            {
                flashEffect.CallDamageFlash();
            }

            if (amount > previousSeedCount)
            {
                SoundManager.PlaySound(SoundType.SEEDSPAWN);
            }

            seed.transform.DOPunchScale(transform.localScale * 0.6f, 0.3f);
            activeSeeds.Add(seed);
        }

        previousSeedCount = amount;
        LayoutSeeds();
    }

    private void LayoutSeeds()
    {
        int count = activeSeeds.Count;
        if (count == 0) return;

        float halfWidth = seedParent.rect.width / 2;
        float halfHeight = seedParent.rect.height / 2;

        // Use smaller dimension to calculate radius
        float radius = Mathf.Min(halfWidth, halfHeight) * 0.8f;

        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;

            Vector2 position = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            activeSeeds[i].transform.localPosition = position * radius;
        }
    }
}
