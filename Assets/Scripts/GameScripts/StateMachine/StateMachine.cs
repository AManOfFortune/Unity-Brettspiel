using GameScripts.StateMachine.State;
using UnityEngine;

namespace GameScripts.StateMachine
{
    public class StateMachine : MonoBehaviour, IStateMachine
    {
        public IState Current { get; private set; }

        public bool IsLocked { get; private set; } = false;

        private void Update()
        {
            if (IsLocked) return;
            
            Current.HandleInput();
            Current.OnUpdate();
        }

        private void FixedUpdate()
        {
            if (IsLocked) return;

            Current.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            if (IsLocked) return;

            Current.OnLateUpdate();
        }

        public void InitState(IState initialState)
        {
            Current = initialState;
            initialState.OnEnter();
        }

        public void ChangeState(IState newState)
        {
            Current.OnExit();
            Current = newState;
            Current.OnEnter();
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Unlock()
        {
            IsLocked = false;
        }
    }
}