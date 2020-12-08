namespace WrappedCommunication {
  using System;
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.Http.Connections.Client;
  using Microsoft.AspNetCore.SignalR.Client;


  public partial class SignalRClientW : IAsyncDisposable {

    public SignalRClientW(HubConnection Connection) {
      _Connection = Connection;
      RegisteEvents(_Connection);
    }
    public SignalRClientW(string Url, Action<HttpConnectionOptions> Configure)
      : this(
      new HubConnectionBuilder()
        .WithUrl(Url, Configure)
        .Build()) { }
    public SignalRClientW(string Url, Action<HttpConnectionOptions> Configure, IRetryPolicy RetryPolicy)
      : this(new HubConnectionBuilder()
        .WithUrl(Url, Configure)
        .WithAutomaticReconnect(RetryPolicy)
        .Build()) { }

    public readonly HubConnection _Connection;

    partial void RegisteEvents(HubConnection _Connection);


    public ValueTask DisposeAsync() {
      return _Connection.DisposeAsync();
    }
  }

  //[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
  //public class SignalRClientMethodAttribute : Attribute {
  //  public readonly string MethodName;
  //  public string TargetMethodName(MethodInfo Method) => MethodName ?? Method.Name;
  //  public bool KeepRegistedOnce { get; set; } = false;
  //  public MethodInfo Method { get; set; }
  //  public SignalRClientMethodAttribute() : this(null) { }
  //  public SignalRClientMethodAttribute(string MethodName) {
  //    this.MethodName = MethodName;
  //  }
  //}

  public abstract class SignalRClientWException : Exception {
    public SignalRClientWException() : base() { }
    public SignalRClientWException(string? message) : base(message) { }
    public SignalRClientWException(string? message, Exception? innerException) : base(message, innerException) { }
  }

  //public sealed class InvalidClientMethodNamePairException : SignalRClientWException {
  //  public readonly string[] InvalidMethodKey;
  //  public InvalidClientMethodNamePairException(string[] Keys) : base() { InvalidMethodKey = Keys; }
  //}

}
