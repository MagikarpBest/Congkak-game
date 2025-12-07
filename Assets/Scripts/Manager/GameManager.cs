using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TurnManager turnManager;

    private bool isAnimating;

    private void OnEnable()
    {
        EventBus.OnPitClicked += TriggerPlayerTurnCoroutine;
    }

    private void OnDisable()
    {
        EventBus.OnPitClicked -= TriggerPlayerTurnCoroutine;
    }

    private void TriggerPlayerTurnCoroutine(int pitIndex)
    {
        // If animation havent finish block click
        if (isAnimating) return;

        StartCoroutine(PlayerTurn(pitIndex));
    }

    // Trigger play turn
    private IEnumerator PlayerTurn(int pitIndex)
    {
        isAnimating = true;

        Player currentPlayer = turnManager.CurrentPlayer;
        int seeds = boardManager.GetSeeds(pitIndex);

        if (!IsValidPit(pitIndex))
        {
            isAnimating = false;
            Debug.Log("INVALID PIT");
            yield break;
        }

        if (CheckGameOver())
        {
            isAnimating = false;
            Debug.Log("Game Over");
            yield break;
        }

        boardManager.RemoveAllSeeds(pitIndex);

        int currentIndex = pitIndex;

        while (seeds > 0)
        {
            // If over then just repeat
            currentIndex = (currentIndex + 1) % 16;

            // Skip opponent store
            if (turnManager.CurrentPlayer == Player.Player1 && currentIndex == 15)
            {
                continue;
            }
            if (turnManager.CurrentPlayer == Player.Player2 && currentIndex == 7)
            {
                continue;
            }

            boardManager.AddSeeds(currentIndex);
            seeds--;
            EventBus.OnBoardUpdated?.Invoke();
            yield return new WaitForSeconds(0.5f);
        }

        HandleLastSeed(currentIndex);

        isAnimating = false;
    }

    // Trigger condition based on where last seed lands
    private void HandleLastSeed(int lastIndex)
    {
        Player currentPlayer = turnManager.CurrentPlayer;

        // Extra turn if last seed lands in own store
        if ((currentPlayer == Player.Player1 && lastIndex == 7) || 
            (currentPlayer == Player.Player2 && lastIndex == 15)) 
        {
            Debug.Log($"{currentPlayer} gets an extra turn");
            EventBus.OnBoardUpdated?.Invoke();
            return;
        }

        // Last seed lands in empty pit on own side
        if ((currentPlayer == Player.Player1 && lastIndex >= 0 && lastIndex <= 6 && boardManager.GetSeeds(lastIndex) == 1) ||
            (currentPlayer == Player.Player2 && lastIndex >= 8 && lastIndex <= 14 && boardManager.GetSeeds(lastIndex) == 1))
        {
            int oppositeIndex = 14 - lastIndex;
            int storeIndex;
            if (currentPlayer == Player.Player1)
            {
                storeIndex = 7;
            }
            else
            {
                storeIndex = 15;
            }

            boardManager.AddSeeds(storeIndex, boardManager.GetSeeds(oppositeIndex) + 1);
            boardManager.RemoveAllSeeds(oppositeIndex);
            boardManager.RemoveAllSeeds(lastIndex);
            Debug.Log($"{currentPlayer} took seeds from pit {oppositeIndex}");
            EventBus.OnBoardUpdated?.Invoke();
        }
        // Switch turn
        EventBus.OnBoardUpdated?.Invoke();

        turnManager.SwitchTurn();
        Debug.Log($"Turn switched. Current player: {turnManager.CurrentPlayer}");
    }

    private bool CheckGameOver()
    {
        if (IsSideEmpty(Player.Player1) || IsSideEmpty(Player.Player2))
        {
            // Collect left over seeds to opponent store

            for (int i = 0; i < 16; i++)
            {
                // Skip store
                if (i == 7 || i == 15)
                {
                    continue;
                }

                if (i >= 0 && i <= 6 && !IsSideEmpty(Player.Player1))
                {
                    boardManager.AddSeeds(7, boardManager.GetSeeds(i));
                }
                else if (i >= 8 && i <= 14 && !IsSideEmpty(Player.Player2))
                {
                    boardManager.AddSeeds(15, boardManager.GetSeeds(i));
                }
                boardManager.RemoveAllSeeds(i);
            }

            EventBus.OnBoardUpdated?.Invoke();

            Debug.Log("Game Over!");
            int p1Score = boardManager.GetSeeds(7);
            int p2Score = boardManager.GetSeeds(15);

            Debug.Log($"Player1: {p1Score}, Player2: {p2Score}");
            if (p1Score > p2Score) Debug.Log("Player1 Wins!");
            else if (p2Score > p1Score) Debug.Log("Player2 Wins!");
            else Debug.Log("Draw!");

            return true;
        }
        
        return false;
    }

    // Condition for end game
    private bool IsSideEmpty(Player player)
    {
        int start = player == Player.Player1 ? 0 : 8;
        int end = player == Player.Player1 ? 6 : 14;

        // Loop through one side of player and check whether is all side empty
        for (int i = start; i < end; i++)
        {
            if (boardManager.GetSeeds(i) > 0)
            {
                return false;
            }
        }
        return true;
    }

    // Check is picked pit valid (Player 1 cant pick player 2 pit)
    private bool IsValidPit(int pitIndex)
    {
        Player currentPlayer = turnManager.CurrentPlayer;
        int seeds = boardManager.GetSeeds(pitIndex);

        // If player 1 then can only select pit bottom vice versa
        if (currentPlayer == Player.Player1 && pitIndex >= 0 && pitIndex <= 6 && seeds > 0)
        {
            return true;
        }

        if (currentPlayer == Player.Player2 && pitIndex >= 8 && pitIndex <= 14 && seeds > 0)
        {
            return true;
        }

        return false;
    }
}
