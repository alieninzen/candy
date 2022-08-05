using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CandySpawnerFromPoints : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] candies;
    [SerializeField] private float spawnInterwal;
    [SerializeField] private int spawnCount;
    [SerializeField] private Transform candyParent;
    [SerializeField] private float scalePower = 1f;

    private float verticalStartVelocity = 6;
    private float horisontalStartVelocityMax = 5;
    private float timer = 0.0f;
    private bool first = true;
    private int randomCandie, spawnPoint;
    private Camera camera;
    private float bordersX = 0.6f;
    private float bordersY = 0.8f;

    //playing area
    Vector2 rUCorner, lDCorner;
    float leftSide , rightSide , bottom, top;



    private void Start()
    {
       
        candies = GameObject.Find("CandyUnlocker").GetComponent<CandyUnlocker>().unlockedCandies.ToArray();
        spawnCount = PlayerPrefs.GetInt("level", 1) * 2;
        float candyCurrentWidth = candies[0].gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        camera = Camera.main;
        Vector2 leftDownCorner = camera.ViewportToWorldPoint(new Vector3(0, 0f, camera.nearClipPlane));
        Vector2 rightUpperCorner = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));
        initBorders();
        float cameraWidth = rightUpperCorner.x - leftDownCorner.x;
        float cameraHeight = rightUpperCorner.y - leftDownCorner.y;
        float playableWidth = cameraWidth - 2 * bordersX;
        float playableHeight = (cameraHeight - bordersY)/2;
        float playableSquare = playableWidth*playableHeight;
        float playableSquareOneCandy = playableSquare / spawnCount;
        float neededCandyWidth = Mathf.Sqrt(playableSquareOneCandy) - candyCurrentWidth / 10;
        if (spawnCount <=8 )
        {
            candyParent.localScale = 1*Vector3.one;
        }
        else
        {
            candyParent.localScale = (Vector3.one * neededCandyWidth / candyCurrentWidth) * scalePower;

        }
        
    }
    void initBorders()
    {
        rUCorner = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));
        lDCorner = camera.ViewportToWorldPoint(new Vector3(0, 0f, camera.nearClipPlane));
        leftSide = lDCorner.x;
        rightSide = rUCorner.x;
        bottom = lDCorner.y;
        top = rUCorner.y;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (spawnCount > 0)
        {
            if (timer > spawnInterwal)
            {
                spawnPoint++;
                if (spawnPoint >= spawnPoints.Length) spawnPoint = 0;
                randomCandie = Random.Range(0, candies.Length);
                GameObject newCandie = Instantiate(candies[randomCandie], candyParent);
                newCandie.transform.position = spawnPoints[spawnPoint].position;
                Rigidbody2D newCandieRigidBody2D = newCandie.GetComponentInChildren<Rigidbody2D>();
                newCandieRigidBody2D.transform.gameObject.AddComponent<CollisionDetection>();
                newCandieRigidBody2D.AddForce(Vector2.down * verticalStartVelocity+ Vector2.left * Random.Range(-horisontalStartVelocityMax, horisontalStartVelocityMax), ForceMode2D.Impulse);
                spawnCount--;
                timer -= spawnInterwal;
            }
        }
    }
}
