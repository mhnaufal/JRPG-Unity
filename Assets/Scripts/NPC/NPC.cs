using Fungus;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public enum NPCState { Idle, Patrol, Talk };
    public NPCState currentState = NPCState.Patrol;
    private NPCState defaultState;
    public NPCPatrol patrol;
    public NPCTalk talk;

    [Header("Dialogue")]
    public Flowchart flowchart;
    public string dialogueBlock = "Meet LancerNPC";

    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    private void SwitchState(NPCState nPCState)
    {
        currentState = nPCState;

        patrol.enabled = nPCState == NPCState.Patrol;
        talk.enabled = nPCState == NPCState.Talk;
    }

    public void OnEnterRange() { }

    public void OnExitRange()
    {
        if (currentState == NPCState.Talk)
        {
            SwitchState(defaultState);
        }
    }

    public void Interact(PlayerController player)
    {
        if (currentState == NPCState.Talk)
        {
            return;
        }

        SwitchState(NPCState.Talk);
        flowchart.ExecuteBlock(dialogueBlock);
    }

}
