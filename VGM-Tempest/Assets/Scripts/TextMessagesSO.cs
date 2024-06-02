using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TextMessage", menuName = "Scriptable Objects/TextMessages")]
public class TextMessagesSO : ScriptableObject
{
    [Serializable]
    public struct Message
    {
        public string text;
    }

    public Vector3 spawnOffset;
    public float delay;
    public Message[] messages;
}
