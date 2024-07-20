using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSoundManager : Singleton
{
    public List<EventReference> dialogueSequences;

    private Queue<EventReference> dialogueQueue = new Queue<EventReference>();

    private bool isPlaying = false;

    private int currentDialogueIndex = 0;

/*    private void Start()
    {
        // Enqueue all dialogue sequences
        foreach (var dialogue in dialogueSequences)
        {
            dialogueQueue.Enqueue(dialogue);
        }
    }*/

    public void PlayDialogueSequence(int index)
    {
        if (index < dialogueSequences.Count)
        {
            dialogueQueue.Enqueue(dialogueSequences[index]);
            if (!isPlaying)
            {
                StartCoroutine(PlayDialogue());
            }
        }
    }

    public void PlayNextDialogueSequence()
    {
        if (currentDialogueIndex < dialogueSequences.Count)
        {
            dialogueQueue.Enqueue(dialogueSequences[currentDialogueIndex]);
            currentDialogueIndex++;
            if (!isPlaying)
            {
                StartCoroutine(PlayDialogue());
            }
        }
    }

    private IEnumerator PlayDialogue()
    {
        while (dialogueQueue.Count > 0)
        {
            isPlaying = true;
            var sequence = dialogueQueue.Dequeue();
            EventInstance instance = RuntimeManager.CreateInstance(sequence);
            instance.start();

            // Wait for the sound to finish playing
            instance.getDescription(out EventDescription description);
            description.getLength(out int length);
            yield return new WaitForSeconds(length / 1000f); // Convert milliseconds to seconds

            instance.release();
        }
        isPlaying = false;
    }

    public void TriggerDialogueByEvent(int sequenceIndex)
    {
        PlayDialogueSequence(sequenceIndex);
    }
}
