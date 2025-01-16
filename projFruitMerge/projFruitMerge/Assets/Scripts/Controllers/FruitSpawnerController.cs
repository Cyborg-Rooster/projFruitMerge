using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class FruitSpawnerController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject IndicatorController;
    [SerializeField] GameObject HeightController;

    [SerializeField] GameObject Renderer;

    [SerializeField] CameraShakeController CameraShakeController;
    [SerializeField] GameController GameController;

    [SerializeField] GameObject Canvas;

    [SerializeField] System.Collections.Generic.List<GameObject> Fruits;
    [SerializeField] System.Collections.Generic.List<Sprite> SpritesFruits;

    [Header("Height Limiter Options")]
    [SerializeField] Vector2 StartPosition;
    [SerializeField] LayerMask LayerMask;
    [SerializeField] float Distance;
    [SerializeField] float CollisionTime;

    RaycastManager RaycastManager;
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    int nextFruitIndex;
    bool fruitDropped = false;

    private void Start()
    {
        graphicRaycaster = Canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        IndicatorController.SetActive(true);
        var go = Instantiate(RandomManager.GetRandomObject<GameObject>(Fruits), transform);
        var fc = go.GetComponent<FruitController>();

        fc.SetObjectsFromParent(CameraShakeController, GameController, Canvas);

        nextFruitIndex = RandomManager.GetRandomIndex(SpritesFruits.Count);
        UIManager.SetImage(Renderer, SpritesFruits[nextFruitIndex]);

        RaycastManager = new RaycastManager();
    }

    private void Update()
    {
        if (GameController.OnGame)
        {
            if (Input.touchCount > 0 && !fruitDropped)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended && !IsTouchingUIElement(touch)) 
                {
                    fruitDropped = true;
                    StartCoroutine(SpawnFruit()); 
                }
            }

            if (RaycastManager.IsColliding(Distance, CollisionTime, StartPosition, LayerMask)) HeightController.SetActive(true);
            else HeightController.SetActive(false);
        }
    }

    IEnumerator SpawnFruit()
    {
        yield return new WaitForSeconds(0.5f);

        fruitDropped = false;
        IndicatorController.SetActive(true);
        IndicatorController.transform.position = Vector2.zero;

        var go = Instantiate(Fruits[nextFruitIndex], transform);
        var fc = go.GetComponent<FruitController>();

        fc.SetObjectsFromParent(CameraShakeController, GameController, Canvas);

        nextFruitIndex = RandomManager.GetRandomIndex(SpritesFruits.Count);
        UIManager.SetImage(Renderer, SpritesFruits[nextFruitIndex]);
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
