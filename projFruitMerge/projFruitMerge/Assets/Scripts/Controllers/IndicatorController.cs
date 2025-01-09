using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint
        (
            new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane)
        );

        Debug.Log(touchPosition.x);

        if (touchPosition.x >= -0.88 && touchPosition.x <= 0.88)
            transform.position = new Vector3(touchPosition.x, transform.position.y, transform.position.z);

        if (touch.phase == TouchPhase.Ended) gameObject.SetActive(false);
    }

}
