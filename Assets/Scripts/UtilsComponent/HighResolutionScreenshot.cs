using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HighResolutionScreenshot : MonoBehaviour
{
    [Button]
    void Take()
    {
        ScreenCapture.CaptureScreenshot("ScreenShot.png", 4);
    }
}