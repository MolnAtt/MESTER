using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESTER_Tehenészet
{
	class Program
	{

		static List<int> Tehenészetek;
		static List<int> Kapacitások;
		static int[,] Árak;
		static int akt_költség;
		static int mo_költség;
		static Stack<int> akt_választások = new Stack<int>();
		static List<int> mo_választások;
		static int N, M;

		static bool ElsőMegoldásKeresése(int i)
		{
			bool vannegatívkapacitás = Kapacitások.FindIndex(x => x < 0) != -1;
			if (i == N && !vannegatívkapacitás) // siker
			{
				mo_költség = akt_költség;
				mo_választások = new List<int>(akt_választások);
				return true;
			}

			if (vannegatívkapacitás || i == N) // levél
				return false;

			bool megvan = false;
			for (int j = 0; j < M && !megvan; j++)
			{
				akt_választások.Push(j);
				Kapacitások[j] -= Tehenészetek[i];
				akt_költség += Tehenészetek[i] * Árak[i, j];
				megvan = ElsőMegoldásKeresése(i + 1);
				akt_költség -= Tehenészetek[i] * Árak[i, j];
				Kapacitások[j] += Tehenészetek[i];
				akt_választások.Pop();
			}

			return megvan;
		}

		static bool defined = false;

		static void LegjobbMegoldásKeresése(int i)
		{
			bool vannegatívkapacitás = Kapacitások.FindIndex(x => x < 0) != -1;
			bool hopeless = defined && (akt_költség >= mo_költség);
			if (i == N && !vannegatívkapacitás) // siker
			{
				if (!defined || !hopeless)
				{
					mo_költség = akt_költség;
					mo_választások = new List<int>(akt_választások);
					defined = true;
					/*Console.WriteLine($"mo_költség = {mo_költség}");
					Console.WriteLine($"mo_választások:");
					foreach (var item in mo_választások)
					{
						Console.Write((item+1) + " ");
					}
					Console.WriteLine();
					Console.WriteLine("Kapacitások:");
					foreach (var item in Kapacitások)
					{
						Console.Write(item+" ");
					}
					Console.WriteLine();*/
				}
				return;
			}

			if (vannegatívkapacitás || i == N|| hopeless) // levél
				return;

			for (int j = 0; j < M; j++)
			{
				akt_választások.Push(j);
				Kapacitások[j] -= Tehenészetek[i];
				akt_költség += Tehenészetek[i] * Árak[i, j];
				LegjobbMegoldásKeresése(i + 1);
				akt_költség -= Tehenészetek[i] * Árak[i, j];
				Kapacitások[j] += Tehenészetek[i];
				akt_választások.Pop();
			}
		}

		static void Main(string[] args)
		{
			#region Beolvasás
			string[] sor = Console.ReadLine().Split(' ');
			(N,M) = (int.Parse(sor[0]), int.Parse(sor[1]));

			Tehenészetek = new List<int>();
			sor = Console.ReadLine().Split(' ');
			for (int i = 0; i < N; i++)
			{
				Tehenészetek.Add(int.Parse(sor[i]));
			}

			Kapacitások = new List<int>();
			sor = Console.ReadLine().Split(' ');
			for (int i = 0; i < M; i++)
			{
				Kapacitások.Add(int.Parse(sor[i]));
			}

			Árak = new int[N, M]; // i. tehenészetből a j. tejüzembe való szállítás költsége
			for (int i = 0; i < N; i++)
			{
				sor = Console.ReadLine().Split(' ');
				for (int j = 0; j < M; j++)
				{
					Árak[i, j] = int.Parse(sor[j]);
				}
			}
			#endregion

			LegjobbMegoldásKeresése(0);
			mo_választások.Reverse();
			if (defined)
			{
				string str = "";
				foreach (var item in mo_választások)
				{
					str += (item+1) + " ";
				}
				Console.WriteLine(mo_költség);
				Console.WriteLine(str);
			}
			else Console.WriteLine();
		}
	}
}
