using UnityEngine;

public class FloorColliderTrigger : MonoBehaviour
{
    public LokacjaMisterManager manager;
    public int floorNumber;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            manager.PlayerEnteredFloor(floorNumber);
        }
    }
}
