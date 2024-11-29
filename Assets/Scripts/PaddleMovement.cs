using Unity.VisualScripting;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody2D _rb;
    private PolygonCollider2D _collider;
    [SerializeField] private float startX = 0f;
    [SerializeField] private float startY = -3f;

    private GameObject _borderBox;
    private void Start()
    {
        _borderBox = GameObject.FindGameObjectsWithTag("BorderBox")[0];
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<PolygonCollider2D>();


        if (_rb != null)
        {
            transform.position = new Vector2(startX, startY);

            _rb.gravityScale = 0f;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        

    }


    private void Update()
    {
        var moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 newPosition = _rb.position + new Vector2(moveInput * moveSpeed * Time.fixedDeltaTime, 0f);
        var boundaryLeft = -_borderBox.transform.localScale.x / 2 + 0.5f;
        var boundaryRight = _borderBox.transform.localScale.x / 2 - 0.5f;
        newPosition.x = Mathf.Clamp(newPosition.x, boundaryLeft, boundaryRight);
        _rb.MovePosition(newPosition);
    }
}
