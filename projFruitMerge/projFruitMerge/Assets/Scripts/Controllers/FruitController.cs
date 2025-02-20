using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FruitController : MonoBehaviour
{
    public CameraShakeController CameraShakeController;
    public GameController GameController;
    public GameObject Canvas;

    [Header("Audios")]
    [SerializeField] AudioResource Cut;
    [SerializeField] AudioResource Pop;

    [SerializeField] GameObject NextFruit;

    [SerializeField] int PointsToAddOnMerge;
    [SerializeField] float TimeToJoin;

    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private AudioSource audioSource;

    private float collisionTime = 0f; // Contador de tempo
    private bool isColliding = false; // Estado da colisão

    bool collided;
    bool onGame;


    // Update is called once per frame
    void Update()
    {
        if (GameController.OnGame)
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

                    if (!IsTouchingUIElement(touch))
                    {
                        float realX;
                        if (touchPosition.x < -0.88) realX = -0.8f;
                        else if (touchPosition.x > 0.88) realX = 0.8f;
                        else realX = touchPosition.x;

                        transform.position = new Vector3(realX, transform.position.y, transform.position.z);

                        if (touch.phase == TouchPhase.Ended)
                        {
                            SetOnGame();
                            PlayAudio(true);
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido é uma cópia do mesmo objeto
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            isColliding = true;
            collisionTime = 0f; // Reseta o contador
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isColliding && collision.gameObject.CompareTag(gameObject.tag))
        {
            if (tag != "Watermelon" && collision.gameObject != gameObject)
            {
                var fruitCollided = collision.gameObject;

                collisionTime += Time.deltaTime;

                if (fruitCollided.tag == gameObject.tag && !collided && collisionTime >= TimeToJoin)
                {
                    var go = Instantiate(NextFruit, transform.position, Quaternion.identity, transform.parent);
                    var ft = go.GetComponent<FruitController>();

                    collided = true;
                    fruitCollided.GetComponent<FruitController>().collided = collided;

                    ft.CameraShakeController = CameraShakeController;
                    ft.GameController = GameController;
                    ft.SetOnGame();
                    ft.PlayAudio(false);

                    CameraShakeController.TriggerShake();
                    GameController.AddPoints(PointsToAddOnMerge);

                    Destroy(fruitCollided);
                    Destroy(gameObject);
                }
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

    private void SetOnGame()
    {
        onGame = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void PlayAudio(bool fall)
    {
        if (Player.Sounds > 0)
        {
            audioSource = GetComponent<AudioSource>();
            var r = fall ? Cut : Pop;
            audioSource.resource = r;
            audioSource.Play();
        }
    }

    public void SetObjectsFromParent(CameraShakeController cameraShakeController, GameController gameController, GameObject canvas)
    {
        CameraShakeController = cameraShakeController;
        GameController = gameController;
        Canvas = canvas;

        graphicRaycaster = Canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }
}
