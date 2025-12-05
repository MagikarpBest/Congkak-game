using UnityEngine;

public enum Player
{
    Player1,
    Player2
}

public class TurnManager : MonoBehaviour
{
    private Player currentPlayer = Player.Player1;

    public void SwitchTurn()
    {
        currentPlayer = (currentPlayer == Player.Player1) ? Player.Player2 : Player.Player1;
    }

    // Helpers
    public Player CurrentPlayer => currentPlayer;

}
