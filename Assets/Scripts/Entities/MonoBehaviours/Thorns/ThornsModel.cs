using UnityEngine;

namespace Entities.MonoBehaviours.Thorns
{
    public class ThornsModel : MonoBehaviour, IDamager
    {
        [SerializeField] private int _damage = 25;
        public int Damage { get => _damage; }
    }
}