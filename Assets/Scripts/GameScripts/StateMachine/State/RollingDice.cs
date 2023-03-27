using UnityEngine;

namespace GameScripts.StateMachine.State
{
    public class RollingDice : MonoBehaviour, IState
    {
        public void OnEnter()
        {

        }

        public void HandleInput()
        {
        }

        public void OnUpdate()
        {
            if (GameManager.Instance.GetActivePlayer().Type == Player.PlayerTypes.NPC)
            {
                GameManager.Instance.RollDice(2); // Roll dice with a 2 second delay
            }
            else if (GameManager.Instance.GetActivePlayer().Type == Player.PlayerTypes.HUMAN)
            {
                UIManager.Instance.RollDiceButtonVisible(true); // Show Roll Dice Button
            }
            
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