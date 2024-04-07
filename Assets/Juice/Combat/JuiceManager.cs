using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceManager : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    public Camera mainCamera;

    public float zoomDuration = 0.1f;
    public float zoomMagnitude = 0.1f;

    public GameObject DeckSize_1;
    public GameObject DeckSize_2;
    public GameObject DeckSize_3;
    public GameObject DeckSize_4;

    public GameObject DiscardSize_1;
    public GameObject DiscardSize_2;
    public GameObject DiscardSize_3;
    public GameObject DiscardSize_4;

    public GameObject Ui_ToScreenShake;

    public List<GameObject> Followers;

    public int DeckSize = 10;

    public int DiscardSize = 10;

    private Vector3 originalCanvasPosition;
    private Vector3 originalCanvasScale;

    public int FollowerSize = 10;

    private void Start()
    {
        originalCanvasPosition = Ui_ToScreenShake.transform.localPosition;
        originalCanvasScale = Ui_ToScreenShake.transform.localScale;
        UpdateDeckSize();
        UpdateDiscardSize();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            Debug.Log("whiiiiii");
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
            UpdateDeckSize();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DiscardSize = DiscardSize + 5;
            UpdateDiscardSize();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            DeckSize = DeckSize - 5;
            UpdateDeckSize();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DiscardSize = DiscardSize - 5;
            UpdateDiscardSize();
        }


        if (Input.GetKeyDown(KeyCode.B))
        {
            UpdateFollowers(FollowerSize);
        }
        
    }

    public void UpdateFollowers(int FollowerAmount)
    {
        
    }

    public void UpdateDeckSize()
    {
        if (DeckSize >= 10)
        {
            Debug.Log("10 or less");
            DeckSize_1.SetActive(true);
            DeckSize_2.SetActive(false);
            DeckSize_3.SetActive(false);
            DeckSize_4.SetActive(false);
        }
        if (DeckSize >= 20)
        {
            Debug.Log("20 or more");
            DeckSize_1.SetActive(true);
            DeckSize_2.SetActive(true);
            DeckSize_3.SetActive(false);
            DeckSize_4.SetActive(false);
        }
        if (DeckSize >= 30)
        {
            Debug.Log("30 or more");
            DeckSize_1.SetActive(true);
            DeckSize_2.SetActive(true);
            DeckSize_3.SetActive(true);
            DeckSize_4.SetActive(false);
        }
        if (DeckSize >= 40)
        {
            Debug.Log("40 or more");
            DeckSize_1.SetActive(true);
            DeckSize_2.SetActive(true);
            DeckSize_3.SetActive(true);
            DeckSize_4.SetActive(true);
        }
    }

    public void UpdateDiscardSize()
    {
        if (DiscardSize >= 10)
        {
            Debug.Log("10 or less");
            DiscardSize_1.SetActive(true);
            DiscardSize_2.SetActive(false);
            DiscardSize_3.SetActive(false);
            DiscardSize_4.SetActive(false);
        }
        if (DiscardSize >= 20)
        {
            Debug.Log("20 or more");
            DiscardSize_1.SetActive(true);
            DiscardSize_2.SetActive(true);
            DiscardSize_3.SetActive(false);
            DiscardSize_4.SetActive(false);
        }
        if (DiscardSize >= 30)
        {
            Debug.Log("30 or more");
            DiscardSize_1.SetActive(true);
            DiscardSize_2.SetActive(true);
            DiscardSize_3.SetActive(true);
            DiscardSize_4.SetActive(false);
        }
        if (DiscardSize >= 40)
        {
            Debug.Log("40 or more");
            DiscardSize_1.SetActive(true);
            DiscardSize_2.SetActive(true);
            DiscardSize_3.SetActive(true);
            DiscardSize_4.SetActive(true);
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
