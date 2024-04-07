using System;
using System.Linq;
using Managers;
using UnityEngine;

namespace CatPackage
{
    public class CatMember : MonoBehaviour
    {
        [SerializeField] private Transform shootPos;

        private ActiveCatData _activeCat;

        private float _timer = 0;
        private KeyCode _attackKey;

        public void SetCat(ActiveCatData activeCatData, KeyCode attackKey)
        {
            _attackKey = attackKey;
            _activeCat = activeCatData;
            gameObject.SetActive(true);
            GetComponentInParent<SpriteRenderer>().color =
                PlayerManager.Instance.KeyColors.FirstOrDefault(c => c.key == _attackKey).color;
        }

        public ActiveCatData GetCat()
        {
            return _activeCat;
        }

        public void LevelUp()
        {
            _activeCat.level++;
        }

        public void ChangeHp(int diff, bool add = false)
        {
            var val = diff * (add ? 1 : -1);
            _activeCat.health += val;
        }
        
        private void Update()
        {
            if (_activeCat == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _timer += Time.deltaTime;
            if (!Input.GetKey(_attackKey) || _timer < _activeCat.cat.GetSpecificInfo(_activeCat.level).cooldown) return;

            _timer = 0;
            _activeCat.cat.SpawnAttackPrefab(shootPos.position, transform, _activeCat.level);
        }
    }
}