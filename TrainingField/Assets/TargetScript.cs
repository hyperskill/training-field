using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    public GameObject targetUi;
    public Image TimerImg;
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
        var position = new Vector3(Random.Range(5f, 8.2f), Random.Range(6f, 8f), -0.55f);
        Destroy(gameObject);
        TimerImg.fillAmount = 1;
        TimerImg.Rebuild(CanvasUpdate.Prelayout);
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.IncrementScore();
        }
        
        Instantiate(targetUi, position, transform.rotation);
    }
}
