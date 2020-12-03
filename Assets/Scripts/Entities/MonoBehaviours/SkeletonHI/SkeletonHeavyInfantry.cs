using UnityEngine;

namespace Entities.MonoBehaviours.SkeletonHI
{
    public class SkeletonHeavyInfantry : MonoBehaviour, IDamageable, IDamager
    {
        [SerializeField] private float _runSpeed = 10f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private int _startingHealth = 55;
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