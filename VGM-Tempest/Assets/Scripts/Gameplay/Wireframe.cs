using UnityEngine;

public class Wireframe : MonoBehaviour
{
    public Transform[] lines;
    public Material material;

    public static Wireframe Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetLineMaterial(int index, Material material)
    {
        lines[index].parent.GetComponent<Renderer>().material = material;
    }

    public void ResetLineMaterial(int index)
    {
        lines[index].parent.GetComponent<Renderer>().material = material;
    }
}
