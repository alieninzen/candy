using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    private float soundTreshold = 1;
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float velocity =(collision.relativeVelocity.magnitude);
        if (velocity >= 10)
        {
            if (collision.collider.name.Contains("Point") || collision.collider.name.Contains("Jelly"))
            {
                Sounds.Instance.playJelly();
                HapticPatterns.PlayEmphasis(0.85f, 0.05f);
            }
            else
            {
                HapticPatterns.PlayEmphasis(0.85f, 0.05f);
                Sounds.Instance.playCandy();
            }
            
        }
    }
}
