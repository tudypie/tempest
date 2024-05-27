using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    [SerializeField] private Material[] colors;

    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
        int rnd = Random.Range(0, colors.Length);
        mr.material = colors[rnd];

        transform.rotation = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);
    }
}
