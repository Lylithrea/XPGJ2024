using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    public Camera mainCamera;

    public float zoomDuration = 0.1f;
    public float zoomMagnitude = 0.1f;

    public GameObject DeckSize_1;
    public GameObject DeckSize_2;
    public GameObject DeckSize_3;

    public GameObject Ui_ToScreenShake;

    public int DeckSize = 10;

    private Vector3 originalCanvasPosition;
    private Vector3 originalCanvasScale;

    private void Start()
    {
        originalCanvasPosition = Ui_ToScreenShake.transform.localPosition;
        originalCanvasScale = Ui_ToScreenShake.transform.localScale;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            Debug.Log("whaaaaaa");
            ScreenShake();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("whaaaaaa");
            ScreenZoom();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DeckSize = DeckSize + 5;
        }

       if(DeckSize >= 10)
        {
            Debug.Log("10 or less");
            DeckSize_1.SetActive(true);
            DeckSize_2.SetActive(false);
            DeckSize_3.SetActive(false);
        }
       if(DeckSize >= 20)
        {
            Debug.Log("20 or more");
            DeckSize_1.SetActive(false);
            DeckSize_2.SetActive(true);
            DeckSize_3.SetActive(false);
        }
       if(DeckSize >= 30) 
        {
            Debug.Log("30 or more");
            DeckSize_1.SetActive(false);
            DeckSize_2.SetActive(false);
            DeckSize_3.SetActive(true);
        }
    }

    // Method to trigger screen shake
    public void ScreenShake()
    {
        StartCoroutine(Shake());
    }

    public void ScreenZoom()
    {
        StartCoroutine(Zoom());
    }

    // Coroutine for screen shake effect
    IEnumerator Shake()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration)
        {
            Vector3 shake = Random.insideUnitSphere * shakeMagnitude;
            //mainCamera.transform.localPosition = originalCameraPosition + shake;

            Ui_ToScreenShake.transform.localPosition = originalCanvasPosition + shake;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Ui_ToScreenShake.transform.localPosition = originalCanvasPosition;
    }


    IEnumerator Zoom()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < zoomDuration)
        {
            Vector3 zoom = Random.insideUnitSphere * zoomMagnitude;
            //mainCamera.transform.localPosition = originalCameraPosition + shake;

            Ui_ToScreenShake.transform.localScale = originalCanvasScale + zoom;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Ui_ToScreenShake.transform.localScale = originalCanvasScale;
    }

    // Method to handle player taking damage
    public void TakeDamage(int damageAmount)
    {
        ScreenShake();

        // Add other effects when taking damage
        // Play sound effects
        // Flash red screen
        // Apply knockback
        // Decrease health
    }


}
