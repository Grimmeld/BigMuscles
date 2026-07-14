using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimationNextIcon : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private Image[] images;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
        
    }

    private void OnEnable()
    {
        ClearArrows();
        StartCoroutine(OnArrowAnimation());
    }

    private void OnDisable()
    {
        StopCoroutine(OnArrowAnimation());
    }

    private IEnumerator OnArrowAnimation()
    {

        foreach (Image image in images)
        {

            image.gameObject.SetActive(true);

            yield return new WaitForSeconds(delay);


        }
        
        ClearArrows();

        yield return new WaitForSeconds(delay);

        StartCoroutine(OnArrowAnimation());
    }

    private void ClearArrows()
    {

        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
    }
}
