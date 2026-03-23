using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterCetrainAmountOfTime : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
