using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaivem : MonoBehaviour
{
    [SerializeField] private float velocidade;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.z > 20 || transform.position.z < 10) velocidade *=-1;
        transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z + velocidade * Time.deltaTime);
    }
}
