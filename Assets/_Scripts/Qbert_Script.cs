using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Qbert_Script : MonoBehaviour
{

    [SerializeField]
    private float xFactor = 0.48f;
    [SerializeField]
    private float yFactor = 0.7f;

    [SerializeField] 
    public SpriteRenderer spriteRenderer;
    private Movement_Script movementScript;

    public Vector3 position0 = Vector3.zero;
    public Vector3 position1 = Vector3.zero;
    public Vector3 position2 = Vector3.zero;

    public bool isJumping = false;

    [SerializeField] private float BezierCurveFactor;
    private float tParamBezierParameter = 0f;

    // Elevator stuff
    private Vector3 finalPosition;
    private Vector3 startingPosition;
    private Vector3 NormalizedDirection;
    public bool isOnElevator = false;

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

    [Header("Sprites")]
    public Sprite BottomLeft;
    public Sprite BottomRight;
    public Sprite JumpBottomLeft;
    public Sprite JumpBottomRight;
    public Sprite TopLeft;
    public Sprite TopRight;
    public Sprite JumpTopLeft;
    public Sprite JumpTopRight;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = BottomLeft;
        this.gameObject.layer = 6;
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isOnElevator)
        {
            return;
        }

        if (!isJumping)
        {
            KeyboardInputCheck();
            return;
        }

        if (tParamBezierParameter < 1.0)
        {
            this.tParamBezierParameter += BezierCurveFactor * Time.deltaTime;
            this.transform.position = Vector3.Lerp(Vector3.Lerp(this.position0, this.position1, tParamBezierParameter), 
                Vector3.Lerp(this.position1, this.position2, tParamBezierParameter), tParamBezierParameter);
        }

        if (isJumping)
        {
            switch (SpriteSwitchCase)
            {
                case SpriteJumpDirection.JumpBottomLeft:
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
                    spriteRenderer.sprite = BottomLeft;
                    break;
            }
        }
       

        if (this.transform.position == this.position2)
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

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    void KeyboardInputCheck()
    {
        
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("Left Up");
            //this.transform.position = new Vector3(this.transform.position.x - xFactor  , this.transform.position.y + yFactor, 0);
            MoveAlongBezierCurve(this.transform.position,
                new Vector2(this.transform.position.x - xFactor, this.transform.position.y + yFactor));
            SpriteSwitchCase = SpriteJumpDirection.JumpTopLeft;

        }
        else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.W))
        {
           // Debug.Log("Right Up");
            // this.transform.position = new Vector3(this.transform.position.x + xFactor, this.transform.position.y + yFactor, 0);
            MoveAlongBezierCurve(this.transform.position,
                new Vector2(this.transform.position.x + xFactor, this.transform.position.y + yFactor));
            SpriteSwitchCase = SpriteJumpDirection.JumpTopRight;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("Right Down");
            //this.transform.position = new Vector3(this.transform.position.x + xFactor, this.transform.position.y - yFactor, 0);
            MoveAlongBezierCurve(this.transform.position,
                new Vector2(this.transform.position.x + xFactor, this.transform.position.y - yFactor));
            SpriteSwitchCase = SpriteJumpDirection.JumpBottomRight;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("Left Down");
            // this.transform.position = new Vector3(this.transform.position.x - xFactor, this.transform.position.y - yFactor, 0);
            MoveAlongBezierCurve(this.transform.position,
                new Vector2(this.transform.position.x - xFactor, this.transform.position.y - yFactor));
            SpriteSwitchCase = SpriteJumpDirection.JumpBottomLeft;
        }
    }

    public void MoveAlongBezierCurve(Vector3 startPosition, Vector3 endPosition)
    {
       // Debug.Log("In Move Bezier Curve Function!");
        position0 = startPosition;
        position2 = endPosition;

       // Debug.Log("Position 0 = " + position0);
        //Debug.Log("Position 2 = " + position2);

        position1 = position0 + (position2 - position0) / 2 + Vector3.up * BezierCurveFactor;

        //Debug.Log("Position 1 = " + position1);

        //Vector3 m1 = Vector3.Lerp(position0, position1, BezierCurveFactor * Time.deltaTime);
        //Vector3 m2 = Vector3.Lerp(position1, position2, BezierCurveFactor * Time.deltaTime);
        //this.GetComponent<Transform>().position = Vector3.Lerp(m1, m2, BezierCurveFactor * Time.deltaTime);
        isJumping = true;
    }

    // Elevator stop to Starting Point Translation
    public void PostElevatorMovement()
    {
        finalPosition = new Vector3(0f, 0.5f);
        //startingPosition = this.transform.position;
        //NormalizedDirection = finalPosition - startingPosition;
        //NormalizedDirection = Vector3.Normalize(NormalizedDirection);
        
        //while (Vector3.Distance(this.transform.position, finalPosition) >= 0.01)
        //{
        //    this.transform.Translate(NormalizedDirection * Time.deltaTime * 0.1f);
        //    //Debug.Log("While Loop ");
        //}

        spriteRenderer.sprite = BottomLeft;
        isOnElevator = false;
        this.transform.position = finalPosition;
    }

    public void CheckAccessForScriptDebugPurposes()
    {
        Debug.Log("Script Accessed!");
    }
}
