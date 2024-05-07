using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerNumber = 0;
    public Material wireframeMaterial;
    public Material[] playerMaterial;
    public Transform[] movePoint;

    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnPlayerJoin(Movement movement)
    {
        movement.playerNumber = playerNumber;
        movement.wireframeMaterial = wireframeMaterial;
        movement.playerMaterial = playerMaterial[playerNumber];
        movement.playerMesh.material = playerMaterial[playerNumber];
        playerNumber++;
    }
}
