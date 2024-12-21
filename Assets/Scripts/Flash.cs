using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashManager : MonoBehaviour
{
    public GameObject flashPanel;
    private Image image;
    private void Awake()
    {
        image = flashPanel.GetComponent<Image>();
        flashPanel.SetActive(false);
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // Make the panel visible and fully opaque
        flashPanel.SetActive(true);
        image.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(2f);

        // Gradually fade out the panel
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 2f);
            image.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        flashPanel.SetActive(false);
    }
}
