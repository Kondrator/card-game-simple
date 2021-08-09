using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


public static class MyOperationDevice{

	
	[DllImport("kernel32")]
	extern static ulong GetTickCount64();


	/// <summary>
	/// Up time from stratup OS
	/// </summary>
	/// <returns>In seconds</returns>
	public static ulong GetUpTime(){
		//return DeviceUptime.GetSystemUptime() / 1000;

		return GetTickCount64() / 1000;

		/*
		TimeSpan upTime;
		using( var pc = new PerformanceCounter( "System", "System Up Time" ) ) {
			pc.NextValue(); // The first call returns 0, so call this twice
			upTime = TimeSpan.FromSeconds( pc.NextValue() );
		}
		return upTime;
		*/

		//return (ulong)TimeSpan.FromMilliseconds( Environment.TickCount ).TotalSeconds;
	}

	/// <summary>
	/// Get unique identifier of device
	/// </summary>
	public static string GetDeviceID(){
		return SystemInfo.deviceUniqueIdentifier;
	}


}
