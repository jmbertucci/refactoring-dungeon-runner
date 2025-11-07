using System;
using System.Collections.Generic;

namespace refactoring_dungeon_runner
{
    public class Player
    {
        public string Name { get; }
        public int Health { get; private set; }
        public int MaxHealth { get; }
        public int Gold { get; private set; }
        public List<string> Inventory { get; } = new List<string>();

        public bool IsDead => Health <= 0;

        public Player(string name, int startingHealth)
        {
            if (startingHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(startingHealth), "Starting health must be positive.");

            Name = name ?? string.Empty;
            MaxHealth = startingHealth;
            Health = startingHealth;
        }

        public void AddGold(int amount)
        {
            if (amount > 0) Gold += amount;
        }

        public void AddItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
                Inventory.Add(item);
        }

        public void Heal(int amount)
        {
            if (amount <= 0) return;
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        public void Damage(int amount)
        {
            if (amount <= 0) return;
            Health -= amount;
            if (Health < 0) Health = 0;
        }
    }
}