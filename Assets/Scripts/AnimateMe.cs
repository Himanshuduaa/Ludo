using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TypeOfAnimation
{
    ScaleUp
}
public class AnimateMe : MonoBehaviour
{
    [Range(0,1)]
    public float duration;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        this.gameObject.transform.DOScale(new Vector3(1, 1, 1), duration);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
