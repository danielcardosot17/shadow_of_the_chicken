using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignTargetToGround : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreLayer;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
    }
}
