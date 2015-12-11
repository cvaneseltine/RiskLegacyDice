using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RiskLegacyDice {
	public class Battle {
		Random rand;
		List <Dice> attackDice = new List<Dice> ();
		List <Dice> defendDice = new List<Dice> ();

		public Battle (Random randIn) {
			rand = randIn;
		}

		public void Fight () {
			string response;
			int attackCount = 0;
			int defendCount = 0;
			int troopCount = 0;
			int attackTroop, defendTroop;
			List <Dice> attackDice = new List<Dice> ();
			List <Dice> defendDice = new List<Dice> ();
			bool showBattles = true;
			bool isBunker = false;
			bool isAmmoShortage = false;

			do {
				bool parsed;

				Console.Write ("How many black dice is the attacker using?> ");
				response = Console.ReadLine ();
				if (response == null) {
					Console.WriteLine ("Couldn't understand that. Try a number, like 2.");
				}
				else {
					parsed = Int32.TryParse (response, out attackCount);
					if (!parsed) {
						Console.WriteLine ("Couldn't understand that. Try a number, like 2.");
					}
					else {
						if (attackCount <= 0) {
							Console.WriteLine ("You can't attack with " + attackCount + " dice!");
						}
						else if (attackCount > 3) {
							Console.WriteLine ("That attacker is cheating, but okay.");
						}
					}
				}
			} while (attackCount <= 0);
			do {
				bool parsed;

				Console.Write ("How many red dice is the defender using?> ");
				response = Console.ReadLine ();
				if (response == null) {
					Console.WriteLine ("Couldn't understand that. Try a number, like 2.");
				}
				else {
					parsed = Int32.TryParse (response, out defendCount);
					if (!parsed) {
						Console.WriteLine ("Couldn't understand that. Try a number, like 2.");
					}
					else {
						if (defendCount <= 0) {
							Console.WriteLine ("You can't defend with " + defendCount + " dice!");
						}
						else if (defendCount > 2) {
							Console.WriteLine ("That defender is cheating, but okay.");
						}
					}
				}
			} while (defendCount <= 0);

			do {
				bool parsed;

				Console.Write ("How many troops are they starting with (apiece)?> ");
				response = Console.ReadLine ();
				if (response == null) {
					Console.WriteLine ("Couldn't understand that. Try a number, like 2.");
				}
				else {
					parsed = Int32.TryParse (response, out troopCount);
					if (!parsed) {
						Console.WriteLine ("Couldn't understand that. Try a number, like 2.");
					}
					else {
						if (troopCount <= 0) {
							Console.WriteLine ("They have " + troopCount + " troops? C'mon, be sensible.");
						}
					}
				}
			} while (troopCount <= 0);

			Console.Write ("Is there a bunker? (y for yes)> ");
			response = Console.ReadLine();
			response = response.ToLower();
			if ((response != null) && ((response.Length > 0) && (response[0] == 'y'))) {
				isBunker = true;
				Console.WriteLine ("Bunker, got it.\n");
			}
			else {
				Console.Write ("Is there an ammo shortage? (y for yes)> ");
				response = Console.ReadLine();
				response = response.ToLower();
				if ((response != null) && ((response.Length > 0) && (response[0] == 'y'))) {
					isAmmoShortage = true;
					Console.WriteLine ("Ammo shortage, got it.\n");
				}
				else {
					Console.WriteLine ("");
				}
			}

			if (troopCount > 10) {
				Console.WriteLine ("\nYou're starting with " + troopCount + " troops, so the rolls won't be shown. But trust me, they're there.");
				showBattles = false;
			}

			for (int i = 0; i < attackCount; i++) { //Create the attack dice.
				attackDice.Add (new Dice (rand));
			}

			for (int i = 0; i < defendCount; i++) { //Create the defense dice.
				defendDice.Add (new Dice (rand));
			}

			attackTroop = troopCount;
			defendTroop = troopCount;

			while ((attackTroop > 0) && (defendTroop > 0)) { //Roll the dice and tally the results.
				string report = "";
				int attackerLost = 0;
				int defenderLost = 0;
				int diceToCompare;

				foreach (Dice thisDie in attackDice) {
					thisDie.Roll ();
				}

				report = ("\nAttack dice rolled: ");
				attackDice = attackDice.OrderBy (x=>x.LastRoll).ToList ();
				attackDice.Reverse ();

				foreach (Dice thisDie in attackDice) {
					report = (report + thisDie.LastRoll + " ");
				}

				foreach (Dice thisDie in defendDice) {
					thisDie.Roll ();
				}

				report = (report + "\nDefend dice rolled: ");
				defendDice = defendDice.OrderBy (x=>x.LastRoll).ToList ();
				defendDice.Reverse ();

				foreach (Dice thisDie in defendDice) {
					report = (report + thisDie.LastRoll + " ");
				}

				if (isBunker) {
					if (defendDice[0].LastRoll < 6) {
						defendDice[0].LastRoll++;
						report = (report + "\nFirst die boosted to " + defendDice[0].LastRoll + " by bunker.");
					}
					else {
						report = (report + "\nBunker cannot boost a 6 any higher.");
					}
				}

				if (isAmmoShortage) {
					if (defendDice[0].LastRoll > 1) {
						defendDice[0].LastRoll--;
						report = (report + "\nFirst die reduced to " + defendDice[0].LastRoll + " by ammo shortage.");
					}
					else {
						report = (report + "\nAmmo shortage cannot drop a 1 any lower.");
					}
				}

				diceToCompare = attackDice.Count;
				if (diceToCompare > defendDice.Count) {
					diceToCompare = defendDice.Count;
				}

				for (int j = 0; j < diceToCompare; j++) {
					if ((attackTroop > 0) && (defendTroop > 0)) {
						report = (report + "\nAttacker " + attackDice[j].LastRoll + " vs defender " + defendDice[j].LastRoll + ".");
						if (attackDice[j].DidAttackerWin (defendDice[j])) {
							defenderLost++;
							defendTroop--;
							report = (report + " Defender loses.");
						}
						else {
							attackerLost++;
							attackTroop--;
							report = (report + " Attacker loses.");
						}
					}
				}

				report = (report + "\n* Attacker loses " + attackerLost + ", defender loses " + defenderLost + ". *");

				if (showBattles) {
					Console.WriteLine (report);
				}
			}

			string finalReport = "";

			finalReport = (finalReport + "\n\nAttacking troops remaining: ");
			if (attackTroop == 0) {
				finalReport = (finalReport + "none (" + troopCount + " destroyed).");
			}
			else {
				int destroyed = troopCount - attackTroop;
				finalReport = (finalReport + attackTroop + " (" + destroyed + " destroyed).");
			}
			finalReport = (finalReport + "\nDefending troops remaining: ");
			if (defendTroop == 0) {
				finalReport = (finalReport + "none (" + troopCount + " destroyed).");
			}
			else {
				int destroyed = troopCount - defendTroop;
				finalReport = (finalReport + defendTroop + " (" + destroyed + " destroyed).");
			}

			Console.WriteLine (finalReport);
		}
	}
}

