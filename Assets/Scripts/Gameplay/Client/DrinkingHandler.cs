using UnityEngine;

public class DrinkingHandler : MonoBehaviour
{
    [SerializeField]
    private float liquidToDrinkEvent;

    [SerializeField]
    private Vector2 swallowPitchRange = Vector2.one;

    [SerializeField]
    private AudioSource drinkingAS;
    
    private LiquidHole hole;

    private float lastDrinkEventLiquid = 0.0f;
    
    private void Awake()
    {
        hole = GetComponent<LiquidHole>();
    }

    void Update()
    {
        if (hole.CollectedLiquid - lastDrinkEventLiquid > liquidToDrinkEvent)
        {
            Drink();
            lastDrinkEventLiquid = hole.CollectedLiquid;
        }
    }

    void Drink()
    {
        drinkingAS.pitch = Random.Range(swallowPitchRange.x, swallowPitchRange.y);
        drinkingAS.Play();
    }
}
