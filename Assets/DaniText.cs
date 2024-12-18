using System.Collections;
using TMPro;
using UnityEngine;

public class DaniText : MonoBehaviour
{
    public TextMeshProUGUI textDisplay; // The TextMeshPro component
    public TMP_FontAsset alternateFont; // Font for random letters
    public TMP_FontAsset finalFont; // Font for finalized letters
    public string[] sentences; // Sentences to display
    public float typingSpeed = 0.02f; // Delay between finalized letters
    public int randomLetterSteps = 6; // Number of random iterations per letter

    private int index = 0; // Current sentence index

    void Start()
    {
        if (textDisplay == null || alternateFont == null || finalFont == null || sentences.Length == 0)
        {
            return;
        }
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        yield return new WaitForSeconds(1f); // Initial delay

        string currentText = ""; // Text finalized so far

        foreach (char letter in sentences[index].ToCharArray())
        {

            // Iterate through random letters
            for (int i = 0; i < randomLetterSteps; i++)
            {
                char randomLetter = GetRandomLetter(letter);
                textDisplay.text = GetFormattedText(currentText, randomLetter); // Update with random letter

                yield return new WaitForSeconds(typingSpeed / randomLetterSteps);
            }

            // Add finalized letter to the current text
            currentText += letter;
            textDisplay.text = GetFormattedText(currentText, null); // Update with finalized text

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    char GetRandomLetter(char finalLetter)
    {
        char randomLetter;
        do
        {
            randomLetter = (char)Random.Range(32, 127); // Printable ASCII range
        } while (randomLetter == finalLetter); // Ensure random letter isn't the final letter

        return randomLetter;
    }

    string GetFormattedText(string finalizedText, char? currentLetter)
    {
        string formattedText = $"<font=\"{finalFont.name}\">{finalizedText}</font>"; // Finalized letters in `finalFont`

        if (currentLetter.HasValue)
        {
            // Add the current letter in `alternateFont` if it's still iterating
            formattedText += $"<font=\"{alternateFont.name}\">{currentLetter}</font>";
        }

        return formattedText;
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
        }
    }
}
