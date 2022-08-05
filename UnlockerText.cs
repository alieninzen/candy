using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UnlockerText : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Text percentsText;
    [SerializeField] private Image imageBlack;
    [SerializeField] private Image imageFill;
    [SerializeField] private Color[] colors;
    private int percentsUnlockPerLevel=20;
    void Start()
    {
        text.supportRichText = true;
        loadImage();
    }
    void loadImage()
    {
        if (CandyUnlocker.Instance.lockedIds.Length <= 1)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            if (CandyUnlocker.Instance.startUnlockRandomCandy() != null)
            {
                imageBlack.sprite = CandyUnlocker.Instance.startUnlockRandomCandy();
                imageFill.sprite = imageBlack.sprite;
                setText(CandyUnlocker.Instance.candyName());
                setPercents();
            }
        }

    }

    void setPercents()
    {
        int percents = PlayerPrefs.GetInt("unlockedPercents", 0);
        percentsText.text = percents.ToString() + "%";
        imageFill.fillAmount = (percents) / 100f;
        StartCoroutine(addPercent(percents));
    }
    IEnumerator addPercent(int percents)
    {
        Sounds.Instance.playOther(0);
        int percentsToAdd = 0; //for smooth animation and text;
        if (percents<=100- percentsUnlockPerLevel)
        {
            while (percentsToAdd < percentsUnlockPerLevel)
            {

                yield return new WaitForSeconds(0.07f);
                percentsToAdd++;
                percentsText.text = (percents + percentsToAdd).ToString()+"%";
                imageFill.fillAmount = (percents + percentsToAdd) / 100f;

            }
            if (percentsUnlockPerLevel + percents>= 100){
                PlayerPrefs.SetInt("unlockedPercents", 0);
                //unlock candy
                CandyUnlocker.Instance.unlock(PlayerPrefs.GetInt("unlocking"));
            }
            else
            {
                PlayerPrefs.SetInt("unlockedPercents", percentsUnlockPerLevel + percents);
            }
            
        }
        SceneManager.LoadScene("Game");
        yield return null;
    }

    void setText(string itemName)
    {
        int colorid = 0;
        string color = ColorUtility.ToHtmlStringRGB(colors[colorid]);
        text.text = "Next candy: " + "\n<size=70><color=#" + color + ">" + itemName + "\n" +"" + "</color></size>";
    }
}
