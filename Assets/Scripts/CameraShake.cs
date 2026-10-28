using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    Vector3 startPos;

    void Awake()
    {
        Instance = this;
        startPos = transform.position;
    }

    public void Shake(float duration = 0.25f, float strength = 0.2f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float t = 0f;
        while (t < duration)
        {
            transform.position = startPos + (Vector3)(Random.insideUnitCircle * strength);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.position = startPos;
    }
}