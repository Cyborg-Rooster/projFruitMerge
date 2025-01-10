using System.Collections;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    public CameraShakeController CameraShakeController;
    public GameController GameController;

    [SerializeField] GameObject NextFruit;

    [SerializeField] int PointsToAddOnMerge;

    bool collided;
    bool onGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (!onGame)
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

                if (touch.phase == TouchPhase.Ended) SetOnGame();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag != "Watermelon" && collision.gameObject != gameObject)
        {
            var fruitCollided = collision.gameObject;

            if (fruitCollided.tag == gameObject.tag && !collided)
            {
                var go = Instantiate(NextFruit, transform.position, Quaternion.identity, transform.parent);
                var ft = go.GetComponent<FruitController>();

                collided = true;
                fruitCollided.GetComponent<FruitController>().collided = collided;

                ft.CameraShakeController = CameraShakeController;
                ft.GameController = GameController;
                ft.SetOnGame();

                CameraShakeController.TriggerShake();
                GameController.AddPoints(PointsToAddOnMerge);

                Destroy(fruitCollided);
                Destroy(gameObject);
            }
        }
    }


    private void SetOnGame()
    {
        onGame = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
