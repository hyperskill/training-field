using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class targetScript : MonoBehaviour
{
    public GameObject targetPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //print("new target");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        var targetCollider = GetComponent<Collider>();
        var bounds = targetCollider.bounds;
        var radius = bounds.size.x / 2; //My targets are always parallel to the x-y plane

        if (Camera.main != null)
        {
            var camTransform = Camera.main.transform;
            RaycastHit[] hit = Physics.RaycastAll(camTransform.position, camTransform.forward);
            RaycastHit exact = Array.Find(hit, e => e.collider.gameObject.layer == LayerMask.NameToLayer("Target"));

            var dist = Vector3.Distance(exact.point, bounds.center);
            
            int points;
            if (dist > radius)
            {
                points = 1;
            }else if(dist/radius>2f/3f)
            {
                points = 3;
            }else if(dist/radius>1f/6f)
            {
                points = 5;
            }else
            {
                points = 10;
            }

            var player = FindObjectOfType<PlayerScript>();
            if (player != null)
            {
                player.IncrementScore(points);
            }
            
        }

        var position = new Vector3(Random.Range(7.6f, 33.4f), Random.Range(6.6f, 10.6f), -0.55f );
        Instantiate(targetPrefab, position, transform.rotation);

        ;
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}   
