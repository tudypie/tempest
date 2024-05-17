using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text3D : MonoBehaviour
{
    [TextArea(3, 5)]
    [SerializeField] private string[] text;
    [SerializeField] private GameObject textPrefab;
    private int index = 0;
    int point = 0;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnText), 0f, 10f);
    }

    private void SpawnText()
    {
        GameObject newText = Instantiate(textPrefab, transform);
        //newText.transform.position = new Vector3(0, 0, 0);
        newText.GetComponent<Text>().text = text[index];
        index++;
        point++;
    }
}
