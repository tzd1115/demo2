using UnityEngine;

public class ColliderIgnore : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), Manager.Instance.player.GetComponent<CharacterController>(), true);
    }

    // Update is called once per frame

}
