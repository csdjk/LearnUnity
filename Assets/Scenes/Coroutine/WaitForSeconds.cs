using UnityEngine;
/// <summary>
/// 按秒等待
/// </summary>
public class WaitForSeconds : IWait {
    float _seconds = 0f;

    public WaitForSeconds (float seconds) {
        _seconds = seconds;
    }

    public bool Tick () {
        _seconds -= Time.deltaTime;
        return _seconds <= 0;
    }
}


/// <summary>
/// 按帧等待
/// </summary>
public class WaitForFrames:IWait
{
    private int _frames = 0;
    public WaitForFrames(int frames)
    {
        _frames = frames;
    }

    public bool Tick()
    {
        _frames -= 1;
        return _frames <= 0;
    }
}