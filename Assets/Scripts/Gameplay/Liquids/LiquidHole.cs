using UnityEngine;

public class LiquidHole : MonoBehaviour
{
    public float CollectedLiquid { get; private set; }
    
    public void AddLiquid(float amount)
    {
        CollectedLiquid += amount;
    }
}