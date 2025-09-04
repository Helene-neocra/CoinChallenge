using UnityEngine;

public class Turtle : EnnemiComportement
{
    private void OnCollisionEnter(Collision other)
    {
        KillPlayer(other.collider);
        Destroy(gameObject);
    }
}
