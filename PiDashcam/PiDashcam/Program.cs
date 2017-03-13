using System;

namespace PiDashcam
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("PiDashcam started");
			var stillCam = new StillCam();
			while (Console.ReadKey().KeyChar != 'q')
			{
			}
			stillCam.Stop();
		}
	}
}
