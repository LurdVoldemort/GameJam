using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_trail : MonoBehaviour
{
    public float lifetime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
