using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsCounter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text starsText;
    int currentStars = 0;
    void Start()
    {
        currentStars = PlayerPrefs.GetInt("stars", 0);
        starsText.text = currentStars.ToString();
    }

    public void AddStars(int stars)
    {
       StartCoroutine( addStarWithDelay(stars));
    }

    IEnumerator addStarWithDelay(int starsCount)
    {
        yield return new WaitForSeconds(1.8f);

        while (starsCount > 0)
        {
            
            yield return new WaitForSeconds(0.1f);
            starsCount--;
            currentStars++;
            starsText.text = currentStars.ToString();

        }
        PlayerPrefs.SetInt("stars", currentStars);
        yield return null;
    }
}
