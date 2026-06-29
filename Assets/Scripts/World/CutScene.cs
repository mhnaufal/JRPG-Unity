using UnityEngine;
using Fungus;
using Unity.Cinemachine;
using System.Collections;

public class CutScene : MonoBehaviour
{
    public PlayerController player;
    public float walkDistance = 3f;
    public Flowchart flowchart;
    public string cutsceneBlock = "EngagingWithShaman";

    [Header("Cinematic")]
    public GameObject letterbox;
    public CinemachineBasicMultiChannelPerlin cameraNoise;
    public float shakeAmplitude = 2f;
    public float shakeDuration = 0.4f;
    private float targetX;
    private bool isWalking;

    void Start()
    {
        if (cameraNoise != null)
        {
            cameraNoise.AmplitudeGain = 0f;
        }

        if (letterbox != null)
        {
            letterbox.SetActive(true);
        }

        player.SetInputEnabled(false);
        targetX = player.transform.position.x + walkDistance;
        player.SetMove(1f, 0f);
        isWalking = true;
    }

    void Update()
    {
        if (!isWalking) return;

        if (player.transform.position.x >= targetX)
        {
            player.SetMove(0f, 0f);
            isWalking = false;
            flowchart.ExecuteBlock(cutsceneBlock);
        }
    }

    public void EndCutscene()
    {
        if (letterbox != null)
        {
            letterbox.SetActive(false);
        }

        player.SetInputEnabled(true);
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        if (cameraNoise == null)
        {
            yield break;
        }

        cameraNoise.AmplitudeGain = shakeAmplitude;
        yield return new WaitForSeconds(shakeDuration);
        cameraNoise.AmplitudeGain = 0f;
    }
}