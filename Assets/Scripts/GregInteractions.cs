using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GregInteractions : MonoBehaviour
{
    public GameObject Dialogue;
    bool doingDialogue = false;
    bool completed = false;
    private void completedDialog(int result) {
        doingDialogue = false;
        completed = true;
        if (result == 1) {
            Object.Destroy(gameObject);
        }
    }
    void OnMouseDown() {
        if (!doingDialogue && !completed) {
            Dialogue.GetComponent<DialogueScript>().BeginDialogue(
                "Greg",
                completedDialog
            );
        } else {
            Dialogue.GetComponent<DialogueScript>().BeginDialogue(
                "Greg_Completed",
                completedDialog
            );
        }
    }
}
