using System;
using System.Collections;
using UnityEngine;

class FruitSpawnerController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject IndicatorController;
    [SerializeField] GameObject HeightController;
    [SerializeField] SpriteRenderer Renderer;
    [SerializeField] CameraShakeController CameraShakeController;
    [SerializeField] GameController GameController;

    [SerializeField] System.Collections.Generic.List<GameObject> Fruits;
    [SerializeField] System.Collections.Generic.List<Sprite> SpritesFruits;

    [Header("Height Limiter Options")]
    [SerializeField] Vector2 StartPosition;
    [SerializeField] LayerMask LayerMask;
    [SerializeField] float Distance;
    [SerializeField] float CollisionTime;

    RaycastManager RaycastManager;

    int nextFruitIndex;

    private void Start()
    {
        IndicatorController.SetActive(true);
        var go = Instantiate(RandomManager.GetRandomObject<GameObject>(Fruits), transform);
        var fc = go.GetComponent<FruitController>();

        fc.CameraShakeController = CameraShakeController;
        fc.GameController = GameController;

        nextFruitIndex = RandomManager.GetRandomIndex(SpritesFruits.Count);
        Renderer.sprite = SpritesFruits[nextFruitIndex];

        RaycastManager = new RaycastManager();
    }

    private void Update()
    {
        if (GameController.OnGame)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) StartCoroutine(SpawnFruit());
            }

            if (RaycastManager.IsColliding(Distance, CollisionTime, StartPosition, LayerMask)) HeightController.SetActive(true);
            else HeightController.SetActive(false);
        }
    }

    IEnumerator SpawnFruit()
    {
        yield return new WaitForSeconds(0.5f);

        IndicatorController.SetActive(true);
        IndicatorController.transform.position = Vector2.zero;

        var go = Instantiate(Fruits[nextFruitIndex], transform);
        var fc = go.GetComponent<FruitController>();

        fc.CameraShakeController = CameraShakeController;
        fc.GameController = GameController;

        nextFruitIndex = RandomManager.GetRandomIndex(SpritesFruits.Count);
        Renderer.sprite = SpritesFruits[nextFruitIndex];
    }
}
