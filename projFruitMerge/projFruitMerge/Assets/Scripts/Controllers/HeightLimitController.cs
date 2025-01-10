using UnityEngine;

public class HeightLimitController : MonoBehaviour
{
    [SerializeField] float RepeatTime;

    SpriteRenderer SpriteRenderer;
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("Blink", 0, RepeatTime);
    }

    void Blink()
    {
        SpriteRenderer.enabled = !SpriteRenderer.enabled;
    }
}
