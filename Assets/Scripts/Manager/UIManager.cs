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
    [SerializeField] private CircleTransition transition;

    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private int[] previousSeedCounts;

    private void Start()
    {
        StartCoroutine(transition.GoingOutTransition());
    }
    private void OnEnable()
    {
        EventBus.OnBoardUpdated += UpdateSeedsUI;
        EventBus.OnTurnUpdated += UpdatePlayerTurn;
        EventBus.OnBoardManagerReady += InitializeUI;
        EventBus.OnGameOver += ShowGameover;
    }

    private void OnDisable()
    {
        EventBus.OnBoardUpdated -= UpdateSeedsUI;
        EventBus.OnTurnUpdated -= UpdatePlayerTurn;
        EventBus.OnBoardManagerReady -= InitializeUI;
        EventBus.OnGameOver -= ShowGameover;
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
        }

        for (int i = 0; i < seedsText.Length; i++)
        {
            previousSeedCounts[i] = boardManager.GetSeeds(i);
            seedsText[i].text = $"{previousSeedCounts[i]}";
        }
        currentPlayerText.text = $"{turnManager.CurrentPlayer} turn";
        UpdateSeedsUI();
    }

    private void PitClicked(int pitIndex)
    {
        EventBus.OnPitClicked?.Invoke(pitIndex);
        Debug.Log($"Clicked pit {pitIndex}");
    }

    private void UpdatePlayerTurn(bool turnUpdated)
    {
        // If get extra turn then dont update
        // Else update show whose next
        if (turnUpdated)
        {
            currentPlayerText.transform.DOPunchScale(transform.localScale * 0.3f, 0.3f);
            currentPlayerText.text = $"{turnManager.NextPlayerCheck()} Turn";
        }
        else
        {
            currentPlayerText.transform.DOPunchScale(transform.localScale * 0.3f, 0.3f);
            currentPlayerText.text = $"{turnManager.CurrentPlayer} Extra Turn!";
        }
    }

    private void UpdateSeedsUI()
    {
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

    private void ShowGameover(int playerWon)
    {
        gameOverPopup.SetActive(true);

        if (playerWon == 1)
        {
            gameOverText.text = $"Player 1 Win!";
        }
        else if (playerWon == 2)
        {
            gameOverText.text = $"Player 2 Win!";
        }
        else if (playerWon == 3)
        {
            gameOverText.text = $"Draw!";
        }
    }
}
