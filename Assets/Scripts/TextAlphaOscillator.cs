using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAlphaOscillator : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float minAlpha;
    [SerializeField] private float speed;
    private CanvasGroup canvasGroup;
    private float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        if(angle > 360) angle=0;
        var aux = (Mathf.Sin(Mathf.Deg2Rad * angle)+1)/2;
        canvasGroup.alpha = minAlpha + (1-minAlpha) * aux;
    }
}
