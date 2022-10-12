using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{

    public bool isColored = false;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeColor(Color color)
    {
        meshRenderer.material.color = color;
        isColored = true;

        GameManager.singleton.CheckComplete();
    }
}
