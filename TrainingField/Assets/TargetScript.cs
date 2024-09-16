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
        var targetCollider = GetComponent<Collider>();
        var bounds = targetCollider.bounds;
        var radius = bounds.size.x / 2;
        var player = FindObjectOfType<PlayerMovement>();

        if (Camera.main != null)
        {
            var camTransform = Camera.main.transform;
            RaycastHit[] hit = Physics.RaycastAll(camTransform.position, camTransform.forward);
            RaycastHit exact = System.Array.Find(hit, e => e.collider.gameObject.layer == LayerMask.NameToLayer("Target"));

            var dist = Vector3.Distance(exact.point, bounds.center);

            int points;
            if (dist > radius)
            {
                points = 1;
            }
            else if (dist / radius > 2f / 3f)
            {
                points = 3;
            }
            else if (dist / radius > 1f / 6f)
            {
                points = 5;
            }
            else
            {
                points = 10;
            }
            if (player != null)
            {
                player.IncrementScore(points);
            }
        }
        var position = new Vector3(Random.Range(5f, 8.2f), Random.Range(6f, 8f), -0.55f);
        Destroy(gameObject);
        
        TimerImg.fillAmount = 1;
        TimerImg.Rebuild(CanvasUpdate.Prelayout);
        Instantiate(targetUi, position, transform.rotation);
    }
}
