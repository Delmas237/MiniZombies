using UnityEngine;

public class ItemLevitation : MonoBehaviour
{
    [SerializeField] private Vector3 moveArea;
    private Vector3 startPos;
    [SerializeField] private Vector3 speed;
    private Vector3 currentSpeed;
    [SerializeField] private Vector3 rotationSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        currentSpeed = speed;
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
            MoveInDir(ref currentSpeed.x, speed.x, startPos.x, transform.position.x, moveArea.x);
            MoveInDir(ref currentSpeed.y, speed.y, startPos.y, transform.position.y, moveArea.y);
            MoveInDir(ref currentSpeed.z, speed.z, startPos.z, transform.position.z, moveArea.z);

            rb.velocity = currentSpeed;
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
            transform.Rotate(rotationSpeed);
    }
}
