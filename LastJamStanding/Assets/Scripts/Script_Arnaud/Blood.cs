using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public float bloodTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitingForDestruction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitingForDestruction()
    {
        yield return new WaitForSeconds(bloodTime);
        Destroy(this.gameObject);
    }
}
