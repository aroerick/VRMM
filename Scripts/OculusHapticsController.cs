// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script handles haptic feedback for Oculus systems

using UnityEngine;

namespace VRMM {

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
		public void Vibrate(e_hapticIntensity vibrationForce, e_hapticHand hapticsHand)
		{
			switch(hapticsHand)
			{
					case e_hapticHand.LeftController:
						channel = OVRHaptics.LeftChannel;
						break;
					case e_hapticHand.RightController:
						channel = OVRHaptics.RightChannel;
						break;
					
			}

			switch (vibrationForce)
			{
				case e_hapticIntensity.Light:
					channel.Preempt(clipLight);
					break;
				case e_hapticIntensity.Medium:
					channel.Preempt(clipMedium);
					break;
				case e_hapticIntensity.Strong:
					channel.Preempt(clipHard);
					break;
			}
		}
	}
}
