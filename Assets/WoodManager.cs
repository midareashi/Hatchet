using UnityEngine;
using UnityEngine.UI;

public class WoodManager : MonoBehaviour
{
  public Text woodText; // Reference to the Text GameObject

  private int woodCount = 0;

  void Start()
  {
    UpdateWoodText();
  }

  void UpdateWoodText()
  {
    woodText.text = "Wood: " + woodCount;
  }

  public void AddWood(int amount)
  {
    woodCount += amount;
    UpdateWoodText();
  }
}
