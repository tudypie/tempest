using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int[] score;
    [SerializeField] private int totalScore;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void AddScore(int playerNumber, int value = 1)
    {
        score[playerNumber] += value;
        gameManager.uiManager.UpdatePlayerScoreText(playerNumber, score[playerNumber]);
    }

    public void CalculateTotalScore()
    {
        totalScore = score[0] + score[1];
        gameManager.uiManager.UpdateTotalScoreText(totalScore);
        gameManager.uiManager.UpdatePlayerScoreText(0, score[0]);
        gameManager.uiManager.UpdatePlayerScoreText(1, score[1]);
        StartCoroutine(gameManager.uiManager.ShowTotalScore(score[0], score[1], totalScore));
    }
}
