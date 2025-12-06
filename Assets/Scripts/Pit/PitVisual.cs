using UnityEngine;
using System.Collections.Generic;

public class PitVisual : MonoBehaviour
{
    [SerializeField] private RectTransform seedParent;
    [SerializeField] SeedVisualPool pool;
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
            activeSeeds.Add(seed);
        }

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
