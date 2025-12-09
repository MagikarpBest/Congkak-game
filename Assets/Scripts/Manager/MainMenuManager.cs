using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CircleTransition transition;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] GameObject creditPopup;
    [SerializeField] private Button backButton;

    private void Start()
    {
        StartCoroutine(transition.GoingOutTransition());
        Initialize();
    }

    private void Initialize()
    {
        startButton.onClick.AddListener(() => StartCoroutine(StartButton()));
        quitButton.onClick.AddListener(()=> Application.Quit());
        creditButton.onClick.AddListener(() => creditPopup.SetActive(true));
        backButton.onClick.AddListener(() => creditPopup.SetActive(false));
        SoundManager.PlayMusic(SoundType.MAINMUSIC);
    }

    private IEnumerator StartButton()
    {
        yield return StartCoroutine(transition.GoingInTransition());
        SceneManager.LoadScene("GameScene");
    }
}
