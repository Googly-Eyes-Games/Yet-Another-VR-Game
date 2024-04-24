using UnityEngine;
using UnityTimer;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 5f;
    
    void Start()
    {
        Timer.Register(lifeTime, () =>
        {
            Destroy(this);
        }, autoDestroyOwner: this);
    }
}
