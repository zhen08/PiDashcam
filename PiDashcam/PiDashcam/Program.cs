using System;
using System.Threading;

namespace PiDashcam
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var stillCam = new StillCam("image");
			while (true) {
				Thread.Sleep(60000);
			}
		}
	}
}
