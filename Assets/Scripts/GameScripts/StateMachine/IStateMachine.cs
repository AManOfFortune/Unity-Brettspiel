using GameScripts.StateMachine.State;

namespace GameScripts.StateMachine
{
    public interface IStateMachine
    {
        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        IState Current { get; }
        /// <summary>
        /// Initializes the state machine with an initial state.
        /// </summary>
        /// <param name="initialState"></param>
        void InitState(IState initialState);
        /// <summary>
        /// Changes the current state of the state machine.
        /// </summary>
        /// <param name="newState"></param>
        void ChangeState(IState newState);
        /// <summary>
        /// Stops the execution of Unity event functions.
        /// </summary>
        void Lock();
        /// <summary>
        /// Continues the execution of Unity event functions.
        /// </summary>
        void Unlock();
        /// <summary>
        /// Indicates whether the Unity event function execution is stopped or not.
        /// </summary>
        /// <returns></returns>
        bool IsLocked { get; }
    }
}