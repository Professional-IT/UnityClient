using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySocketIO;

public class GameOverPanel : MonoBehaviour
{
    public GameObject Board;
    public TextMeshProUGUI WinnerText;
    public GameAudio GameAudio;

    private Animator gameOverPanelAnimator;
    SocketIOController socket;

    private void Awake()
    {
        gameOverPanelAnimator = GetComponent<Animator>();
        socket = SocketIOController.instance;
    }

    public void SetWinnerText(PawnColor winnerPawnColor)
    {
        WinnerText.text = winnerPawnColor.ToString().ToUpper() + " WINS";
    }

    public void DisableBoard()
    {
        Board.SetActive(false);
    }

    public void ReturnToMenu()
    {
        gameOverPanelAnimator.SetTrigger("ReturnToMenu");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        socket.Emit("leaveRoom");
    }

    public void FadeGameMusic()
    {
        GameAudio.FadeGameMusic();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}