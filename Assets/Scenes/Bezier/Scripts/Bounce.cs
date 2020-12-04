namespace BezierUtils
{
	/// <summary>
	/// This class contains a C# port of the easing equations created by Robert Penner (http://robertpenner.com/easing).
	/// </summary>
	public static class Bounce
	{
		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: accelerating from zero velocity.
		/// </summary>
		/// <param name="time">
		/// Current time (in frames or seconds).
		/// </param>
		/// <param name="duration">
		/// Expected easing duration (in frames or seconds).
		/// </param>
		/// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
		/// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
		/// <returns>
		/// The eased value.
		/// </returns>
		public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return 1f - EaseOut(duration - time, duration, -1f, -1f);
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: decelerating from zero velocity.
		/// </summary>
		/// <param name="time">
		/// Current time (in frames or seconds).
		/// </param>
		/// <param name="duration">
		/// Expected easing duration (in frames or seconds).
		/// </param>
		/// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
		/// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
		/// <returns>
		/// The eased value.
		/// </returns>
		public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration) < 0.363636374f)
			{
				return 7.5625f * time * time;
			}
			if (time < 0.727272749f)
			{
				return 7.5625f * (time -= 0.545454562f) * time + 0.75f;
			}
			if (time < 0.909090936f)
			{
				return 7.5625f * (time -= 0.8181818f) * time + 0.9375f;
			}
			return 7.5625f * (time -= 21f / 22f) * time + 63f / 64f;
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="time">
		/// Current time (in frames or seconds).
		/// </param>
		/// <param name="duration">
		/// Expected easing duration (in frames or seconds).
		/// </param>
		/// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
		/// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
		/// <returns>
		/// The eased value.
		/// </returns>
		public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time < duration * 0.5f)
			{
				return EaseIn(time * 2f, duration, -1f, -1f) * 0.5f;
			}
			return EaseOut(time * 2f - duration, duration, -1f, -1f) * 0.5f + 0.5f;
		}
	}
}
