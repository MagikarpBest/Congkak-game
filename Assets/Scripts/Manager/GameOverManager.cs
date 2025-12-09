using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] CircleTransition transition;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        restartButton.onClick.AddListener(() => StartCoroutine(RestartButton()));
        mainMenuButton.onClick.AddListener(() => StartCoroutine(MainMenuButton()));
    }

    private IEnumerator MainMenuButton()
    {
        yield return StartCoroutine(transition.GoingInTransition());
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator RestartButton()
    {
        yield return StartCoroutine(transition.GoingInTransition());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
