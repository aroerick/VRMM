// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script handles haptic feedback for Oculus systems

using System;
using UnityEngine;

namespace VRMM {

	public class HapticsController : MonoBehaviour
	{
        private OVRInput.Controller _controllerMask;
        private OVRHapticsClip _clipLight;
		private OVRHapticsClip _clipMedium;
		private OVRHapticsClip _clipStrong;
		private OVRHaptics.OVRHapticsChannel _channel;

		private void Start()
		{
			InitializeOVRHaptics();
		}

		// Create haptic clips
		private void InitializeOVRHaptics()
		{
			const int cnt = 10;
			_clipLight = new OVRHapticsClip(cnt);
			_clipMedium = new OVRHapticsClip(cnt);
			_clipStrong = new OVRHapticsClip(cnt);
			for (var i = 0; i < cnt; i++)
			{
				_clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)75;
				_clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)150;
				_clipStrong.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)255;
			}

			_clipLight = new OVRHapticsClip(_clipLight.Samples, _clipLight.Samples.Length);
			_clipMedium = new OVRHapticsClip(_clipMedium.Samples, _clipMedium.Samples.Length);
			_clipStrong = new OVRHapticsClip(_clipStrong.Samples, _clipStrong.Samples.Length);
		}

		// Send haptic impulse based on intensity and hand options
		public void Vibrate(EHapticIntensity hapticIntensity, EHapticHand hapticHand)
		{
			switch(hapticHand)
			{
					case EHapticHand.LeftController:
						_channel = OVRHaptics.LeftChannel;
						break;
					case EHapticHand.RightController:
						_channel = OVRHaptics.RightChannel;
						break;
                    case EHapticHand.NoHaptics:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(hapticHand), hapticHand, null);
            }

			switch (hapticIntensity)
			{
				case EHapticIntensity.Light:
					_channel.Preempt(_clipLight);
					break;
				case EHapticIntensity.Medium:
					_channel.Preempt(_clipMedium);
					break;
				case EHapticIntensity.Strong:
					_channel.Preempt(_clipStrong);
					break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hapticIntensity), hapticIntensity, null);
            }
		}
	}
}
