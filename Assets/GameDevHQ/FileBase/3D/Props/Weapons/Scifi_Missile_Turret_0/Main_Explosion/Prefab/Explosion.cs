using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(cleanUp());

    }
    // should probably add pooling to this.
    IEnumerator cleanUp() 
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    
}
