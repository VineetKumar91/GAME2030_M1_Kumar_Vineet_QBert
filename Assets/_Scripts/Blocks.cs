using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite Block;
    public Sprite BlockChanged;
    private bool isChanged = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Qbert" && !isChanged)
        {
            //Debug.Log("Collision");
            spriteRenderer.sprite = BlockChanged;
            isChanged = true;
            GameManager.CurrentScore += 25;
        }
            
    }
}
