using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int[] score;
    [SerializeField] private int totalScore;
    [SerializeField] private int highscore;

    private GameManager gameManager;

    public bool IsHighscore { get; private set; }

    private void Awake()
    {
        highscore = PlayerPrefs.GetInt("Highscore");
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void AddScore(int playerNumber, int value = 1)
    {
        score[playerNumber] += value;
        gameManager.uiManager.UpdatePlayerScoreText(playerNumber, score[playerNumber]);
        gameManager.uiManager.PlayScoreFeedbackAnimation(playerNumber, value);
    }

    public void CalculateTotalScore()
    {
        totalScore = score[0] + score[1];
        if(totalScore > highscore) {
            PlayerPrefs.SetInt("Highscore", totalScore);
            highscore = PlayerPrefs.GetInt("Highscore");
            IsHighscore = true;
        }
        StartCoroutine(gameManager.uiManager.ShowTotalScore(score[0], score[1], totalScore));
    }

    public void ResetScore()
    {
        score[0] = 0;
        score[1] = 0;
        totalScore = 0;
        gameManager.uiManager.UpdatePlayerScoreText(0, score[0]);
        gameManager.uiManager.UpdatePlayerScoreText(1, score[1]);
    }
}
