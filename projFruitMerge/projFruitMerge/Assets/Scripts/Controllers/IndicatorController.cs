using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    [SerializeField] Canvas Canvas;
    [SerializeField] GameController gameController;

    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        graphicRaycaster = Canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && gameController.OnGame)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint
            (
                new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane)
            );

            if (!IsTouchingUIElement(touch))
            {
                float realX;
                if (touchPosition.x < -0.88) realX = -0.8f;
                else if (touchPosition.x > 0.88) realX = 0.8f;
                else realX = touchPosition.x;

                transform.position = new Vector3(realX, transform.position.y, transform.position.z);

                if (touch.phase == TouchPhase.Ended) gameObject.SetActive(false);
            }
        }
    }

    private bool IsTouchingUIElement(Touch touch)
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = touch.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0) return true;

        return false;
    }

}
