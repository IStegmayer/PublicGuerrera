namespace Game._Scripts.Player.StateMachine
{
    public abstract class PlayerBaseState
    {
        private bool _isRootState;
        private PlayerStateMachine _ctx;
        private PlayerStateFactory _factory;
        private PlayerBaseState _currentSuperState;
        private PlayerBaseState _currentSubState;

        protected bool IsRootState {
            set { _isRootState = value; }
        }
        protected PlayerStateMachine Ctx {
            get => _ctx;
        }

        protected PlayerStateFactory Factory {
            get => _factory;
        }
        
        public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) {
            _ctx = currentContext;
            _factory = playerStateFactory;
        }
        
        public abstract void EnterState();
        public abstract void UpdateState();
        // NOTE: this method could be made recursive too
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        public void UpdateStates() {
            UpdateState();
            _currentSubState?.UpdateStates();
        }

        protected void SwitchState(PlayerBaseState newState) {
            // current state exits
            ExitState();
            
            // new state enters
            newState.EnterState();
            
            // switch current state of context
            if (_isRootState) _ctx.CurrentState = newState;
            else if (_currentSuperState != null) _currentSuperState.SetSubState(newState);
        }

        protected void SetSuperState(PlayerBaseState newSuperState) {
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(PlayerBaseState newSubState) {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }
    }
}