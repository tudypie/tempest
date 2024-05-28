using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject startGameCanvas;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject endGameCanvas;

    public void ActivateStartCanvas(bool value) => startGameCanvas.SetActive(value);
    public void ActivatePlayerCanvas(bool  value) => playerCanvas.SetActive(value);
    public void ActivateEndCanvas(bool value) => endGameCanvas.SetActive(value);
}
