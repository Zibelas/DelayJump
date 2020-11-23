using UnityEngine;
using UnityEngine.InputSystem;

public class QueuedJump: MonoBehaviour, PlayerInputActions.IPlayerActions
{
   
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float jumpPressedRememberTime = 0.2f;
    [SerializeField] float groundedRememberTime = 0.25f;  
    [SerializeField] [Range(0, 10)] float jumpHeight = 5f;

    private Rigidbody2D _rb;
    private PlayerInputActions _controls; 
    private Vector2 _playerSize;
    private float _jumpPressedRemember = 0;
    private float _groundedRemember = 0;
   
    private void Awake() 
    {
        _controls = new PlayerInputActions();    
        _controls.Player.SetCallbacks(this);
        _controls.Enable();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();    
        _playerSize = (Vector2) GetComponent<SpriteRenderer>().bounds.size + new Vector2(-0.02f, 0);
    }
   
    void PlayerInputActions.IPlayerActions.OnJump(InputAction.CallbackContext context){
        switch (context.phase) {
            case InputActionPhase.Performed:
                _jumpPressedRemember = jumpPressedRememberTime;
                break;
        }
    }

    void Update(){ 
        Vector2 groundedBoxCheckPosition = (Vector2) transform.position + new Vector2(0, -0.01f);
        bool grounded = Physics2D.OverlapBox(groundedBoxCheckPosition, _playerSize, 0, groundLayers);
        _groundedRemember -= Time.deltaTime;
        if (grounded)
        {
            _groundedRemember = groundedRememberTime;
        }
        _jumpPressedRemember -= Time.deltaTime;
        if ((_jumpPressedRemember > 0) && (_groundedRemember > 0))
        {
            _rb.velocity = Vector3.zero;
            _rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            _groundedRemember = 0;
            _jumpPressedRemember = 0;
        }
    }   
}
