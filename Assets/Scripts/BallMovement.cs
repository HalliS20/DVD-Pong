using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private GameObject _borderBox;
    public TextMeshProUGUI counter;
    public GameObject paddle;
    [SerializeField] private Vector2 moveDirection = new Vector2(1f, 1f);


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _borderBox = GameObject.FindGameObjectsWithTag("BorderBox")[0];


        if (_rb != null)
        {
            _rb.linearVelocity = moveDirection.normalized * moveSpeed;
        }
    }

    private void Update()
    {

        _rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ExitScanner(0);
        ColorChange();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Paddle")) return;

        moveDirection.y = -moveDirection.y;
        var text = counter.text;
        ColorChange();
        counter.text = (int.Parse(text) + 1).ToString();
        var roundMul = int.Parse(text) / 5;
        moveSpeed = 5 + (float)Math.Pow(1.15, roundMul);
    }


    private void ExitScanner(int extraSize)
    {
        var width = _borderBox.transform.localScale.x + extraSize;
        var height = _borderBox.transform.localScale.y + extraSize;
        var outsideRight = _rb.position.x >= width/2;
        var outsideLeft = _rb.position.x <= -width/2;
        var outsideTop = _rb.position.y >= height/2;
        var outsideBottom = _rb.position.y <= -height/2;
        Vector2 position = _rb.position;
        if (outsideRight || outsideLeft)
        {
            moveDirection.x = position.x > 0 ? -math.abs(moveDirection.x) : math.abs(moveDirection.x);
        }
        if (outsideTop)
        {
            moveDirection.y = position.y > 0 ? -math.abs(moveDirection.y) : math.abs(moveDirection.y);
        }
        if (outsideBottom)
        {
            position = new Vector2(0,0);
            _rb.position = position;
            moveDirection = new Vector2(1f, 1f);
            _rb.linearVelocity = moveDirection.normalized * moveSpeed;
            counter.text = "0";
            moveSpeed = 5;
        }
    }

    private void ColorChange()
    {
        var color = GetColor();
        while (color == _sprite.color)
        {
            color = GetColor();
        }
        _sprite.color = color;
    }

    private static Color GetColor()
    {
        var greenSpec = UnityEngine.Random.Range(0, 2);
        var redSpec = UnityEngine.Random.Range(0, 2);
        var blueSpec = UnityEngine.Random.Range(0, 2);
        if (blueSpec+redSpec+greenSpec == 0)
        {
            blueSpec = 1;
            redSpec = 1;
            greenSpec = 1;
        }
        var color = new Color(redSpec, greenSpec, blueSpec);
        return color;
    }
}
