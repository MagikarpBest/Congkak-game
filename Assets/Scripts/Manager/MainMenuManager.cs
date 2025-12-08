using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CircleTransition transition;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        StartCoroutine(transition.GoingOutTransition());
        Initialize();
    }

    private void Initialize()
    {
        startButton.onClick.AddListener(() => StartCoroutine(StartButton()));
        quitButton.onClick.AddListener(()=> Application.Quit());
        SoundManager.PlayMusic(SoundType.MAINMUSIC);
    }

    private IEnumerator StartButton()
    {
        yield return StartCoroutine(transition.GoingInTransition());
        SceneManager.LoadScene("GameScene");
    }
}
