using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce = 10.0f;
    public bool bJump = false;
    private Rigidbody body;

    private bool isJumping = false;
    private bool isRotating = false;
    private float rotationTimer = 0f;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            StartCoroutine(JumpRoutine());
        }

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !isRotating)
        {
            StartCoroutine(RotateRoutine());
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;
        body.velocity = inputDir * moveSpeed;
        transform.LookAt(transform.position + inputDir);
    }

    IEnumerator JumpRoutine()
    {
        isJumping = true;
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(4f);

        isJumping = false;
    }

    IEnumerator RotateRoutine()
    {
        isRotating = true;
        float startAngle = transform.eulerAngles.y;
        float targetAngle = startAngle + 180f;
        float elapsedTime = 0f;

        while (elapsedTime < 4f)
        {
            float t = elapsedTime / 4f;
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, t);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isRotating = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bJump = false;
        }
    }
}

