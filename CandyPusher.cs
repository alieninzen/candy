using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{

    private Camera cam;

    [SerializeField] [Range(1f, 100f)] private float pushForce = 10f ;
    [SerializeField] private GameObject candyParent;
    [SerializeField] private GameObject cursorController;
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private bool slowMotion;
    [SerializeField] private bool enableCursor;
    [SerializeField][Range(0.01f, 1f)] private float timeScaleAtDragging;
    [SerializeField][Range(0f, 1f)] private float candyForceScaler;
    [SerializeField][Range(100f, 111f)] private float maxForce;


    private bool isDragging = false;
    private GameObject selectedCandy;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector2 force;
    private float distance;
    private SpriteRenderer cursorIcon;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cursorIcon = cursorController.GetComponent<SpriteRenderer>();
        cursorIcon.enabled = false;
    }

    void Update()
    {   
        if (enableCursor)
        {
            cursorController.transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0.3f,-0.3f,100f);   
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);
            if (hit.transform != null && hit.transform.root.gameObject == candyParent)

            {
                selectedCandy = hit.transform.gameObject;
                isDragging = true;
                OnDragStart();
            }
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            OnDragEnd();
        }

        if (isDragging)
        {
            OnDrag();
        }


    }

    void OnDragStart()
    {
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        trajectory.Show();  
    }

    void OnDrag()
    {
        if(trajectory) Time.timeScale = timeScaleAtDragging;        
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;
        if (force.x > maxForce)
        {
            force.x = maxForce;
        }
        if (force.y >= maxForce)
        {
            force.y = maxForce;
        }
        Debug.DrawLine(startPoint, endPoint);

        trajectory.UpdateDots(selectedCandy.transform.position, force);

        if (enableCursor)
        {
            cursorIcon.enabled = true;
        }
    }

    void OnDragEnd()
    {
        Time.timeScale = 1f;
        Rigidbody2D rigidbody2D = selectedCandy.GetComponent<Rigidbody2D>();
        if (isJelly(selectedCandy.transform))
        {
            if (selectedCandy.name.Contains("Point"))
            {
                selectedCandy.GetComponentInParent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            }
            else
            {
                rigidbody2D.AddForce(force, ForceMode2D.Impulse);
            }
          
            Sounds.Instance.playJelly();
        }
        else
        {
            rigidbody2D.AddForce(force*candyForceScaler, ForceMode2D.Impulse);
            Sounds.Instance.playCandy();
        }
        
        if (enableCursor)
        {
            cursorIcon.enabled = false;
        }
                    
        trajectory.Hide();
    }

    bool isJelly(Transform selectedCandy)
    {
        if (selectedCandy.name.Contains("Point") || selectedCandy.name.Contains("Jelly")){
            return true;
        }
        return false;
    }
}
