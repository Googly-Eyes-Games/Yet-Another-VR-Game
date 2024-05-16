using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "SO_MusicConfig_MC", menuName = "SFG/MusicConfig", order = 0)]
    public class MusicConfig : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip audioClip { get; private set; }
        
        [field: SerializeField]
        public float bmp { get; private set; }
    }
}