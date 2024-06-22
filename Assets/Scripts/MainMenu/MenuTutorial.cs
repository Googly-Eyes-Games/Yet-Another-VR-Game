using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuTutorial : MonoBehaviour
{
    [SerializeField]
    private MugComponent mug;

    [SerializeField]
    private MenuMugSpawn mugSpawn;

    [SerializeField]
    private MenuMugDetector mugDetector;

    [SerializeField]
    private TMP_Text mugTutorialText;
    
    [SerializeField]
    private TMP_Text leverTutorialText;
    
    [SerializeField]
    private TMP_Text throwTutorialText;
    
    [SerializeField]
    private TMP_Text emptyMugText;

    private TMP_Text[] allText;
    
    void Start()
    {
        XRGrabInteractable grabInteractable = mug.GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HandleMugGrabbed);

        mug.OnFill += HandleMugFilled;
        mugSpawn.OnSpawn.AddListener(HandleMugSpawn);
        
        mugDetector.onMugDetected.AddListener(HandleMugDetected);

        allText = new[] { mugTutorialText, leverTutorialText, throwTutorialText, emptyMugText };
    }
    
    private void HandleMugGrabbed(SelectEnterEventArgs arg0)
    {
        mugTutorialText.enabled = false;
        leverTutorialText.enabled = true;
    }
    
    private void HandleMugFilled(MugComponent obj)
    {
        leverTutorialText.enabled = false;
        throwTutorialText.enabled = true;
    }
    
    public void HandleMugSpawn()
    {
        foreach (TMP_Text text in allText)
        {
            text.enabled = false;
        }

        mugTutorialText.enabled = true;
    }
    
    private void HandleMugDetected(MugComponent mug)
    {
        if (mug.FillPercentage > GameplaySettings.Global.MinimalMugFillAmount)
        {
            TransitionsSceneManger.Get().LoadLevel();
        }
        else
        {
            throwTutorialText.enabled = false;
            emptyMugText.enabled = true;
        }
    }
    
}
