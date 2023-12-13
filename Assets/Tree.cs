using UnityEngine;

public class ResourceNode : MonoBehaviour
{
  public WoodManager woodManager;
  public int woodAmount = 20;

  void Start()
  {
    woodManager = GameObject.Find("Canvas").GetComponent<WoodManager>(); // Adjust this based on your scene hierarchy
  }

  public int GatherWood(int amount)
  {
    int woodToGather = Mathf.Min(amount, woodAmount);
    woodAmount -= woodToGather;

    if (woodManager != null)
    {
      woodManager.AddWood(woodToGather);
    }

    if (woodAmount <= 0)
    {
      Destroy(gameObject);
    }

    return woodToGather;
  }
}
