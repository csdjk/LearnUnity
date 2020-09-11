/// <summary>
/// 等待接口
/// </summary>
public interface IWait
{
    /// <summary>
    /// 每帧检测是否等待结束
    /// </summary>
    /// <returns></returns>
    bool Tick();
}