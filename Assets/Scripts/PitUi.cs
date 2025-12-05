using System;
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

    [SerializeField] private TextMeshProUGUI currentPlayerText;

    private Player currentPlayer;

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
        currentPlayer = turnManager.CurrentPlayer;
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
            int index = i;

            seedsText[i].text = $"{boardManager.GetSeeds(index)}";
        }
    }

}
