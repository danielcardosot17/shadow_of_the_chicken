using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateUpDown : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHeight, minHeight;
    private float radius, center, angle;

    private void Start() {
        center = (maxHeight + minHeight)/2;
        radius = (maxHeight - minHeight)/2;
        angle = Mathf.Asin((transform.position.y - center)/radius);
    }

    void FixedUpdate()
    {
        angle += Mathf.Deg2Rad * speed * Time.deltaTime;
        if(angle > Mathf.Deg2Rad * 360) angle=0;
        var posY = center + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}
