using UnityEngine;

public class Peon : MonoBehaviour
{
  public int carryingCapacity = 5;
  private int currentWood = 0;
  public float moveSpeed = 5f; // Adjust this to your desired movement speed

  private bool isReturning = false;
  private Vector3 destination;

  void Update()
  {
    if (isReturning)
    {
      MoveTowardsDestination();
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (!isReturning)
    {
      if (other.CompareTag("Tree"))
      {
        ResourceNode resourceNode = other.GetComponent<ResourceNode>();
        if (resourceNode != null)
        {
          int woodAmount = resourceNode.GatherWood(carryingCapacity - currentWood);
          currentWood += woodAmount;

          if (currentWood >= carryingCapacity)
          {
            ReturnToDestination(GameObject.Find("TownHall"));
          }
        }
      }
    }
  }


  void ReturnToDestination(GameObject target)
  {
    isReturning = true;
    destination = target.transform.position;
  }

  void MoveTowardsDestination()
  {
    float step = moveSpeed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, destination, step);

    if (transform.position == destination)
    {
      isReturning = false;
      currentWood = 0;
    }
  }
}
