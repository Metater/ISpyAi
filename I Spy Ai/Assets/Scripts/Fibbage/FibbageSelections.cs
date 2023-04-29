using UnityEngine;
using UnityEngine.UI;

public class FibbageSelections : MonoBehaviour
{
    public RawImage image;

    public void Show()
    {
        if (image.texture == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void Hide()
    {
        image.texture = null;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}