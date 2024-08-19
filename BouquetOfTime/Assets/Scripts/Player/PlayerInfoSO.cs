using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerInfo", menuName = "SO/PlayerInfo")]
public class PlayerInfoSO : ScriptableObject
{
    public int playerNumber;
    public Rigidbody rb;
    public Camera camera;
    public Vector3 Normal;

    public bool Grounded;

    public CapsuleCollider currentCollider;
}
