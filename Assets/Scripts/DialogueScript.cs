using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json.Linq;

public class Dialogue
{
    public string Speaker { get; set; }
}

public class SpeechDialogue : Dialogue
{
    public string Speech { get; set; }
}

public class OptionDialogue : Dialogue
{
    public string[] Options { get; set; }
}

public class DialogueScript : MonoBehaviour
{
    private Queue<JObject> dialogueQueue;
    private Action<int> onComplete;
    public GameObject Speech;
    public GameObject Speaker;
    public GameObject SpeechDialogue;
    public GameObject OptionDialogue;
    public GameObject OptionScrollContent;
    public GameObject Option;
    private JObject dialogues;
    private bool skipFirstIteration;
    private Dialogue currentDialogue;
    private bool doDialog;
    private bool waitOnOption;
    private string scenarioTitle;
    private int result;
    public DialogueScript()
    {
        string dialogueText = File.ReadAllText("./Assets/Data/Dialogues.json");
        this.dialogues = JObject.Parse(dialogueText);
        this.doDialog = false;
    }
    private Dialogue dequeueNextDialog()
    {
        var currentDialog = this.dialogueQueue.Dequeue();
        if (currentDialog.ToObject<OptionDialogue>().Options != null)
        {
            return currentDialog.ToObject<OptionDialogue>();
        }
        else if (currentDialog.ToObject<SpeechDialogue>().Speech != null)
        {
            return currentDialog.ToObject<SpeechDialogue>();
        }
        else
        {
            throw new Exception("was not able to parse next dialog in queue as a Dialog");
        }
    }
    private void doOption(int num)
    {
        this.scenarioTitle = $"{this.scenarioTitle}_{num}";
        this.result = num;
        var jArray = (JArray)this.dialogues.GetValue(this.scenarioTitle);
        this.dialogueQueue = new Queue<JObject>(jArray.ToObject<JObject[]>());
        this.currentDialogue = dequeueNextDialog();
        this.skipFirstIteration = true;
        this.waitOnOption = false;
        this.doDialog = true;
    }
    private void updateUI(Dialogue dialog)
    {
        if (dialog is SpeechDialogue)
        {
            this.OptionDialogue.SetActive(false);
            this.Speech.GetComponent<TextMeshProUGUI>().text = (dialog as SpeechDialogue).Speech;
            this.Speaker.GetComponent<TextMeshProUGUI>().text = (dialog as SpeechDialogue).Speaker;
            this.SpeechDialogue.SetActive(true);
        }
        else if (dialog is OptionDialogue)
        {
            OptionDialogue optionDialog = dialog as OptionDialogue;
            this.SpeechDialogue.SetActive(false);
            GameObject prevOption = Instantiate(Option, this.OptionScrollContent.transform.position, new Quaternion());
            prevOption.transform.SetParent(this.OptionScrollContent.transform);
            prevOption.transform.position = new Vector3(726, 472, 0);
            prevOption.GetComponentInChildren<TextMeshProUGUI>().text = optionDialog.Options[0];
            prevOption.GetComponent<Button>().onClick.AddListener(() => doOption(1));
            for (int i = 1; i < optionDialog.Options.Length; i++)
            {
                int copy = i + 1;
                GameObject newOption = Instantiate(Option, prevOption.transform.position, new Quaternion());
                newOption.transform.SetParent(this.OptionScrollContent.transform);
                newOption.transform.Translate(new Vector3(0, -50, 0));
                newOption.GetComponentInChildren<TextMeshProUGUI>().text = optionDialog.Options[i];
                newOption.GetComponent<Button>().onClick.AddListener(() => doOption(copy));
                prevOption = newOption;
            }
            this.doDialog = false;
            this.waitOnOption = true;
            this.OptionDialogue.SetActive(true);
        }
        else
        {
            throw new Exception("dialog is neither option nor speech");
        }
    }
    public void BeginDialogue(string name, Action<int> onComplete)
    {
        this.scenarioTitle = name;
        this.result = 0;
        var jArray = (JArray)this.dialogues.GetValue(name);
        this.dialogueQueue = new Queue<JObject>(jArray.ToObject<JObject[]>());
        this.onComplete = onComplete;
        this.currentDialogue = dequeueNextDialog();
        this.skipFirstIteration = true;
        this.doDialog = true;
    }
    private void cleanup()
    {
        this.doDialog = false;
        this.OptionDialogue.SetActive(false);
        this.SpeechDialogue.SetActive(false);
    }
    void Update()
    {
        if (this.doDialog)
        {
            updateUI(this.currentDialogue);
            if (Input.GetMouseButtonDown(0) && !this.skipFirstIteration && !this.waitOnOption)
            {
                if (this.dialogueQueue.Count == 0)
                {
                    this.onComplete(this.result);
                    cleanup();
                }
                else
                {
                    this.currentDialogue = dequeueNextDialog();
                }
            }
            this.skipFirstIteration = false;
        }
    }
}
