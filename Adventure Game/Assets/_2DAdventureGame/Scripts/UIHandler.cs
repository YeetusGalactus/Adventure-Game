using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{

    public static UIHandler instance { get; private set; }
    private VisualElement m_Healthbar;

    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private Label bossDialogue;
    private float m_TimerDisplay;

    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;

    private Label m_RobotCounter;

    private Toggle m_BossCheck;


    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        bossDialogue = m_NonPlayerDialogue.Q<Label>("Label");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;


        m_LoseScreen = uiDocument.rootVisualElement.Q<VisualElement>("LoseScreenContainer");
        m_WinScreen = uiDocument.rootVisualElement.Q<VisualElement>("WinScreenContainer");

        m_RobotCounter = uiDocument.rootVisualElement.Q<Label>("CounterLabel");
        m_BossCheck = uiDocument.rootVisualElement.Q<Toggle>("BossCheckToggle");

    }

    private void Update() {
        if (m_TimerDisplay > 0) {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0) {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }


    public void SetHealthValue(float percentage) {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    public void DisplayDialogue() {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }

    public void DisplayBossDialogue() {
        bossDialogue.text = "Awesome, there's just one more thing. There is this huge boiler unit that's gone rogue too under the town, can you find it a fix it for us? Come and talk to me once you're done!";
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }

    public void SetCounter(int current, int enemies) {
        m_RobotCounter.text = $"{current} / {enemies}";
    }

    public void SetBossCheck(bool bossDead) {
        if(bossDead) m_BossCheck.value = true;
    }


    public void DisplayWinScreen() {
        m_WinScreen.style.opacity = 1.0f;
    }

    public void DisplayLoseScreen() {
        m_LoseScreen.style.opacity = 1.0f;
    }

}
