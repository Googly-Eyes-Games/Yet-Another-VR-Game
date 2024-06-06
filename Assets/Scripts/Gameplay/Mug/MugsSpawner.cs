using erulathra;
using UnityEngine;
using UnityEngine.Pool;

public class MugSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject mugPrefab;

    [SerializeField]
    private AudioSource ringAudioSource;
    
    [SerializeField]
    private int mugsPoolCapacity = 10;

    private ObjectPool<MugComponent> mugsPool;

    private int targetMugsAmount = 1;

    public void SetTargetMugsAmount(int newValue)
    {
        targetMugsAmount = newValue;
        SpawnMugsToTargetAmount();
    }

    public void SpawnMug()
    {
        MugComponent mug = mugsPool.Get();
        mug.transform.position = transform.position;
        mug.transform.rotation = transform.rotation;
        
        ringAudioSource.Play();
    }

    private void Awake()
    {
        mugsPool = new ObjectPool<MugComponent>(SpawnAndInitializeMug, OnGetMug, OnReleaseMug, defaultCapacity: mugsPoolCapacity);
        
        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        levelSubsystem.OnNextLevel += OnLevelChanged;
        
        OnLevelChanged(0);
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
            SpawnMug();
        }
    }
}
