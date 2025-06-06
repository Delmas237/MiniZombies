using UnityEngine;

public class ItemLevitation : MonoBehaviour
{
    [SerializeField] private Vector3 _moveArea;
    [SerializeField] private Vector3 _speed;
    [SerializeField] private Vector3 _rotationSpeed;
    private Vector3 _startPos;
    private Vector3 _currentSpeed;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _startPos = transform.position;
        _currentSpeed = _speed;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if (Time.timeScale > 0)
        {
            MoveInDir(ref _currentSpeed.x, _speed.x, _startPos.x, transform.position.x, _moveArea.x);
            MoveInDir(ref _currentSpeed.y, _speed.y, _startPos.y, transform.position.y, _moveArea.y);
            MoveInDir(ref _currentSpeed.z, _speed.z, _startPos.z, transform.position.z, _moveArea.z);

            _rb.velocity = _currentSpeed;
        }
    }

    private void MoveInDir(ref float currentSpeed, float speed, float startPos, float pos, float moveArea)
    {
        if (pos > startPos + moveArea / 2)
            currentSpeed = -speed;
        else if (pos < startPos - moveArea / 2)
            currentSpeed = speed;
    }

    private void Rotate()
    {
        if (Time.timeScale > 0)
            transform.Rotate(_rotationSpeed);
    }
}
