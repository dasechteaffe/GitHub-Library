using UnityEngine;
using UnityEngine.UI;

public class ImageScaler : MonoBehaviour
{
    public Image imageToScale;
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float scaleDuration = 1f;

    private bool isScalingUp = true;
    private float currentTime = 0f;

    private void Update()
    {
        currentTime += Time.deltaTime;
        float t = currentTime / scaleDuration;

        if (isScalingUp)
        {
            imageToScale.transform.localScale = Vector3.Lerp(Vector3.one * minScale, Vector3.one * maxScale, t);
        }
        else
        {
            imageToScale.transform.localScale = Vector3.Lerp(Vector3.one * maxScale, Vector3.one * minScale, t);
        }

        if (t >= 1f)
        {
            currentTime = 0f;
            isScalingUp = !isScalingUp;
        }
    }
}