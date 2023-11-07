using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController characterController;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            characterController.Move(Vector3.forward);
    }
}
