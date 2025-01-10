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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint
            (
                new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane)
            );

            float realX;
            if (touchPosition.x < -0.88) realX = -0.8f;
            else if (touchPosition.x > 0.88) realX = 0.8f;
            else realX = touchPosition.x;

            transform.position = new Vector3(realX, transform.position.y, transform.position.z);

            if (touch.phase == TouchPhase.Ended) gameObject.SetActive(false);
        }
    }

}
