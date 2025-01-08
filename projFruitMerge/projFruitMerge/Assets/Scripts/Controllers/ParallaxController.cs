using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] Vector2 StartPos;
    [SerializeField] Vector2 EndPos;

    [SerializeField] float Speed;
    [SerializeField] bool IsParallax;

    [SerializeField] bool Moving;

    void Start()
    {
        
    }

    void Update()
    {
        if (Moving) Move();
    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, EndPos, Speed * Time.deltaTime);

        if ((Vector2)transform.position == EndPos && IsParallax) transform.position = StartPos;
    }
}
