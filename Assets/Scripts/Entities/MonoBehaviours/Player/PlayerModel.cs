using UnityEngine;

namespace Entities.MonoBehaviours.Player
{
    public class PlayerModel : MonoBehaviour, IDamageable, IDamager
    {
        [SerializeField] private float _runSpeed = 40f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private int _startingHealth = 100;
        [SerializeField] private int _currentHealth;
        public float RunSpeed => _runSpeed;

        public int Damage => _damage;

        public int StartingHealth => _startingHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }
    }
}