using System;
using erulathra;
using UnityEngine;
using UnityEngine.Pool;
using UnityTimer;

public class MugSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject mugPrefab;

    [SerializeField]
    private AudioSource ringAudioSource;
    
    [SerializeField]
    private Vector3 spawnVelocity;
    
    [SerializeField]
    private float timeBetweenSpawns = 1f;
    
    [SerializeField]
    private int mugsPoolCapacity = 10;

    private ObjectPool<MugComponent> mugsPool;

    private int targetMugsAmount = 1;

    private int mugsToSpawn;
    
    
    public void SetTargetMugsAmount(int newValue)
    {
        targetMugsAmount = newValue;
        SpawnMugsToTargetAmount();
    }


    private void Awake()
    {
        mugsPool = new ObjectPool<MugComponent>(SpawnAndInitializeMug, OnGetMug, OnReleaseMug, defaultCapacity: mugsPoolCapacity);
        
        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        levelSubsystem.OnNextLevel += OnLevelChanged;
        
        OnLevelChanged(0);

        Timer.Register(timeBetweenSpawns, SpawnTick, isLooped: true, autoDestroyOwner: this);
    }

    private void SpawnTick()
    {
        if (mugsToSpawn > 0)
        {
            SpawnMug();
            mugsToSpawn--;
        }
    }

    private void OnLevelChanged(int levelIndex)
    {
        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        SetTargetMugsAmount(levelSubsystem.CurrentLevelConfig.MaxClients + 1);
    }

    private MugComponent SpawnAndInitializeMug()
    {
        GameObject spawnedGameObject = Instantiate(mugPrefab, transform.position, transform.rotation, transform);
        MugComponent mug = spawnedGameObject.GetComponent<MugComponent>();
        mug.OnDestroy += HandleMugDestroyed;
        mug.OnClean += HandleMugDestroyed;
        
        mug.gameObject.SetActive(false);

        return mug;
    }

    private void OnGetMug(MugComponent mug)
    {
        mug.gameObject.SetActive(true);
    }
    
    private void OnReleaseMug(MugComponent mug)
    {
        mug.gameObject.SetActive(false);
        mug.FillPercentage = 0f;
        mug.IsClean = true;

        Rigidbody mugRigidbody = mug.GetComponent<Rigidbody>();
        mugRigidbody.velocity = Vector3.zero;
        mugRigidbody.angularVelocity = Vector3.zero;
    }

    private void HandleMugDestroyed(MugComponent mug)
    {
        mugsPool.Release(mug);
        SpawnMugsToTargetAmount();
    }

    private void SpawnMugsToTargetAmount()
    {
        int activeMugs = mugsPool.CountActive;
        for (int mugID = activeMugs; mugID < targetMugsAmount; mugID++)
        {
            QueueMugSpawn();
        }
    }

    private void QueueMugSpawn()
    {
        mugsToSpawn++;
    }
    
    private void SpawnMug()
    {
        MugComponent mug = mugsPool.Get();
        
        Rigidbody mugRigidbody = mug.GetComponent<Rigidbody>();
        mugRigidbody.velocity = spawnVelocity;
        
        mugRigidbody.position = transform.position;
        mugRigidbody.rotation = transform.rotation;
        
        ringAudioSource.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawRay(transform.position, spawnVelocity.normalized);
    }
}
