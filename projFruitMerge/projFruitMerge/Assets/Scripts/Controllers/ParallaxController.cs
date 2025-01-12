using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] Vector2 StartPos;
    [SerializeField] Vector2 EndPos;

    [SerializeField] float Speed;
    [SerializeField] bool IsParallax;

    public bool Moving;

    RectTransform RectTransform;

    bool isUI;

    void Start()
    {
        isUI = TryGetComponent<RectTransform>(out RectTransform);
    }

    void Update()
    {
        if (Moving) Move();
    }

    public void Move()
    {
        if (isUI)
        {
            RectTransform.anchoredPosition = Vector2.MoveTowards(RectTransform.anchoredPosition, EndPos, Speed * Time.deltaTime);

            if (RectTransform.anchoredPosition == EndPos && IsParallax) RectTransform.anchoredPosition = StartPos;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, EndPos, Speed * Time.deltaTime);

            if ((Vector2)transform.position == EndPos && IsParallax) transform.position = StartPos;
        }
    }
}
