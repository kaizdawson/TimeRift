using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping = false;
    private bool isDialogueActive = false;
    private bool skipRequested = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        if (!isDialogueActive)
        {
            StartDialogue();
            return;
        }

        if (isTyping)
        {
            skipRequested = true;
            return;
        }

        NextLine();
    }

    void StartDialogue()
    {
        RebindUIReferences();

        if (dialoguePanel == null || nameText == null || dialogueText == null || portraitImage == null)
        {
            Debug.LogError("UI references missing. Dialogue cannot start.");
            return;
        }

        isDialogueActive = true;
        dialogueIndex = 0;
        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        skipRequested = false;
        dialogueText.SetText("");

        string line = dialogueData.dialogueLines[dialogueIndex];

        foreach (char letter in line)
        {
            if (skipRequested)
            {
                dialogueText.SetText(line);
                break;
            }

            dialogueText.text += letter;

            if (dialogueData.voiceSound != null)
            {
                audioSource.pitch = dialogueData.voicePitch;
                audioSource.PlayOneShot(dialogueData.voiceSound);
            }

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (!skipRequested &&
            dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }

    private void RebindUIReferences()
    {
        var canvas = GameObject.Find("UICanvas");
        if (canvas == null)
        {
            Debug.LogError("UICanvas not found in scene.");
            return;
        }

        dialoguePanel = canvas.transform.Find("DialoguePanel")?.gameObject;
        if (dialoguePanel != null)
        {
            portraitImage = dialoguePanel.transform.Find("DialoguePortrait")?.GetComponent<Image>();
            nameText = dialoguePanel.transform.Find("NPCNameText")?.GetComponent<TMP_Text>();
            dialogueText = dialoguePanel.transform.Find("DialogueText")?.GetComponent<TMP_Text>();
        }
    }


    public bool CanInteract()
    {
        return true;    
    }
}
