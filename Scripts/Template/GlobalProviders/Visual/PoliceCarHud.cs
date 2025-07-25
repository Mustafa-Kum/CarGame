using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoliceCarHUD : MonoBehaviour
{
    public RectTransform arrowIndicator; // UI element for offscreen indicator
    public RectTransform closeIndicator; // UI element for close/on-screen indicator
    public TextMeshProUGUI distanceText; // UI element for displaying distance
    public Transform player; // Reference to the player
    public Transform policeCar; // Reference to the police car
    public float minDistance = 50f; // Minimum distance to show close indicator
    public float alertDistance = 20f; // Distance at which to start changing color to red
    public float screenEdgeBuffer = 50f; // Buffer to keep the arrow slightly inside the screen
    public float smoothing = 0.1f; // Smoothing factor
    public float hysteresisBuffer = 15f; // Buffer to prevent rapid toggling
    public Image _redScreen;
    public Image _redScreen2;

    private Camera mainCamera;
    private Vector3 smoothArrowPosition;
    private Vector3 smoothClosePosition;
    private Quaternion smoothArrowRotation;
    private bool isClose; // Track the current state
    private Coroutine fadeCoroutine;

    private readonly Color startColor = Color.white;
    private readonly Color endColor = Color.red;

    private void Start()
    {
        mainCamera = Camera.main;
        arrowIndicator.gameObject.SetActive(false);
        closeIndicator.gameObject.SetActive(false);
        distanceText.gameObject.SetActive(false);
        smoothArrowPosition = arrowIndicator.position;
        smoothClosePosition = closeIndicator.position;
        smoothArrowRotation = arrowIndicator.rotation;
    }

    private void Update()
    {
        var screenPoint = mainCamera.WorldToScreenPoint(policeCar.position);
        var isOffScreen = screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height;

        var distance = Vector3.Distance(player.position, policeCar.position);

        if (distance <= minDistance + hysteresisBuffer && !isOffScreen)
        {
            if (!isClose)
            {
                // Only activate if the state has changed
                arrowIndicator.gameObject.SetActive(false);
                closeIndicator.gameObject.SetActive(true);
                distanceText.gameObject.SetActive(true);
                isClose = true;
            }
            ShowCloseIndicator(screenPoint, distance);
            
            StartFade(_redScreen, 1f, 1f);
            StartFade(_redScreen2, 1f, 1f);
        }
        else if (distance > minDistance - hysteresisBuffer || isOffScreen)
        {
            if (isClose)
            {
                // Only activate if the state has changed
                closeIndicator.gameObject.SetActive(false);
                distanceText.gameObject.SetActive(false);
                arrowIndicator.gameObject.SetActive(true);
                isClose = false;
            }
            ShowOffscreenIndicator(screenPoint, distance);
            
            StartFade(_redScreen, 0f, 1f);
            StartFade(_redScreen2, 0f, 1f);
        }
    }



    private void ShowCloseIndicator(Vector3 screenPoint, float distance)
    {
        smoothClosePosition = Vector3.Lerp(smoothClosePosition, screenPoint, smoothing);
        closeIndicator.position = smoothClosePosition;
        distance = Mathf.Clamp(distance, 0, distance);
        // Ensure showText is not negative
        var showText = Mathf.Max(0, distance - 5);
        distanceText.text = $"{showText:F1}m";

        // Smooth color change using Mathf.SmoothStep
        float t = Mathf.SmoothStep(alertDistance, minDistance, distance);
        distanceText.color = Color.Lerp(endColor, startColor, t);

        // Calculate linear scaling
        var scale = Mathf.Lerp(4.0f, 1.0f, distance/8.0f);

        // Implement a "ping-pong" scale effect
      
        distanceText.transform.localScale = Vector3.one*scale;
    }

    private void ShowOffscreenIndicator(Vector3 screenPoint, float distance)
    {
        var clampedScreenPoint = screenPoint;

        // Clamp the position to stay within the screen bounds with a buffer
        clampedScreenPoint.x = Mathf.Clamp(clampedScreenPoint.x, screenEdgeBuffer, Screen.width - screenEdgeBuffer);
        clampedScreenPoint.y = Mathf.Clamp(clampedScreenPoint.y, screenEdgeBuffer, Screen.height/2 - screenEdgeBuffer);

        // Smooth the arrow position
        smoothArrowPosition = Vector3.Lerp(smoothArrowPosition, clampedScreenPoint, smoothing);
        arrowIndicator.position = smoothArrowPosition;

        // Rotate the arrow to point towards the police car smoothly
        var direction = (policeCar.position - player.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        smoothArrowRotation = Quaternion.Lerp(smoothArrowRotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), smoothing);
        arrowIndicator.rotation = smoothArrowRotation;

        // Update the distance text position and content
        distanceText.gameObject.SetActive(true);

        var showText = distance - 5;
        distanceText.text = $"{showText:F1}m";
        //   distanceText.transform.position = smoothArrowPosition + new Vector3(0, -30, 0); // Adjust position as needed

        // Reset scale and color
        distanceText.transform.localScale = Vector3.one;
        distanceText.color = startColor;
    }
    
    private void StartFade(Image image, float targetAlpha, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeImage(image, targetAlpha, duration));
    }

    private IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = newAlpha;
            image.color = newColor;
            yield return null;
        }

        // Ensure the target alpha is set at the end of the duration
        Color finalColor = image.color;
        finalColor.a = targetAlpha;
        image.color = finalColor;
    }
}
