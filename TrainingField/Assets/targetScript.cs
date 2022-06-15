using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class targetScript : MonoBehaviour
{
    public GameObject targetPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        var position = new Vector3(Random.Range(7.6f, 33.4f), Random.Range(6.6f, 10.6f), -0.55f );
        Instantiate(targetPrefab, position, transform.rotation);
        
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}   
