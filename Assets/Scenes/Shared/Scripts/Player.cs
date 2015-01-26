using UnityEngine;
using System.Collections;

public static class Player {

	public class Score {
		private int score = 0;
		public int Get {
			get {
				return score;
			}
		}

		public void Add (int quantity) {
			score += quantity;
		}

		public void Reset () {
			score = 0;
		}
	}

	public class Health {
		private const int maxHealth = 3;
		private int health = maxHealth;
		public int Get {
			get {
				return health;
			}
		}

		// Loose some life. Return 'true' if the players are still alive.
		public bool Subtract (int quantity) {
			health -= quantity;
			if (health <= 0)
			{
				health = 0;
				return false;
			}

			return true;
		}

		// Increase life.
		public void Add (int quantity) {
			health += quantity;
			if (health > maxHealth)
				health = maxHealth;
		}
	}

	public class Energy {
		private const int maxEnergy = 100;
		private int energy = maxEnergy;
		public int Get {
			get {
				return energy;
			}
		}

		public void Add (int quantity) {
			energy += quantity;
			if (energy > maxEnergy)
				energy = maxEnergy;
		}

		public void Burn (int quantity) {
			energy -= quantity;
			if (energy < 0)
				energy = 0;
		}
	}

	// Scores of player 1 and player 2.
	public static Score score1;
	public static Score score2;

	// Common health of both players.
	public static Health health;

	// Energy of player 1 and player 2.
	public static Energy energy1;
	public static Energy energy2;
}
