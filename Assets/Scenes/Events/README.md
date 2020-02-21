
## Unity高度解耦和 - 事件的监听与广播系统

必须先在 [EventType.cs](./EventType.cs) 定义事件名

```csharp
//注册事件
EventCenter.AddListener<string>(EventType.ShowText,ShowText);
//广播
EventCenter.Broadcast(EventType.ShowText,"123456");
//注销事件
EventCenter.RemoveListener<string>(EventType.ShowText,ShowText);

```