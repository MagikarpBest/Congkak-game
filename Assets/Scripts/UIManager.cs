using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Button[] pits;
    [SerializeField] private TextMeshProUGUI[] seedsText;
    [SerializeField] private PitVisual[] pitVisuals;

    [SerializeField] private TextMeshProUGUI currentPlayerText;

    private FlashEffect flashEffect;
    private int[] previousSeedCounts;

    private void OnEnable()
    {
        EventBus.OnBoardUpdated += UpdateSeedsUI;
        EventBus.OnBoardManagerReady += InitializeUI;
    }

    private void OnDisable()
    {
        EventBus.OnBoardUpdated -= UpdateSeedsUI;
        EventBus.OnBoardManagerReady -= InitializeUI;
    }


    private void InitializeUI()
    {
        previousSeedCounts = new int[seedsText.Length];

        for (int i = 0; i < pits.Length; i++)
        {
            int index = i;
            if (i >= 7)
            {
                index += 1;
            }

            // Set click action for buttons. (If button 2 is clicked then do it stuff based on the assigned index)
            pits[i].onClick.AddListener(() => PitClicked(index));

            previousSeedCounts[i] = boardManager.GetSeeds(i);
            seedsText[i].text = $"{previousSeedCounts[i]}";
        }
        UpdateSeedsUI();
    }
    private void PitClicked(int pitIndex)
    {
        EventBus.OnPitClicked?.Invoke(pitIndex);
        Debug.Log($"Clicked pit {pitIndex}");
    }

    private void UpdateSeedsUI()
    {
        currentPlayerText.text = $"{turnManager.CurrentPlayer}";

        for (int i = 0; i < seedsText.Length; i++)
        {
            int currentCount = boardManager.GetSeeds(i);

            if (currentCount != previousSeedCounts[i])
            {
                seedsText[i].text = $"{currentCount}";
                seedsText[i].transform.DOPunchScale(transform.localScale * 0.3f, 0.3f);
                previousSeedCounts[i] = currentCount;
            }
        }

        if (pitVisuals != null)
        {
            for (int i = 0; i < pitVisuals.Length; i++)
            { 
                pitVisuals[i].SetVisualSeedCount(boardManager.GetSeeds(i));
            }
        }
    }
}
