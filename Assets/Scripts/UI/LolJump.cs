using UnityEngine;

public class LolJump : MonoBehaviour
{
    public float delay;

    private void OnEnable()
    {
        StartJumping(); 
    }

    private void OnDisable()
    {
    }

    public void StartJumping()
    {

        transform.position = new Vector2(0, 0);
        transform.LeanMoveLocal(new Vector2(0, 40), delay).setEaseInOutQuad().setLoopPingPong();
    }
}
