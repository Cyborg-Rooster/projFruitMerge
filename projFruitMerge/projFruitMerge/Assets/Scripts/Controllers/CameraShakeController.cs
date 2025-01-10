using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float duration = 0.5f;      // Dura��o do tremor
    public float magnitude = 0.3f;    // Intensidade do tremor
    private Vector3 originalPosition; // Posi��o inicial da c�mera

    void Start()
    {
        // Armazena a posi��o original da c�mera
        originalPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        // Inicia o tremor
        StartCoroutine(Shake());
    }

    private System.Collections.IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calcula um deslocamento aleat�rio para a posi��o
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;

            yield return null; // Espera pelo pr�ximo frame
        }

        // Retorna � posi��o original
        transform.localPosition = originalPosition;
    }
}

