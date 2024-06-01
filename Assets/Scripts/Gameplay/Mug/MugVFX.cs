using UnityEngine;

[RequireComponent(typeof(MugComponent))]
public class MugVFX : MonoBehaviour
{
    [SerializeField]
    private GameObject mugDestroyPrefab;

    private void Awake()
    {
        MugComponent mug = GetComponent<MugComponent>();
        mug.OnDestroy += HandleMugDestroy;
    }

    private void HandleMugDestroy(MugComponent brokenMug)
    {
        Instantiate(mugDestroyPrefab, brokenMug.transform.position, brokenMug.transform.rotation);
    }
}