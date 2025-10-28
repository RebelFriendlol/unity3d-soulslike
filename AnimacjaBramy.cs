using UnityEngine;

public class TriggerLegacyAnimation : MonoBehaviour
{
    public string tagToFind = "AnimObject";      // Tag przypisany do 3 obiektów z animacj¹
    public string animationName = "PlayOnce";    // Nazwa legacy animacji

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tagToFind);
            foreach (GameObject obj in targets)
            {
                Animation anim = obj.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.Play(animationName);
                }
            }

            hasPlayed = true;
        }
    }
}
