using UnityEngine;

public class PlayLegacyAnimationOnce : MonoBehaviour
{
    public Animation animationComponent;       // Komponent Animation
    public string animationName = "Open";      // Nazwa animacji do zagrania
    public string playerTag = "Player";        // Tag gracza

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag(playerTag))
        {
            if (animationComponent != null && animationComponent[animationName] != null)
            {
                animationComponent.Play(animationName);
                hasPlayed = true;
                Debug.Log("Legacy animation started.");
            }
            else
            {
                Debug.LogWarning("Animation or clip not assigned/found.");
            }
        }
    }
}
