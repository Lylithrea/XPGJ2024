using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PileSize : MonoBehaviour
{

    public List<Sprite> cardVariations = new List<Sprite>();
    public GameObject cardPrefab;

    public int maxSize = 10;
    public float xOffset = 25f;
    public float ySpace = 10f;

    private int oldSize;

    public void AdjustSize(int newSize)
    {
        if (oldSize > newSize)
        {
            int amountToRemove = oldSize - newSize;
            int childCount = this.gameObject.transform.childCount;

            for (int i = 0; i < amountToRemove; i++)
            {

                GameObject child = this.gameObject.transform.GetChild(childCount - 1).gameObject;
                Destroy(child);

                // Decrease the stored child count
                childCount--;
            }
        }

        else
        {
            for (int i = oldSize; i < newSize; i++)
            {
                if (i > maxSize) return;
                GameObject img = Instantiate(cardPrefab, transform);
                float offset = Random.Range(-xOffset, xOffset);
                img.transform.localPosition = new Vector3(offset, i * ySpace, 0);
                img.GetComponent<Image>().sprite = cardVariations[Random.Range(0, cardVariations.Count)];
            }
        }
        oldSize = newSize;

    }


}
