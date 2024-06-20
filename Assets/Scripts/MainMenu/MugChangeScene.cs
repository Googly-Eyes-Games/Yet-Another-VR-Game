using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MugChangeScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            TransitionsSceneManger tsm = TransitionsSceneManger.Get();
            tsm.LoadScene("S_TestMap");
        }
    }
}
