using Entities.MonoBehaviours;
using UnityEngine;

namespace Environments.ShootingTrap
{
    public class ArrowModel : MonoBehaviour, IDamager
    {
        [SerializeField] private int _damage;
        public int Damage { get => _damage; }
    }
}
