using System.Collections.Generic;
using UnityEngine;


namespace Game._Scripts.Player.StateMachine
{
    enum PlayerStatesEnum
    {
        Idle,
        Walk,
        Run,
        Grounded,
        Jump,
        Fall,
        Attack,
        Roll,
        Block
    }
    
    // Not really a factory, more of a pool for now
    public class PlayerStateFactory
    {
        private PlayerStateMachine _context;
        private Dictionary<PlayerStatesEnum, PlayerBaseState> _states = new Dictionary<PlayerStatesEnum, PlayerBaseState>();

        public PlayerStateFactory(PlayerStateMachine currentContext) {
            _context = currentContext;
            _states[PlayerStatesEnum.Idle] = new PlayerIdleState(_context, this);
            _states[PlayerStatesEnum.Walk] = new PlayerWalkState(_context, this);
            _states[PlayerStatesEnum.Run] = new PlayerRunState(_context, this);
            _states[PlayerStatesEnum.Jump] = new PlayerJumpState(_context, this);
            _states[PlayerStatesEnum.Grounded] = new PlayerGroundedState(_context, this);
            _states[PlayerStatesEnum.Fall] = new PlayerFallState(_context, this);
            _states[PlayerStatesEnum.Attack] = new PlayerAttackState(_context, this);
            _states[PlayerStatesEnum.Roll] = new PlayerRollState(_context, this);
            _states[PlayerStatesEnum.Block] = new PlayerBlockState(_context, this);
        }

        public PlayerBaseState Grounded() {
            return _states[PlayerStatesEnum.Grounded];  
        } 
        public PlayerBaseState Jump() {
            return _states[PlayerStatesEnum.Jump];  
        } 
        public PlayerBaseState Fall() {
            return _states[PlayerStatesEnum.Fall];  
        } 
        public PlayerBaseState Idle() {
            return _states[PlayerStatesEnum.Idle];  
        } 
        public PlayerBaseState Walk() {
            return _states[PlayerStatesEnum.Walk];  
        } 
        public PlayerBaseState Run() {
            return _states[PlayerStatesEnum.Run];  
        } 
        public PlayerBaseState Attack() {
            return _states[PlayerStatesEnum.Attack];  
        } 
        public PlayerBaseState Roll() {
            return _states[PlayerStatesEnum.Roll];  
        } 
        public PlayerBaseState Block() {
            return _states[PlayerStatesEnum.Block];  
        } 
    }
}