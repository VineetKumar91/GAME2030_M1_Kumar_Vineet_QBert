using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Prefab of Block
    public Blocks prefab_Blocks;

    // Prefab of Red Ball
    public GameObject prefab_RedBall;
    private GameObject RedBall;

    // Prefab of Green Ball (friendly)
    public GameObject prefab_GreenBall;
    private GameObject GreenBall;

    // Prefab of Coily
    public GameObject prefab_Coily;
    private GameObject Coily;

    // Arrows Blinking
    public bool bKeepOnBlinking = true;
    private GameObject[] insideArrows = new GameObject[2];
    private GameObject[] outsideArrows = new GameObject[2];
    public float fblinkArrowInterval = 0.25f;

    int rows = 7; 
    // Start is called before the first frame update
    void Start()
    {
        // Design Layout 1 by 1
        createBlocks(rows);
        
        // Initialize red ball, green ball and coily
        initializeRedBall();
        initializeGreenBall();
        initializeCoily();

        // Get the blinking arrows
        insideArrows[0] = GameObject.Find("InsideArrow_Left");
        insideArrows[1] = GameObject.Find("InsideArrow_Right");
        outsideArrows[0] = GameObject.Find("OutsideArrow_Left");
        outsideArrows[1] = GameObject.Find("OutsideArrow_Right");
        for (int i = 0; i < 2; i++)
        {
            insideArrows[i].SetActive(false);
            outsideArrows[i].SetActive(false);
        }
        StartCoroutine(BlinkArrow());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Debug.Log("Escape Key Pressed");
            QuitGame();
        }
    }
    
    // Create the layout for the game (starting with block)
    private void createBlocks(int rows)
    {
        float xPos = 0.0f;
        float yPos = 0.0f;
        float xFactor = 0.96f;
        float yFactor = -0.7f;
        // i = 0
        // j = 0

        // Number of rows for the pyramid
        for (int i = rows - 1; i >= 0; i--)
        {
            //Debug.Log(i);
            yPos = yFactor * i;
            xPos = -0.48f * i;
            // Number of columns of blocks in each row
            for (int j = 0; j <= i; j++)
            {
                // Instantiate creates the object during run-time
                // position factor -> +- -0.48, -0.7
                Instantiate(prefab_Blocks, new Vector2(xPos, yPos), Quaternion.identity);
                xPos += xFactor;
            }
        }
    }

    // Initialize Red ball, Green ball and Coily
    private void initializeRedBall()
    {
        RedBall = Instantiate(prefab_RedBall, new Vector2(-0.96f, -1f), Quaternion.identity);
        RedBall.layer = 1;
    }

    private void initializeGreenBall()
    {
        GreenBall = Instantiate(prefab_GreenBall, new Vector2(0.96f, -2.5f), Quaternion.identity);
        GreenBall.layer = 1;
    }

    private void initializeCoily()
    {
        Coily = Instantiate(prefab_Coily, new Vector2(0f, -3.75f), Quaternion.identity);
        Coily.layer = 1;
    }

    // Blink Arrow Function
    public IEnumerator BlinkArrow()
    {
        // Use Renderer
        /*SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        while (bKeepOnBlinking)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(seconds);
        }*/

        //SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

        while (bKeepOnBlinking)
        {
            //Debug.Log("In Blink Arrow");

            // Animation 1 - Only Outside Arrows are Visible
            yield return new WaitForSeconds(fblinkArrowInterval);
            for (int i = 0; i < 2; i++)
            {
                outsideArrows[i].SetActive(true);
            }
            
            // Animation 2 - Inside Arrows are also Visible
            yield return new WaitForSeconds(fblinkArrowInterval);
            for (int i = 0; i < 2; i++)
            {
                insideArrows[i].SetActive(true);
            }

            // Animation 3 - Both arrows are not visible
            yield return new WaitForSeconds(fblinkArrowInterval);
            for (int i = 0; i < 2; i++)
            {
                outsideArrows[i].SetActive(false);
                insideArrows[i].SetActive(false);
            }
        }
    }

   

    // Quit Game on Esc Button
    private void QuitGame()
    {
        Application.Quit();
    }
}
