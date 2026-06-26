using System;
using UnityEngine;

namespace Entity
{
    [Serializable]
    public class EntityHealthModule : IEntityHealthModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _maxHealth = 100;
        private float _health;

        public event Action Decreased;
        public event Action Increased;
        public event Action IsOver;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public float MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                if (value <= 0)
                {
                    Debug.LogWarning("Max health cannot be less than zero");
                    return;
                }
                _maxHealth = value;
                _health = Math.Min(_maxHealth, _health);
            }
        }
        public float Health => _health;

        public void Initialize()
        {
            _health = MaxHealth;
        }

        public void Decrease(float value)
        {
            if (value <= 0 || _health == 0)
                return;

            _health = Math.Max(_health - value, 0);
            Decreased?.Invoke();

            if (_health <= 0)
                IsOver?.Invoke();
        }

        public void Increase(float value)
        {
            if (value <= 0 || _health == MaxHealth)
                return;

            _health = Math.Min(_health + value, MaxHealth);
            Increased?.Invoke();
        }
    }
}
