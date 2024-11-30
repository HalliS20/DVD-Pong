using TMPro;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public TextMeshProUGUI counter;
    public GameObject paddle;
    [SerializeField] private Vector2 moveDirection = new(1f, 1f);
    private GameObject _borderBox;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _borderBox = GameObject.FindGameObjectsWithTag("BorderBox")[0];


        if (_rb != null) _rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        ColorChange();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var height = _borderBox.transform.localScale.y;
        var outsideBottom = _rb.position.y <= -height / 2;

        if (outsideBottom)
        {
            var position = new Vector2(0, 0);
            _rb.position = position;
            moveDirection = new Vector2(1f, 1f);
            _rb.linearVelocity = moveDirection.normalized * moveSpeed;
            counter.text = "0";
            moveSpeed = 5;
        }

        ColorChange();
    }

    private void ColorChange()
    {
        var color = GetColor();
        while (color == _sprite.color) color = GetColor();
        _sprite.color = color;
    }

    private static Color GetColor()
    {
        var greenSpec = Random.Range(0, 2);
        var redSpec = Random.Range(0, 2);
        var blueSpec = Random.Range(0, 2);
        if (blueSpec + redSpec + greenSpec == 0)
        {
            blueSpec = 1;
            redSpec = 1;
            greenSpec = 1;
        }

        var color = new Color(redSpec, greenSpec, blueSpec);
        return color;
    }
}