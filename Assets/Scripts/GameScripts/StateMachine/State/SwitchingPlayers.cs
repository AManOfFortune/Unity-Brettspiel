using UnityEngine;

namespace GameScripts.StateMachine.State
{
    public class SwitchingPlayers : MonoBehaviour, IState
    {
        public void OnEnter()
        {

        }

        public void HandleInput()
        {
        }

        public void OnUpdate()
        {
            GameManager.Instance.SwitchPlayer(2);
            
            GameManager.StateMachine.ChangeState(GameManager.Waiting);
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLateUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}