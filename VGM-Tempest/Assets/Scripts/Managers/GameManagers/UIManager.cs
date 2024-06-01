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
    [SerializeField] private Text[] scoreFeedbackText;
    [SerializeField] private Text totalScoreText;

    [Header("Intro Text")]
    [SerializeField] private Text introText;
    [SerializeField] private float delayBetweenMessages;
    [SerializeField][TextArea(5, 10)] private string[] introMessages;
    public bool finishedIntro;

    public void ActivateStartCanvas(bool value) => startGameCanvas.SetActive(value);

    public void ActivatePlayerCanvas(bool  value) => playerCanvas.SetActive(value);

    public void ActivateEndCanvas(bool value) => endGameCanvas.SetActive(value);

    public void PlayScoreFeedbackAnimation(int player, int amount)
    {
        scoreFeedbackText[player].text = (amount > 0 ? "+" : "") + amount.ToString();
        scoreFeedbackText[player].GetComponent<Animator>().Play("Score");
    }

    public void UpdatePlayerScoreText(int player, int amount) => playerScoresText[player].text = amount.ToString("000");

    public void UpdateTotalScoreText(int amount) => totalScoreText.text = amount.ToString("0000");

    public IEnumerator PlayGameIntro()
    {
        ActivateStartCanvas(false);
        foreach (string message in introMessages)
        {
            StartCoroutine(TypeWriterEffect(introText, message));
            yield return new WaitForSeconds(delayBetweenMessages);
        }
        ActivatePlayerCanvas(true);
        introText.text = string.Empty;
        finishedIntro = true;
    }

    public IEnumerator ShowTotalScore(float score0, float score1, float totalScore)
    {
        float displayedScore0 = score0;
        float displayedScore1 = score1;
        float displayedTotalScore = 0;

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

        yield return new WaitForSeconds(4f);
        GameManager.Instance.RestartGame();
    }

    public IEnumerator TypeWriterEffect(Text text, string fullText)
    {
        string currentText;
        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            text.text = currentText;
            yield return new WaitForSeconds(0.06f);
        }
    }
}
