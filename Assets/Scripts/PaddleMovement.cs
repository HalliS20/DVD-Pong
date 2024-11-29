using System;
using TMPro;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float startX;
    [SerializeField] private float startY = -3f;
    [SerializeField] private float bouncingMultiplier = 1f;
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 12f;
    public TextMeshProUGUI counter;


    private GameObject _borderBox;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;

    private void Start()
    {
        _borderBox = GameObject.FindGameObjectsWithTag("BorderBox")[0];
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();


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
        var objHitPosition = other.contacts[0].point;
        var paddlePosition = _rb.position;
        var hitOffsetFromCenter = objHitPosition.x - paddlePosition.x;
        var fullPaddleWidth = _collider.bounds.size.x;

        const double angleDec = 0;
        var yPoint = Math.Sqrt(Math.Pow(fullPaddleWidth / (2 - angleDec), 2) - Math.Pow(hitOffsetFromCenter, 2));
        var angle = Math.Atan2(yPoint, hitOffsetFromCenter);
        var newDirection = new Vector2(Mathf.Cos((float)angle), Mathf.Sin((float)angle)).normalized;

        var text = counter.text;
        counter.text = (int.Parse(text) + 1).ToString();

        var floatRoundMul = float.Parse(text) / 5f;
        var intRoundMul = (int)floatRoundMul;
        bouncingMultiplier = (float)Math.Pow(1.15, intRoundMul);
        minSpeed = 5 * bouncingMultiplier;
        maxSpeed = 12 * bouncingMultiplier;
        var movement = Input.GetAxisRaw("Horizontal");
        var moveMul = Math.Pow(movement, 2) + 1;
        var relativeVelocity = other.relativeVelocity.magnitude;
        var otherVelocity = otherRb.linearVelocity.magnitude;
        var temperedMagnitude = (float)(relativeVelocity / moveMul);
        if ((intRoundMul - floatRoundMul == 0 && otherVelocity <= maxSpeed) || otherVelocity <= minSpeed)
            temperedMagnitude = (float)(relativeVelocity / moveMul) * bouncingMultiplier;

        otherRb.linearVelocity = newDirection * Mathf.Max(minSpeed / 2, temperedMagnitude);
    }
}