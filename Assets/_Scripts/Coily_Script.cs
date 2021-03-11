using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coily_Script : BallMovement
{

    [SerializeField]
    private float xFactor = 0.48f;
    [SerializeField]
    private float yFactor = -0.7f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Sprite currentSprite;

    [Header("Sprites For Coily Ball")]
    public Sprite Idle;
    public Sprite Jump;

    [Header("Sprites For Coily")]
    public Sprite BottomLeft;
    public Sprite BottomRight;
    public Sprite JumpBottomLeft;
    public Sprite JumpBottomRight;
    public Sprite TopLeft;
    public Sprite TopRight;
    public Sprite JumpTopLeft;
    public Sprite JumpTopRight;

    enum SpriteJumpDirection
    {
        BottomLeft,
        BottomRight,
        JumpBottomLeft,
        JumpBottomRight,
        TopLeft,
        TopRight,
        JumpTopLeft,
        JumpTopRight
    }

    private SpriteJumpDirection SpriteSwitchCase;

    [Header("Qbert")]
    [SerializeField]
    private GameObject Qbert;
    private Qbert_Script QbertScript;

    private bool spawnLocationCheck = true;

    public Vector3 position0 = Vector3.zero;
    public Vector3 position1 = Vector3.zero;
    public Vector3 position2 = Vector3.zero;

    public bool isJumping = true;

    [SerializeField] private float BezierCurveFactor;
    private float tParamBezierParameter = 0f;

    private bool isCoilyHatched = false;

   

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Coily Started");
        spriteRenderer.sprite = Idle;
        this.gameObject.layer = 8;
        QbertScript = GameObject.FindGameObjectWithTag("Qbert").GetComponent<Qbert_Script>();
        if (Qbert == null)
        {
            Qbert = GameObject.Find("Qbert");
        }
        //QbertScript.CheckAccessForScriptDebugPurposes();
        //Qbert.gameObject.GetComponent<Transform>().position = new Vector2(10f, 10f);

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y <= -0.37f && spawnLocationCheck)
        {
            //Debug.Log(this.GetComponent<Rigidbody2D>().gravityScale);
            //Debug.Log("Disabled");
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            //Debug.Log(this.GetComponent<Rigidbody2D>().gravityScale);
            spawnLocationCheck = false;
            isJumping = false;
            // Nice function call way.
            InvokeRepeating("MoveToNextPosition", 0, 1.5f);
        }

        if (tParamBezierParameter < 1.0 && isJumping)
        {
            this.tParamBezierParameter += BezierCurveFactor * Time.deltaTime;
            this.transform.position = Vector3.Lerp(Vector3.Lerp(this.position0, this.position1, tParamBezierParameter),
                Vector3.Lerp(this.position1, this.position2, tParamBezierParameter), tParamBezierParameter);
        }

        if (this.transform.position == this.position2 && !isCoilyHatched)
        {
            isJumping = false;
            tParamBezierParameter = 0;
            spriteRenderer.sprite = Idle;
        }

        if (this.transform.position.y <= -3.917324f && !isCoilyHatched)
        {
            CoilyHatch();
        }

        if (isJumping && isCoilyHatched)
        {
            switch (SpriteSwitchCase)
            {
                case SpriteJumpDirection.JumpBottomLeft:
                    //Debug.Log("Here in resting of hatched coily");
                    spriteRenderer.sprite = JumpBottomLeft;
                    break;

                case SpriteJumpDirection.JumpBottomRight:
                    spriteRenderer.sprite = JumpBottomRight;
                    break;

                case SpriteJumpDirection.JumpTopLeft:
                    spriteRenderer.sprite = JumpTopLeft;
                    break;

                case SpriteJumpDirection.JumpTopRight:
                    spriteRenderer.sprite = JumpTopRight;
                    break;

                default:
                    
                    break;
            }
        }

        if (this.transform.position == this.position2 && isCoilyHatched)
        {
            isJumping = false;
            tParamBezierParameter = 0;
            if (SpriteSwitchCase == SpriteJumpDirection.JumpTopLeft)
            {
                spriteRenderer.sprite = TopLeft;
            }
            else if (SpriteSwitchCase == SpriteJumpDirection.JumpTopRight)
            {
                spriteRenderer.sprite = TopRight;
            }
            else if (SpriteSwitchCase == SpriteJumpDirection.JumpBottomLeft)
            {
                spriteRenderer.sprite = BottomLeft;
            }
            else if (SpriteSwitchCase == SpriteJumpDirection.JumpBottomRight)
            {
                spriteRenderer.sprite = BottomRight;
            }
        }

    }

    public void MoveAlongBezierCurve(Vector3 startPosition, Vector3 endPosition)
    {
        position0 = startPosition;
        position2 = endPosition;
        position1 = position0 + (position2 - position0) / 2 + Vector3.up * BezierCurveFactor;

    }

    public void MoveToNextPosition()
    {
        spriteRenderer.sprite = Jump;
        isJumping = true;
        //Debug.Log("Check");
        float newXPosition = (Random.Range(1, 10) % 2 == 0) ? xFactor : (xFactor * -1f);
        MoveAlongBezierCurve(this.transform.position, new Vector3(this.transform.position.x + newXPosition, this.transform.position.y + yFactor));
        //Debug.Log("Next Position = " + new Vector3(this.transform.position.x + newXPosition, this.transform.position.y + yFactor));
    }

    public void CoilyHatch()
    {
        CancelInvoke();
        isJumping = false;
        isCoilyHatched = true;
        InvokeRepeating("CoilyChase", 0, 1.5f);
    }

    // Make Coily chase Qbert
    public void CoilyChase()
    {
        isJumping = true;
        float newXPosition = 0f;
        float newYPosition = 0f;
        if (Qbert.transform.position.y >= this.transform.position.y)
        {
            // y is a negative factor, hence to make is positive  multiplying by -1
            newYPosition = (yFactor * -1);
            //Debug.Log("Moving Up");
            if (Qbert.transform.position.x >= this.transform.position.x)
            {
                //Debug.Log("Moving Right Now");
                //spriteRenderer.sprite = TopRight;
                newXPosition = xFactor;
                SpriteSwitchCase = SpriteJumpDirection.JumpTopRight;
            }
            else
            {
                //Debug.Log("Moving Left Now");
                newXPosition = xFactor * -1f;
                SpriteSwitchCase = SpriteJumpDirection.JumpTopLeft;
            }
        }
        else
        {
            newYPosition = (yFactor);
            //Debug.Log("Moving Downwards");
            if (Qbert.transform.position.x >= this.transform.position.x)
            {
                //Debug.Log("Moving Right Now");
                newXPosition = xFactor;
                SpriteSwitchCase = SpriteJumpDirection.JumpBottomRight;
            }
            else
            {
                //Debug.Log("Moving Left Now");
                newXPosition = xFactor * -1;
                SpriteSwitchCase = SpriteJumpDirection.JumpBottomLeft;
            }
        }
        MoveAlongBezierCurve(this.transform.position, new Vector3(this.transform.position.x + newXPosition, this.transform.position.y + newYPosition));
    }
}
