using UnityEngine;

public class BorderBoxScript : MonoBehaviour
{
    private Camera _camera;

    private Collider2D _collider;
    private float _height;
    private Transform _transform;
    private float _width;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _camera = Camera.main;
        _transform = gameObject.transform;
        if (_camera == null) return;
        _width = _camera.orthographicSize * 2 * _camera.aspect - 1;
        _height = _camera.orthographicSize * 2 - 1;
        _transform.localScale = new Vector3(_width, _height, 1f);
        _collider.transform.localScale = new Vector3(_width, _height, 1f);
    }

    private void Update()
    {
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;
        var position = rb.position;
        var width = _width;
        var height = _height;
        var outsideRight = position.x >= width / 2;
        var outsideLeft = position.x <= -width / 2;
        var outsideTop = position.y >= height / 2;
        if (outsideRight || outsideLeft) rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
        if (outsideTop) rb.linearVelocity = new Vector2(rb.linearVelocity.x, -rb.linearVelocity.y);
    }
}