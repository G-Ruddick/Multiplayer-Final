using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public enum PlayerState {Idle, Walking, Jumping, FastFall, Attacking, Stunned, Blocking};
    
    [SerializeField] private float moveSpeed = 50;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float gravityAcceleration = 5;

    private PlayerState playerState = PlayerState.Idle;

    [Header("Components")]
    [SerializeField] private Rigidbody rigidBody;

    private void Start() {
        rigidBody.useGravity = false;
    }

    private void Update() {
        if (rigidBody.linearVelocity.y != 0 && playerState != PlayerState.FastFall) {
            playerState = PlayerState.Jumping;
        }

        rigidBody.AddForce(Vector3.down * gravityAcceleration, ForceMode.Acceleration);

        Invoke(playerState.ToString() + "State", 0);
    }

    private void IdleState() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
            rigidBody.linearVelocity += Vector3.up * jumpSpeed;
            playerState = PlayerState.Jumping;
            return;
        }
        if (Input.GetAxis("Horizontal") != 0) {
            playerState = PlayerState.Walking;
            return;
        }
    }

    private void WalkingState() {
        MovePlayerHorizontally();
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
            rigidBody.linearVelocity += Vector3.up * jumpSpeed;
            playerState = PlayerState.Jumping;
            return;
        }

        if (rigidBody.linearVelocity == Vector3.zero) {
            playerState = PlayerState.Idle;
            return;
        }
    }
    
    private void JumpingState() {
        MovePlayerHorizontally();

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.S)) {
            rigidBody.linearVelocity = Vector3.down * jumpSpeed * 0.75f;
            playerState = PlayerState.FastFall;
            return;
        }

        if (rigidBody.linearVelocity.y == 0) {
            playerState = PlayerState.Idle;
            return;
        }
    }

    private void FastFallState() {
        MovePlayerHorizontally();

        if (rigidBody.linearVelocity.y == 0) {
            playerState = PlayerState.Idle;
            return;
        }
    }

    private void AttackingState() {

    }

    private void StunnedState() {


    }
    private void BlockingState() {

    }

    private void MovePlayerHorizontally() {
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;
        rigidBody.linearVelocity = new Vector3(horizontalMovement, rigidBody.linearVelocity.y, 0);
    }
}
