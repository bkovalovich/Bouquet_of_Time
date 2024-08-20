using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxObject : MonoBehaviour
{

    public float hitBoxTime = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, hitBoxTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
