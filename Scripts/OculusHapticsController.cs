// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script handles haptic feedback for Oculus systems

using UnityEngine;

namespace VRMM {

	// Define levels of vibration
	public enum VibrationForce
	{
		Light,
		Medium,
		Hard,
	}

	public class OculusHapticsController : MonoBehaviour
	{
		OVRInput.Controller controllerMask;

		private OVRHapticsClip clipLight;
		private OVRHapticsClip clipMedium;
		private OVRHapticsClip clipHard;
		private OVRHaptics.OVRHapticsChannel channel;

		private void Start()
		{
			InitializeOVRHaptics();
		}

		// Create haptic clips
		private void InitializeOVRHaptics()
		{
			int cnt = 10;
			clipLight = new OVRHapticsClip(cnt);
			clipMedium = new OVRHapticsClip(cnt);
			clipHard = new OVRHapticsClip(cnt);
			for (int i = 0; i < cnt; i++)
			{
				clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)75;
				clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)150;
				clipHard.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)255;
			}

			clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
			clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
			clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
		}

		// Send haptic impulse
		public void Vibrate(string vibrationForce, string hapticsHand)
		{
			switch(hapticsHand)
			{
					case "Left":
						channel = OVRHaptics.LeftChannel;
						break;
					case "Right":
						channel = OVRHaptics.RightChannel;
						break;
					
			}

			switch (vibrationForce)
			{
				case "Light":
					channel.Preempt(clipLight);
					break;
				case "Medium":
					channel.Preempt(clipMedium);
					break;
				case "Hard":
					channel.Preempt(clipHard);
					break;
			}
		}
	}
}
