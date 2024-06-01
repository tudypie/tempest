using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TextMessage", menuName = "Scriptable Objects/TextMessages")]
public class TextMessagesSO : ScriptableObject
{
    [Serializable]
    public struct Message
    {
        [TextArea(5, 10)] public string text;
        public int lineSpawnPoint;
        public Vector3 spawnOffset;
        public float delay;
    }

    public Message[] messages;
}
