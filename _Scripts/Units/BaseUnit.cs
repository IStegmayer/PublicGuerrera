using System;
using UnityEngine;

    public class BaseUnit : MonoBehaviour
    {
        protected int _id;
        //TODO: is this the best way?
        public void SetId(int id) => _id = id;
        public Stats Stats { get; private set; }
        public virtual void SetStats(Stats stats) => Stats = stats;

        public virtual void TakeDamage(int dmg) { }
        //TODO: die?
    }