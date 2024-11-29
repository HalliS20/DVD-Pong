using UnityEngine;

public class BorderBoxScript : MonoBehaviour{

    private Collider2D _collider;
    private Camera _camera;
    private Transform _transform;
    private float _width;
    private float _height;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _camera = Camera.main;
        _transform = gameObject.transform;
        if (_camera == null) return;
        _width = _camera.orthographicSize * 2 * _camera.aspect-1;
        _height = _camera.orthographicSize * 2 -1;
        _transform.localScale = new Vector3(_width, _height, 1f);
        _collider.transform.localScale = new Vector3(_width, _height, 1f);
    }

    private void Update()
    {
        
    }
}
