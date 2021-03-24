using UnityEngine;

public class Target : MonoBehaviour
{
    public Agent tracker;
    private void Update()
    {
        tracker.SetDestination(transform.position);
    }
}