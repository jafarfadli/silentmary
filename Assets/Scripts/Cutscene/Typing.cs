using System.Collections;
using UnityEngine;
using TMPro;

public class Typing : MonoBehaviour
{
    public float timeBetweenCharacters = 0.05f;

    private string fullText;
    private Coroutine typingCoroutine;

    public void StartTyping(string textToType, TMP_Text textComponent)
    {
        fullText = textToType;
        
        // Stop any running typing routine to avoid overlaps
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeTextRoutine(textComponent));
    }

    IEnumerator TypeTextRoutine(TMP_Text textComponent)
    {
        textComponent.text = fullText;
        textComponent.maxVisibleCharacters = 0;

        // Force an immediate mesh update to parse rich text and calculate total characters accurately
        textComponent.ForceMeshUpdate(); 
        int totalVisibleCharacters = textComponent.textInfo.characterCount;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }
}
