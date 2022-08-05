using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Candy
{
    public GameObject candyPrefab;
    public string candyName;
    public bool isJelly;
    public Sprite imageForJelly;
}

public class CandyUnlocker : MonoBehaviour
{
    private static CandyUnlocker instance = null;
    public static CandyUnlocker Instance
    {
        get { return instance; }
    }

    [HideInInspector] public List<Candy> allCandies = new List<Candy>();
    [HideInInspector] public string[] lockedIds;
    [HideInInspector] public List<GameObject> unlockedCandies= new List<GameObject>();
    private string defaultUnlocked = "0,1,2,15";
    private bool firstStart = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            loadPlayerPrefs();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    public Sprite startUnlockRandomCandy()
    {
        int nextCandyToUnlock = PlayerPrefs.GetInt("unlocking",-1);
        Sprite sprite = null ;
        if (PlayerPrefs.GetInt("unlocking",-1)== -1){
            nextCandyToUnlock = int.Parse(lockedIds[Random.Range(0, lockedIds.Length)]);
            PlayerPrefs.SetInt("unlocking", nextCandyToUnlock);
            PlayerPrefs.SetInt("unlockedPercents", 0);
        }

        if (allCandies[nextCandyToUnlock].imageForJelly != null)
        {
            sprite = allCandies[nextCandyToUnlock].imageForJelly;
        }
        else
        {
            sprite = allCandies[nextCandyToUnlock].candyPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        return sprite;
    }
    public string candyName()
    {
        int nextCandyToUnlock = PlayerPrefs.GetInt("unlocking");
        return (allCandies[nextCandyToUnlock].candyName);
    }
    void loadPlayerPrefs()
    {
        string[] unlocked = PlayerPrefs.GetString("unlocked").Split(',') ;
        lockedIds = PlayerPrefs.GetString("locked").Split(',');
        if (PlayerPrefs.GetString("locked", "first") == "first")
        {
            firstStart = true;
        }

        if (firstStart)
        {
            lockedIds = new string[allCandies.Count];
            string lockedString ="";
            for (int i = 0; i < allCandies.Count; i++)
            {
                lockedIds[i] = i.ToString() ;                   
                if (i == 0)
                {
                    lockedString = defaultUnlocked.Split(',')[0];
                }
                else
                {
                    lockedString += "," + i.ToString();
                }
            }
            PlayerPrefs.SetString("unlocked", "");
            PlayerPrefs.SetString("locked", lockedString);
            unlocked = defaultUnlocked.Split(',');
        }

        for (int i = 0; i < unlocked.Length; i++)
        {
            if (firstStart)
            {
                unlock(int.Parse(unlocked[i]));
            }
            else
            {
                int id = int.Parse(unlocked[i]);
                unlockedCandies.Add(allCandies[id].candyPrefab);
            }
            
        }

        Debug.Log("locked:" + PlayerPrefs.GetString("locked"));
        Debug.Log("unlocked:" + PlayerPrefs.GetString("unlocked"));
    }
    
   public void unlock(int id)
    {
        string unlocked = PlayerPrefs.GetString("unlocked");
        unlockedCandies.Add(allCandies[id].candyPrefab);
        if (unlocked == "")
        {
            unlocked = id.ToString();
        }
        else
        {
            unlocked += "," + id.ToString();
        }
     
        string[] locked = PlayerPrefs.GetString("locked").Split(','); 
        string newLocked = "";
        for(int i = 0; i < locked.Length; i++)
        {
            if (locked[i] != id.ToString())
            {
                if (newLocked == "")
                {
                    newLocked = locked[i];
                }
                else
                {
                    newLocked += ","+locked[i];
                }
               
            }
        }
        PlayerPrefs.SetString("unlocked", unlocked);
        PlayerPrefs.SetString("locked", newLocked);
        PlayerPrefs.SetInt("unlocking", -1);
        PlayerPrefs.Save();
    }
}
