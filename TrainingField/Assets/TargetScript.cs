using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public GameObject targetUi;
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
        var position = new Vector3(Random.Range(21.2f, 21.9f), Random.Range(9f, 9.4f), -0.55f);
        Instantiate(targetUi, position, transform.rotation);
        Destroy(gameObject);
    }
}
