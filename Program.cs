using System;

namespace RiskLegacyDice {
	class MainClass {

		public static void Main () {
			Random rand;
			Battle battle;
			string response = "y";

			rand = new Random ();
			battle = new Battle (rand);

			Console.WriteLine ("Risk Legacy Odds Test\n\n");

			do {
				battle.Fight ();

				Console.Write ("Run another battle? (y to continue, anything else to quit)> ");
				response = Console.ReadLine();
				if ((response != null) && (response.Length > 0)) {
					if (response[0] == 'y') {
						Console.WriteLine ("Onward then!\n\n");
					}
				}
			} while (response == "y");
				
			Console.WriteLine ("Have a good day!");



		}
	}
}
