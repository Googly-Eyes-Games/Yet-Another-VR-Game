using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MenuMugSpawn : MonoBehaviour
{
    [SerializeField]
    public UnityEvent OnSpawn;
    
    [SerializeField]
    private Rigidbody mug;
    
    [SerializeField]
    private float spawnAnimationDuration = 0.25f;

    [SerializeField]
    private Vector3 startAngularVelocity = new (0f, 0.5f, 0f);
    

    private void OnEnable() 
    {
        SpawnMug();
    }

    public void SpawnMug()
    {
        mug.position = transform.position;
        mug.rotation = transform.rotation;
        mug.velocity = Vector3.zero;
        mug.angularVelocity = startAngularVelocity;
        mug.useGravity = false;

        MugComponent mugComponent = mug.GetComponent<MugComponent>();
        mugComponent.FillPercentage = 0f;
        
        OnSpawn?.Invoke();

        mug.transform.localScale = Vector3.zero;
        mug.transform.DOScale(1f, spawnAnimationDuration);
    }
}
