namespace GameScripts.StateMachine.State
{
    /// <summary>
    /// State class of a state machine.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Is called when the state is entered.
        /// </summary>
        void OnEnter();
        /// <summary>
        /// Gets the user input. Is called every Update cycle.
        /// </summary>
        void HandleInput();
        /// <summary>
        /// Is called every Update cycle.
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// Is called every FixedUpdate cycle.
        /// </summary>
        void OnFixedUpdate();
        /// <summary>
        /// Is called every LateUpdate cycle.
        /// </summary>
        void OnLateUpdate();
        /// <summary>
        /// Is called when the state is exited.
        /// </summary>
        void OnExit();
    }
}