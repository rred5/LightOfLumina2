using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TarodevController {
    public class PlayerInput : MonoBehaviour {
        public FrameInput FrameInput { get; private set; }
        private bool _actionsEnabled;

        private void Update() => FrameInput = Gather();

#if ENABLE_INPUT_SYSTEM
        private PlayerInputActions _actions;
        private InputAction _move, _jump, _dash, _attack,_exampleAction;

        private void Awake() {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _dash = _actions.Player.Dash;
            _attack = _actions.Player.Attack;
            _exampleAction = _actions.Player.ExampleAction;
            _actionsEnabled = true;
        }

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();


        public void ToggleActions(bool enable)
        {
            _actionsEnabled = enable;
            if (_actionsEnabled)
            {
                   _actions.Enable();
            }
            else {
                _actions.Disable();  
            }
        }
        private FrameInput Gather() {
            return new FrameInput {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),
                AttackDown = _attack.WasPressedThisFrame(),
                Move = _move.ReadValue<Vector2>(),
                ExampleActionHeld = _exampleAction.IsPressed(),
            };
        }

#elif ENABLE_LEGACY_INPUT_MANAGER
        private FrameInput Gather() {
            return new FrameInput {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                DashDown = Input.GetKeyDown(KeyCode.X),
                AttackDown = Input.GetKeyDown(KeyCode.Z),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
                ExampleActionHeld = Input.GetKey(KeyCode.E),
            };
        }
#endif
    }

    public struct FrameInput {
        public Vector2 Move;
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool AttackDown;
        public bool ExampleActionHeld;
    }
}