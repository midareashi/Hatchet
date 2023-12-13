using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 5f;

  void Update()
  {
    // Basic movement example (you can customize this)
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
  }
}
