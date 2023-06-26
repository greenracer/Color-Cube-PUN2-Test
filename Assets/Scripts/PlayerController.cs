using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Player Speed:")]
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _turnSpeed = 5f;

    private int _groundLayer;

    private PlayerInput _playerInput;
    private PhotonView _view;
    private Coroutine _moveCoroutine;
    private CharacterController _characterController;
    private Renderer _renderer;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
        _renderer = GetComponent<Renderer>();
        _groundLayer = LayerMask.NameToLayer("Ground");
    }

    public override void OnEnable()
    {
        if (!_view.IsMine) return;
        
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.PlayerMap.Move.performed += Move;
            _playerInput.PlayerMap.ChangeColor.performed += ChangeColor;
        }
        _playerInput.Enable();
    }

    public override void OnDisable()
    {
        if (!_view.IsMine) return;

        _playerInput.PlayerMap.Move.performed -= Move;
        _playerInput.PlayerMap.ChangeColor.performed -= ChangeColor;
        _playerInput.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(_groundLayer) == 0)
        {
            if (_moveCoroutine != null) 
                StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveTowardsTarget(hit.point));
        }
    }

    private IEnumerator MoveTowardsTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            float yOffset = transform.position.y;
            target.y = yOffset;
            Vector3 direction = (target - transform.position).normalized;
            Vector3 movement = direction * _moveSpeed * Time.deltaTime;
            _characterController.Move(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _turnSpeed);

            yield return null;
        }
    }

    private void ChangeColor(InputAction.CallbackContext context)
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        _view.RPC("UpdateColor", RpcTarget.AllBuffered, r, g, b);
    }

    [PunRPC]
    private void UpdateColor(float r, float g, float b)
    {
        _renderer.material.color = new Color(r, g, b);
    }
}
