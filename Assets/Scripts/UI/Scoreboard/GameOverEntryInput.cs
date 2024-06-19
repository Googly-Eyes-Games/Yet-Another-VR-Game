using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEntryInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TMP_InputField inputField;
    
    private int score;
    private string nick;
    
    void Start()
    {
        score = TransitionsSceneManger.Get().LastSceneScore;
        
        if (score < 0)
            return;

        scoreText.text = score.ToString();
        
        inputField.ActivateInputField();
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string inputNick)
    {
        nick = inputNick;
    }
    
    public void Next()
    {
        ScoreboardEntry scoreboardEntry = new ScoreboardEntry(
            nick,
            score
        );
        
        SaveManager.AddEntry(scoreboardEntry);
    }
}
