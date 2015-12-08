using System;
using System.Collections.Generic;

namespace RiskLegacyDice {
	
	public class Dice {

		Random rand;
		int lastRoll;

		public int LastRoll {
			get { return lastRoll;}
			set { lastRoll = value;}
		}
		
		public Dice (Random inputRand) {
			rand = inputRand;
		}

		public int Roll () {
			lastRoll = rand.Next (1, 7);
			return lastRoll;
		}

		public bool DidAttackerWin (Dice defender) {
			if (LastRoll > defender.LastRoll) {
				return true;
			}
			return false;
		}
	}
}

