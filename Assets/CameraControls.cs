using UnityEngine;

public class CameraController : MonoBehaviour
{
  public float moveSpeed = 10f;
  public float edgeScrollSpeed = 15f;

  void Update()
  {
    // Get the forward and right vectors in world space
    Vector3 forward = transform.forward;
    Vector3 right = transform.right;

    // Flatten the vectors (ignore the Y component)
    forward.y = 0f;
    right.y = 0f;
    forward.Normalize();
    right.Normalize();

    // Move camera with arrow keys
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

    /*
    // Move camera with mouse at screen edges
    if (Input.mousePosition.x <= 0)
      moveDirection += Vector3.left;
    else if (Input.mousePosition.x >= Screen.width - 1)
      moveDirection += Vector3.right;

    if (Input.mousePosition.y <= 0)
      moveDirection += Vector3.back;
    else if (Input.mousePosition.y >= Screen.height - 1)
      moveDirection += Vector3.forward;
    */
    // Normalize the direction to prevent faster diagonal movement
    moveDirection = moveDirection.normalized;

    // Translate the camera position
    transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

    // If you want the camera to rotate with arrow keys, uncomment the next two lines
    // float rotationInput = Input.GetAxis("Rotation");
    // transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
  }
}
