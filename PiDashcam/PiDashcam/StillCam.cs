using System;
using System.Timers;
using Shell.Execute;

namespace PiDashcam
{
	public class StillCam
	{
		Timer timer;
		int imgcounter;

		public StillCam()
		{
			timer = new Timer(10000);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
			imgcounter = 1;
		}

		public void Stop()
		{
			timer.Stop();
		}

		void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			ProgramLauncher.Execute("raspistill", String.Format("-o {0}.jpg",imgcounter++));
		}
	}
}
