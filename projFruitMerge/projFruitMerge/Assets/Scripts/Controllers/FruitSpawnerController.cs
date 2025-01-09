using System;
using System.Collections;
using UnityEngine;

class FruitSpawnerController : MonoBehaviour
{
    [SerializeField] GameObject IndicatorController;
    [SerializeField] SpriteRenderer Renderer;

    [SerializeField] System.Collections.Generic.List<GameObject> Fruits;
    [SerializeField] System.Collections.Generic.List<Sprite> SpritesFruits;

    bool hasFruitSpawned = true;
    int nextFruitIndex;

    private void Start()
    {
        IndicatorController.SetActive(true);
        Instantiate(RandomManager.GetRandomObject<GameObject>(Fruits), transform);
        nextFruitIndex = RandomManager.GetRandomIndex(SpritesFruits.Count);
        Renderer.sprite = SpritesFruits[nextFruitIndex];
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended) StartCoroutine(SpawnFruit());
        }
    }

    IEnumerator SpawnFruit()
    {
        yield return new WaitForSeconds(0.5f);

        IndicatorController.SetActive(true);
        IndicatorController.transform.position = Vector2.zero;

        Instantiate(Fruits[nextFruitIndex], transform);

        nextFruitIndex = RandomManager.GetRandomIndex(SpritesFruits.Count);
        Renderer.sprite = SpritesFruits[nextFruitIndex];
    }
}
