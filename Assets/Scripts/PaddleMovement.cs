using System;
using TMPro;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float startX;
    [SerializeField] private float startY = -3f;
    [SerializeField] private float bouncingMultiplier = 1f;

    public TextMeshProUGUI counter;

    private GameObject _borderBox;
    private BoxCollider2D _collider;
    private float _floatRoundMul;

    private float _fullPaddleWidth;
    private int _intRoundMul;
    private float _maxSpeed = 12f;
    private float _minSpeed = 5f;
    private Rigidbody2D _rb;
    private string _text;

    private void Start()
    {
        _borderBox = GameObject.FindGameObjectsWithTag("BorderBox")[0];
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _fullPaddleWidth = _collider.bounds.size.x;
        UpdateValues();

        if (_rb != null)
        {
            transform.position = new Vector2(startX, startY);

            _rb.gravityScale = 0f;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Update()
    {
        var moveInput = Input.GetAxisRaw("Horizontal");
        var newPosition = _rb.position + new Vector2(moveInput * moveSpeed * Time.fixedDeltaTime, 0f);
        var boundaryLeft = -_borderBox.transform.localScale.x / 2 + 0.5f;
        var boundaryRight = _borderBox.transform.localScale.x / 2 - 0.5f;
        newPosition.x = Mathf.Clamp(newPosition.x, boundaryLeft, boundaryRight);
        _rb.MovePosition(newPosition);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb == null) return;
        var newDirection = GetNewDirection(other);
        UpdateValues();
        var movement = Input.GetAxisRaw("Horizontal");
        var moveMul = Math.Pow(movement, 2) + 1;
        var relativeVelocity = other.relativeVelocity.magnitude;
        var otherVelocity = otherRb.linearVelocity.magnitude;
        var temperedMagnitude = (float)(relativeVelocity / moveMul);
        if (otherVelocity <= _minSpeed / 2)
            temperedMagnitude = (float)(relativeVelocity / moveMul) * bouncingMultiplier;

        otherRb.linearVelocity = newDirection * Mathf.Max(_minSpeed / 2, temperedMagnitude);
    }


    private void UpdateValues()
    {
        var text = counter.text;
        counter.text = (int.Parse(text) + 1).ToString();
        _floatRoundMul = float.Parse(text) / 5f;
        _intRoundMul = (int)_floatRoundMul;
        bouncingMultiplier = (float)Math.Pow(1.15, _intRoundMul);
        _minSpeed = 5 * bouncingMultiplier;
        _maxSpeed = 7 + _minSpeed;
    }

    private Vector2 GetNewDirection(Collision2D other)
    {
        var objHitPosition = other.contacts[0].point;
        var paddlePosition = _rb.position;
        var hitOffsetFromCenter = objHitPosition.x - paddlePosition.x;
        var yPoint = Math.Sqrt(Math.Pow(_fullPaddleWidth / 2, 2) - Math.Pow(hitOffsetFromCenter, 2));
        var angle = Math.Atan2(yPoint, hitOffsetFromCenter);
        var newDirection = new Vector2(Mathf.Cos((float)angle), Mathf.Sin((float)angle)).normalized;
        return newDirection;
    }
}