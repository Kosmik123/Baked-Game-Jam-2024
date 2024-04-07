using System;
using System.Collections.Generic;
using System.Linq;
using CatPackage;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.CagePack
{
    public class Cage : MonoBehaviour
    {
        [SerializeField] private Transform catInBox;
        [SerializeField] private GameObject bars;
        [SerializeField] private float radius;

        private bool opened = false;
        private List<SOCat> cats;
        private List<(ECatTier tier, int weight)> weights = new()
        {
            (tier: ECatTier.Common, weight: 50),
            (tier: ECatTier.Uncommon, weight: 25),
            (tier: ECatTier.Rare, weight: 12),
            (tier: ECatTier.Legendary, weight: 6),
        };
        private void Awake()
        {
            cats = Resources.LoadAll<SOCat>("SoCats").ToList();
        }

        private void Update()
        {
            if (opened) return;
            
            var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            if (colliders.Length == 0) return;

            if (colliders.ToList().FirstOrDefault(c => c.CompareTag("Player")) == default) return;
            
            OpenCage();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public void OpenCage()
        {
            opened = true;
            bars.GetComponent<Bars>().ThrowBars();
            var catSpawnPos = catInBox.transform.position;
            Destroy(catInBox.gameObject);
            
            var tier = GetRandomTier();
            var catsByTier = cats.Where(c => c.GetDisplayInfo().catTier == tier).ToList();
            if (catsByTier.Count == 0) return;
            
            var randomIndex = Random.Range(0, catsByTier.Count);
            var cat = catsByTier[randomIndex];
            var added = PlayerManager.Instance.PickUpCat(cat, catSpawnPos);
            if (added) return;
            
            // todo: animacja wyskoku kota z klatki i jego zniknięcie
        }

        private ECatTier GetRandomTier()
        {
            weights.Sort((a, b) => a.weight - b.weight);
            var sum = weights.Sum(w => w.weight);
            var randomNumber = Random.Range(0, sum + 1);
            foreach (var weight in weights)
            {
                var diff = randomNumber - weight.weight;
                if (diff <= 0) return weight.tier;
                randomNumber = diff;
            }

            return 0;
        }
    }
}