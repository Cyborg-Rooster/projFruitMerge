using UnityEngine;
using UnityEngine.UI;

public class BlinkController: MonoBehaviour
{
    [SerializeField] float RepeatTime;

    SpriteRenderer SpriteRenderer;
    Text Text;

    bool isText;

    void Start()
    {
        isText = TryGetComponent<Text>(out Text);

        if(!isText) SpriteRenderer = GetComponent<SpriteRenderer>();

        InvokeRepeating("Blink", 0, RepeatTime);
    }

    void Blink()
    {
        if (isText) Text.enabled = !Text.enabled;
        else SpriteRenderer.enabled = !SpriteRenderer.enabled;
    }
}
