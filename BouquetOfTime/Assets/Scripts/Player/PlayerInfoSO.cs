using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerInfo", menuName = "SO/PlayerInfo")]
public class PlayerInfoSO : ScriptableObject
{
    public Rigidbody rb;
    public Vector3 Normal;

    public bool Grounded;

    public CapsuleCollider currentCollider;
}
