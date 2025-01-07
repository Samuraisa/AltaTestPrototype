using System;
using System.Collections.Generic;

namespace Alta
{
    public class StateMachine
    {
        private StateNode _currentState;
        private Dictionary<Type, StateNode> _stateNodes = new();
        private HashSet<ITransition> _anyTransitions = new();


        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.ToState.GetType());

            _currentState?.State?.Update();
        }
        
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(new Transition(GetOrAddNode(to).State, condition));
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }
        
        public void AddState(IState state)
        {
            GetOrAddNode(state);
        }

        public void SetState<T>() where T : IState
        {
            var stateType = typeof(T);
            SetState(stateType);
        }

        private void SetState(Type stateType)
        {
            if (_currentState?.State != null && stateType == _currentState.State.GetType())
                return;

#if UNITY_EDITOR && DEBUG_FSM
            if (_currentState?.State != null)
                UnityEngine.Debug.Log($"FSM State Exit: {_currentState.State.GetType().Name}");
#endif
            _currentState?.State?.OnExit();
            _currentState = _stateNodes[stateType];      
#if UNITY_EDITOR && DEBUG_FSM
            if (_currentState?.State != null)
                UnityEngine.Debug.Log($"FSM State Enter: {_currentState.State.GetType().Name}");
#endif
            _currentState.State?.OnEnter();
        }

        private StateNode GetOrAddNode(IState state)
        {
            if (!_stateNodes.TryGetValue(state.GetType(), out var node))
            {
                node = new StateNode(state);
                _stateNodes.Add(state.GetType(), node);
            }

            return node;
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;
            
            if (_currentState == null)
                return null;

            foreach (var transition in _currentState.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;
            
            return null;
        }


        private class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }
            
            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }
            
            public void AddTransition(ITransition transition)
            {
                Transitions.Add(transition);
            }
        }
    }
}