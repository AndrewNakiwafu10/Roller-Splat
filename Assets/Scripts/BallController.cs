using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody ballRb;
    public float speed = 15.0f;
    public int minSwipeRecognition = 500;
    

    private bool isTravelling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;
    private Vector2 swipePositionLastFrame;
    private Vector2 swipePositionCurrentFrame;
    private Vector2 currentSwipe;
    private Color solveColor;
    private MeshRenderer meshRenderer;

    public ParticleSystem endLevel;
    public AudioSource audioSource;
    //public AudioSource playerAudio;
    //public AudioClip cheerPlayer;

    // Start is called before the first frame update
    void Start()
    {
        solveColor = Random.ColorHSV(0.5f, 1);
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer.material.color = solveColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isTravelling)
        {
            ballRb.velocity = speed * travelDirection;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);

        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTravelling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isTravelling)
            return;

        if (Input.GetMouseButton(0))
        {
            swipePositionCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipePositionLastFrame != Vector2.zero)
            {
                currentSwipe = swipePositionCurrentFrame - swipePositionLastFrame;

                if(currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                // Up/Down
                if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    //Go Up/Down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                // left/Right
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
                {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }
            }
            swipePositionLastFrame = swipePositionCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePositionLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTravelling = true;
    }
}
