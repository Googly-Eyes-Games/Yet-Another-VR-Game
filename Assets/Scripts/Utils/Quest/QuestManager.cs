using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Get()
    {
        if (instance)
            return instance;

        GameObject questManagerGameObject = new GameObject
        {
            name = "QuestManager"
        };
        
        DontDestroyOnLoad(questManagerGameObject);

        instance = questManagerGameObject.AddComponent<QuestManager>();
        instance.ApplyDefaultSettings();

        return instance;
    }
    
    private void ApplyDefaultSettings()
    {
        OVRPlugin.systemDisplayFrequency = 90f;
        OVRPlugin.foveatedRenderingLevel = OVRPlugin.FoveatedRenderingLevel.Medium;

        OVRPlugin.RecenterTrackingOrigin(OVRPlugin.RecenterFlags.Default);
    }
    
    static class QuestSettingsApplierInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Initialize()
        {
            QuestManager.Get();
        }
    }
}
