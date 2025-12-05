using NUnit.Framework;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private int[] pits = new int[16];
    private int startingSeeds = 2;


    private void Start()
    {
        InitializeBoard();
    }
    private void InitializeBoard()
    {
        for (int i = 0; i < pits.Length; i++)
        {
            if (i == 7 || i == 15)
            {
                // Store pit starts empty
                pits[i] = 0;
                Debug.Log("BoardInitialized store");
            }
            else
            {
                // Normal pit
                pits[i] = startingSeeds;
                Debug.Log($"BoardInitialized normal pit { startingSeeds}");
            }

            Debug.Log($"pit {pits[i] = GetSeeds(i)}");
        }
        EventBus.OnBoardManagerReady?.Invoke();
    }

    public int GetSeeds(int pitIndex)
    {
        return pits[pitIndex];
    }

    public void AddSeeds(int pitIndex, int amount = 1)
    {
        pits[pitIndex] += amount;
    }

    public void RemoveAllSeeds(int pitIndex)
    {
        pits[pitIndex] = 0;
    }
}
