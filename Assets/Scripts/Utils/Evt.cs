using System;
using System.Collections.Generic;

namespace Alta
{
    public class Evt
    {
        private readonly HashSet<Action> _handlers = new();
        
        public void AddListener(Action handler) => _handlers.Add(handler);
        
        public void RemoveListener(Action handler) => _handlers.Remove(handler);

        public void Invoke()
        {
            foreach (var handler in _handlers)
                handler.Invoke();
        }
    }
    
    public class Evt<T>
    {
        private readonly HashSet<Action<T>> _handlers = new();
        
        public void AddHandler(Action<T> handler) => _handlers.Add(handler);
        
        public void RemoveHandler(Action<T> handler) => _handlers.Remove(handler);

        public void Invoke(T parameter)
        {
            foreach (var handler in _handlers)
                handler.Invoke(parameter);
        }
    }
}