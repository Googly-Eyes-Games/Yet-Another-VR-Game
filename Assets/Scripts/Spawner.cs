using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MugSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject mugPrefab;

    [SerializeField]
    private AudioSource ringAudioSource;

    public HashSet<MugComponent> SpawnedMugs { get; private set; } = new();

    private int targetMugsAmount = 1;

    public void SetTargetMugsAmount(int newValue)
    {
        targetMugsAmount = newValue;
        SpawnMugsToTargetAmount();
    }

    public void Awake()
    {
        SpawnMugsToTargetAmount();
    }

    public GameObject SpawnMug()
    {
        GameObject spawnedGameObject = Instantiate(mugPrefab, transform.position, transform.rotation, transform);

        MugComponent mug = spawnedGameObject.GetComponent<MugComponent>();
        SpawnedMugs.Add(mug);
        mug.OnDestroy += HandleMugDestroyed;
        
        ringAudioSource.Play();

        return spawnedGameObject;
    }

    private void HandleMugDestroyed(MugComponent destroyedMug)
    {
        SpawnedMugs.Remove(destroyedMug);

        SpawnMugsToTargetAmount();
    }

    private void SpawnMugsToTargetAmount()
    {
        int activeMugs = SpawnedMugs.Count;
        for (int mugID = activeMugs; mugID < targetMugsAmount; mugID++)
        {
            SpawnMug();
        }
    }
}
