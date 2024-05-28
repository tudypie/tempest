using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Serializable]
    public struct PlayerScore
    {
        public int scoreValue;
        public Text scoreText;
    }

    [SerializeField] private PlayerScore[] score;

    public void AddScore(int playerNumber, int value = 1)
    {
        score[playerNumber].scoreValue += value;
        score[playerNumber].scoreText.text = score[playerNumber].scoreValue.ToString("00");
    }
}
