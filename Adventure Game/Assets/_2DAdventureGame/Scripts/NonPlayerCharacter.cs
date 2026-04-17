using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject dialogueBubble;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueBubble.SetActive(false);
    }


}
