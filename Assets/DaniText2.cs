using System.Collections;
using TMPro;
using UnityEngine;

public class DaniText2 : MonoBehaviour
{
    public TextMeshProUGUI textDisplay; // The TextMeshPro component
    public TMP_FontAsset alternateFont; // Font for random letters
    public TMP_FontAsset finalFont; // Font for finalized letters
    public string[] sentences; // Sentences to display
    public float typingSpeed = 0.02f; // Delay between finalized letters
    public float randomChangeDuration = 0.5f; // Duration to change random letters before settling

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
        string currentRandomText = ""; // Current random text (changing)

        // Create an array to hold the characters of the current sentence
        char[] currentChars = new char[sentences[index].Length];
        for (int i = 0; i < currentChars.Length; i++)
        {
            currentChars[i] = sentences[index][i]; // Initialize with the final characters
        }

        // Create an array to hold the randomly changing characters
        char[] randomChars = new char[sentences[index].Length];
        for (int i = 0; i < randomChars.Length; i++)
        {
            randomChars[i] = GetRandomLetter(); // Initialize with random characters
        }

        int steps = 0;
        // Loop to gradually reveal the final text
        while (steps < sentences[index].Length)
        {
            // For each step, randomly replace one character with the final one
            int randomIndex = Random.Range(0, sentences[index].Length); // Choose a random index to reveal
            randomChars[randomIndex] = currentChars[randomIndex];

            // Build the string from randomChars
            currentRandomText = new string(randomChars);
            textDisplay.text = GetFormattedText(currentRandomText);

            steps++; // Increase the number of steps
            yield return new WaitForSeconds(randomChangeDuration);
        }

        // Once all characters are revealed, finalize the text
        currentText = sentences[index];
        textDisplay.text = GetFormattedText(currentText);

        // Optionally, type the finalized text one letter at a time (optional effect)
        foreach (char letter in sentences[index].ToCharArray())
        {
            currentText += letter;
            textDisplay.text = GetFormattedText(currentText);
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    char GetRandomLetter()
    {
        // Generate a random letter from printable ASCII characters
        return (char)Random.Range(32, 127);
    }

    string GetFormattedText(string text)
    {
        // Format text using the alternateFont for the current random letters, and the finalFont for the real ones
        string formattedText = "";

        for (int i = 0; i < text.Length; i++)
        {
            // Check if the character is the final letter (if it's from the sentence, use the final font)
            if (text[i] == sentences[index][i])
            {
                formattedText += $"<font=\"{finalFont.name}\">{text[i]}</font>";
            }
            else
            {
                formattedText += $"<font=\"{alternateFont.name}\">{text[i]}</font>";
            }
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
