using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class DaniText : MonoBehaviour
{

    public enum DaniTextType
    {
        LETTER,
        WORD

    }

    public TextMeshProUGUI textDisplay; // The TextMeshPro component
    public TMP_FontAsset alternateFont; // Font for random letters
    public TMP_FontAsset finalFont; // Font for finalized letters
    [TextArea(10, 10)]
    public string[] sentences; // Sentences to display
    public float typingSpeed = 0.02f; // Delay between finalized letters
    public int randomLetterSteps = 6; // Number of random iterations per letter
    public bool startOnLoad = false;
    public DaniTextType type = DaniTextType.LETTER;

    private int index = 0; // Current sentence index


    void Start()
    {
        if (startOnLoad)
        {
            StartCoroutine(Type());
        }
        //if (textDisplay == null || alternateFont == null || finalFont == null || sentences.Length == 0)
        //{
        //    return;
        //}
        //StartCoroutine(Type());
    }

    //private void OnEnable()
    //{
    //    DoTyping();
    //}

    public void setText(string input)
    {
        StopCoroutine(Type());
        textDisplay.text = "";
        if(input == null || input == "")
        {
            sentences = new[] {""};
        }
        else
        {
            //sentences = input
            //    .Split(new[] { '.', '!', '?', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            //    .Select(sentence => sentence.Trim())
            //    .Where(sentence => !string.IsNullOrEmpty(sentence))
            //    .ToArray()
            //    ;
            sentences = new string[] {input};
            StartCoroutine(Type());
        }

    }

    private void DoTyping()
    {
        StopCoroutine(Type());
        textDisplay.text = "";
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        //yield return new WaitForSeconds(1f); // Initial delay
        //yield return new WaitForSeconds(typingSpeed);

        string currentText = ""; // Finalized text so far


        if (type == DaniTextType.LETTER)
        {
            foreach (char letter in sentences[index].ToCharArray())
            {

                // Iterate through random letters
                for (int i = 0; i < randomLetterSteps; i++)
                {
                    string randomLetter = GetRandomLetter(letter);
                    textDisplay.text = GetFormattedText(currentText, randomLetter); // Update with random letter

                    yield return new WaitForSeconds(typingSpeed / randomLetterSteps);
                }

                // Add finalized letter to the current text
                currentText += letter;
                textDisplay.text = GetFormattedText(currentText, null); // Update with finalized text

                //yield return new WaitForSeconds(typingSpeed);
            }
        }

        if(type == DaniTextType.WORD)
        {
            string[] words = sentences[index].Split(' '); // Split sentence into words

            foreach (string word in words)
            {
                // Iterate through random letters for the word
                for (int i = 0; i < randomLetterSteps; i++)
                {
                    string randomWord = GetRandomWord(word.Length);
                    textDisplay.text = GetFormattedText(currentText, randomWord); // Update with random word
                    yield return new WaitForSeconds(typingSpeed / randomLetterSteps);
                }

                // Add finalized word to the current text
                currentText += word + " ";
                textDisplay.text = GetFormattedText(currentText.TrimEnd(), null); // Update with finalized text

            }
        }




    }

    string GetRandomLetter(char finalLetter)
    {
        char randomLetter;
        do
        {
            randomLetter = (char)UnityEngine.Random.Range(32, 127); // Printable ASCII range
        } while (randomLetter == finalLetter); // Ensure random letter isn't the final letter

        return randomLetter.ToString();
    }

    string GetRandomWord(int length)
    {
        char[] randomWord = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomWord[i] = (char)UnityEngine.Random.Range(32, 127); // Printable ASCII range
        }

        return new string(randomWord);
    }

    string GetFormattedText(string finalizedText, string? currentLetter)
    {
        string formattedText = $"<font=\"{finalFont.name}\">{finalizedText}</font>"; // Finalized letters in `finalFont`

        if (currentLetter != null)
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
