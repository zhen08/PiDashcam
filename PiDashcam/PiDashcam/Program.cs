using System;
using System.Net.NetworkInformation;
using System.Threading;
using Raspberry.IO.GeneralPurpose;

namespace PiDashcam
{
	class MainClass
	{
		enum DashCamState
		{
			IDLE_NO_NETWORK,
			IDLE_WITH_NETWORK,
			VEHICLE_RUNNING
		}

		private static ProcessorPin VEHICLE_PIN = ConnectorPin.P1Pin29.ToProcessor();
		private static IGpioConnectionDriver driver = GpioConnectionSettings.DefaultDriver;
		private static PiDashcamSettings settings;

		public static void Main(string[] args)
		{

			IRecorder recorder = null;
			DashCamState state = DashCamState.IDLE_NO_NETWORK;

			driver.Allocate(VEHICLE_PIN, PinDirection.Input);

			settings = PiDashcamSettings.Load();
			if (settings == null)
			{
				settings = new PiDashcamSettings();
				settings.Save();
			}

			if (settings.RecordMode == "still")
			{
				recorder = new StillCam("image");
			}
			else
			{
			}

			while (true)
			{
				if (Console.KeyAvailable)
				{
					if (Console.ReadKey().KeyChar.Equals('q'))
					{
						break;
					}
				}
				Thread.Sleep(1000);
				switch (state)
				{
					case DashCamState.IDLE_NO_NETWORK:
						if (IsVehicleRunning())
						{
							state = DashCamState.VEHICLE_RUNNING;
							recorder.Start();
						}
						if (IsConnectedToInternet())
						{
							state = DashCamState.IDLE_WITH_NETWORK;
						}
						break;
					case DashCamState.IDLE_WITH_NETWORK:
						if (IsVehicleRunning())
						{
							state = DashCamState.VEHICLE_RUNNING;
							recorder.Start();
						}
						if (!IsConnectedToInternet())
						{
							state = DashCamState.IDLE_NO_NETWORK;
						}
						UploadData();
						break;
					case DashCamState.VEHICLE_RUNNING:
						if (!IsVehicleRunning())
						{
							state = DashCamState.IDLE_NO_NETWORK;
							recorder.Stop();
						}
						break;
				}
			}
			if (recorder != null)
			{
				recorder.Stop();
			}
		}

		static void UploadData()
		{
			CloudStorage storage = new CloudStorage(settings.StorageConnectionString, settings.StorageContainerName);

		}

		private static bool IsVehicleRunning()
		{
			return driver.Read(VEHICLE_PIN);
		}

		private static bool IsConnectedToInternet()
		{
			string host = "http://www.google.com";
			bool result = false;
			var p = new Ping();
			try
			{
				PingReply reply = p.Send(host, 3000);
				if (reply.Status == IPStatus.Success)
					return true;
			}
			catch { }
			return result;
		}
	}
}
