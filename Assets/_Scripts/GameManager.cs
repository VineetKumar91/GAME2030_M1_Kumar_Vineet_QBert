using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Prefab of Block
    public Blocks prefab_Blocks;
    private Blocks[] prefab_blocksArray;

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

    [SerializeField]
    private float xFactor = 0.96f;
    [SerializeField]
    private float yFactor = -0.7f;


    // Spawn Locations
    [Header("Enemy Spawn Location")]
    [SerializeField]
    private float spawnEnemy_X_1 = 0.48f;
    [SerializeField]
    private float spawnEnemy_X_2 = -0.48f;
    [SerializeField]
    private float spawnEnemy_Y = -0.37f;

    // Red Ball
    [SerializeField] 
    private float delay_spawnRedBall;

    private bool isReadyToSpawnRedBall = true;

    // Green Ball
    [SerializeField]
    private float delay_spawnGreenBall;

    private bool isReadyToSpawnGreenBall = true;

    // Purple Ball / Coily
    [SerializeField]
    private float delay_spawnPurpleBall;

    private bool isReadyToSpawnPurpleBall = true;

    private bool isCoilyStillAlive = false;


    public static int CurrentScore = 0;

    [SerializeField]
    private TextMeshProUGUI scoreTMP;

    void Start()
    {
        // Design Layout 1 by 1
        createBlocks(rows);
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
        CurrentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{ 
        //    Debug.Log("Escape Key Pressed");
        //    //  QuitGame();         <--- M1 change to M2 requirement
        //}

        if (isReadyToSpawnRedBall)
        {
            StartCoroutine(SpawnRedBall());
        }

        if (isReadyToSpawnGreenBall)
        {
            StartCoroutine(SpawnGreenBall());
        }

        if (isReadyToSpawnPurpleBall && !isCoilyStillAlive)
        {
            StartCoroutine(SpawnPurpleBall());
            isCoilyStillAlive = true;
        }

        scoreTMP.text = CurrentScore.ToString();
    }


    // Create the layout for the game (starting with block)
    private void createBlocks(int rows)
    {
        float xPos = 0.0f;
        float yPos = 0.0f;
        // i = 0
        // j = 0
        int k = 0;
        // Number of rows for the pyramid
        for (int i = rows - 1; i >= 0; i--)
        {
            //Debug.Log(i);
            yPos = yFactor * i;
            xPos = -0.48f * i;
            // Number of columns of blocks in each row
            for (int j = 0; j <= i; j++, k++)
            {
                // Instantiate creates the object during run-time
                // position factor -> +- -0.48, -0.7
                Instantiate(prefab_Blocks, new Vector2(xPos, yPos), Quaternion.identity);
                xPos += xFactor;

            }
        }
    }

    // Initialize Red ball, Green ball and Coily
    private void initializeRedBall(float xPos, float yPos)
    {
        RedBall = Instantiate(prefab_RedBall, new Vector2(xPos, yPos), Quaternion.identity);
        RedBall.layer = 1;
    }

    private void initializeGreenBall(float xPos, float yPos)
    {
        GreenBall = Instantiate(prefab_GreenBall, new Vector2(xPos, yPos), Quaternion.identity);
        GreenBall.layer = 1;
    }

    private void initializeCoily(float xPos, float yPos)
    {
        Coily = Instantiate(prefab_Coily, new Vector2(xPos, yPos), Quaternion.identity);
        Coily.layer = 1;
    }

    public IEnumerator SpawnRedBall()
    {
        isReadyToSpawnRedBall = false;
        yield return new WaitForSeconds(2);
        //Debug.Log("Spawned");
        if (Random.Range(1, 10) % 2 == 0)
        {
            initializeRedBall(spawnEnemy_X_1, spawnEnemy_Y);
        }
        else
        {
            initializeRedBall(spawnEnemy_X_2, spawnEnemy_Y);
        }
        yield return new WaitForSeconds(delay_spawnRedBall);
        isReadyToSpawnRedBall = true;
    }

    public IEnumerator SpawnGreenBall()
    {
        isReadyToSpawnGreenBall = false;
        yield return new WaitForSeconds(6);
        //Debug.Log("Spawned");
        if (Random.Range(1, 10) % 2 == 0)
        {
            initializeGreenBall(spawnEnemy_X_1, spawnEnemy_Y);
        }
        else
        {
            initializeGreenBall(spawnEnemy_X_2, spawnEnemy_Y);
        }
        yield return new WaitForSeconds(delay_spawnGreenBall);
        isReadyToSpawnGreenBall = true;
    }

    public IEnumerator SpawnPurpleBall()
    {
        isReadyToSpawnPurpleBall = false;
        yield return new WaitForSeconds(4);
        //Debug.Log("Spawned");
        if (Random.Range(1, 10) % 2 == 0)
        {
            initializeCoily(spawnEnemy_X_1, spawnEnemy_Y);
        }
        else
        {
            initializeCoily(spawnEnemy_X_2, spawnEnemy_Y);
        }
        yield return new WaitForSeconds(delay_spawnPurpleBall);
        isReadyToSpawnPurpleBall = true;
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

   
}
