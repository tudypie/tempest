using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject startGameCanvas;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private Text[] playerScoresText;
    [SerializeField] private Text totalScoreText;

    public float displayedScore0;
    public float displayedScore1;
    public float displayedTotalScore;

    public void ActivateStartCanvas(bool value) => startGameCanvas.SetActive(value);

    public void ActivatePlayerCanvas(bool  value) => playerCanvas.SetActive(value);

    public void ActivateEndCanvas(bool value) => endGameCanvas.SetActive(value);

    public void UpdatePlayerScoreText(int player, int amount) => playerScoresText[player].text = amount.ToString("000");

    public void UpdateTotalScoreText(int amount) => totalScoreText.text = amount.ToString("0000");

    public IEnumerator ShowTotalScore(float score0, float score1, float totalScore)
    {
        displayedScore0 = score0;
        displayedScore1 = score1;
        displayedTotalScore = 0;

        while(displayedTotalScore < totalScore)
        {
            displayedScore0 -= Time.deltaTime * (score0 / 10f);
            displayedScore1 -= Time.deltaTime * (score1 / 10f);
            displayedTotalScore += Time.deltaTime * (totalScore / 10f);
            UpdateTotalScoreText((int)displayedTotalScore);
            UpdatePlayerScoreText(0, (int)displayedScore0);
            UpdatePlayerScoreText(1, (int)displayedScore1);
            yield return new WaitForEndOfFrame();
        }

        displayedScore0 = 0;
        displayedScore1 = 0;
        displayedTotalScore = totalScore;
        UpdateTotalScoreText((int)displayedTotalScore);
        UpdatePlayerScoreText(0, (int)displayedScore0);
        UpdatePlayerScoreText(1, (int)displayedScore1);

        yield return new WaitForSeconds(2f);
        GameManager.Instance.RestartGame();
    }
}
