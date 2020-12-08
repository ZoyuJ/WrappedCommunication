namespace WrappedCommunication {
  using System;
  using System.Collections;
  using System.IO;
  using System.Net.Http;
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.WebUtilities;

  using HttpFormParas = System.Collections.Generic.Dictionary<System.String, System.Object>;
  using HttpHeaderParas = System.Collections.Generic.Dictionary<System.Net.HttpRequestHeader, System.String>;
  using HttpQueryParas = System.Collections.Generic.Dictionary<System.String, System.String>;
  using HttpRequestErrorHandle = System.Action<System.Exception>;
  using HttpResponseHandle = System.Action<System.Net.Http.HttpResponseMessage>;
  public partial class HttpClientW {
    public HttpClientW() : this(new HttpClient()) { }
    public HttpClientW(HttpClient Client) {
      HttpClient = Client;
    }

    public readonly HttpClient HttpClient;

    #region FormKits
    public static MultipartFormDataContent GeneratHttpForm_Multipart_form_data(HttpFormParas FormData) {
      var Content = new MultipartFormDataContent();
      foreach (var FormPa in FormData) {
        if (FormPa.Value == null) continue;
        __AsmFormData(ref Content, FormPa.Value, FormPa.Key);

      }
      return Content;

      void __AsmFormData(ref MultipartFormDataContent Form, object Value, string Key) {
        if (Value == null) return;
        switch (Value) {
          case string s:
            Content.Add(new StringContent(s), $"\"{Key}\"");
            break;
          case int s:
            Content.Add(new StringContent(s.ToString()), $"\"{Key}\"");
            break;
          case MemoryStream s:
            Content.Add(new ByteArrayContent(s.ToArray()), $"\"{Key}\"", $"{Key}");
            break;
          case Stream s:
            Content.Add(new StreamContent(s), $"\"{Key}\"", $"{Key}");
            break;
          case byte[] s:
            Content.Add(new ByteArrayContent(s), $"\"{Key}\"", $"{Key}");
            break;
          case IEnumerable s:
            foreach (var item in s) {
              __AsmFormData(ref Form, item, Key);
            }
            break;
          default:
            break;
        }
      }

    }
    public static string GeneratHttpQueryString(string Uri, HttpQueryParas QueryData) {
      return QueryHelpers.AddQueryString(Uri, QueryData);
    }
    public void SetHttpHeader(HttpClient Client, HttpHeaderParas HeaderData) {
      if (HeaderData != null && HeaderData.Count > 0) {
        foreach (var item in HeaderData) {
          Client.DefaultRequestHeaders.Add(Enum.GetName(typeof(System.Net.HttpRequestHeader), item.Key), item.Value);
        }
      }
    }

    public static HttpRequestMessage CreateDefaultMessage(HttpMethod Method) { return new HttpRequestMessage() { Method = Method }; }
    public static HttpRequestMessage SetUriAndQuery(HttpRequestMessage Message, string Uri, HttpQueryParas Query = null) {
      Message.RequestUri = new Uri(Query == null ? Uri : GeneratHttpQueryString(Uri, Query));
      return Message;
    }
    public static HttpRequestMessage SetRequestForm(HttpRequestMessage Message, HttpFormParas Form = null) {
      if (Form != null) {
        Message.Content = GeneratHttpForm_Multipart_form_data(Form);
      }
      return Message;
    }
    public static HttpRequestMessage SetRequestHeaders(HttpRequestMessage Message, HttpHeaderParas Header = null) {
      if (Header != null) {
        foreach (var item in Header) {
          Message.Headers.Add(Enum.GetName(typeof(System.Net.HttpRequestHeader), item.Key), item.Value);
        }
      }
      return Message;
    }

    #endregion
    #region Exten
    public async Task HttpPostHandled(string Url, HttpQueryParas Query = null, HttpFormParas Form = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await HttpClient.PostAsync(
          Query == null ? Url : GeneratHttpQueryString(Url, Query),
          Form == null ? null : GeneratHttpForm_Multipart_form_data(Form));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpPostAsync(string Url, HttpQueryParas Query = null, HttpFormParas Form = null) {
      return await HttpClient.PostAsync(
         Query == null ? Url : GeneratHttpQueryString(Url, Query),
         Form == null ? null : GeneratHttpForm_Multipart_form_data(Form));
    }

    public async Task HttpGetHandled(string Url, HttpQueryParas Query = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await HttpClient.GetAsync(Query == null ? Url : GeneratHttpQueryString(Url, Query));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpGetAsync(string Url, HttpQueryParas Query = null) {
      return await HttpClient.GetAsync(Query == null ? Url : GeneratHttpQueryString(Url, Query));
    }

    public async Task HttpPutHandled(string Url, HttpQueryParas Query = null, HttpFormParas Form = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await HttpClient.PutAsync(
        Query == null ? Url : GeneratHttpQueryString(Url, Query),
        Form == null ? null : GeneratHttpForm_Multipart_form_data(Form));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpPutAsync(string Url, HttpQueryParas Query = null, HttpFormParas Form = null) {
      return await HttpClient.PutAsync(
              Query == null ? Url : GeneratHttpQueryString(Url, Query),
              Form == null ? null : GeneratHttpForm_Multipart_form_data(Form));
    }

    public async Task HttpDeleteHandled(string Url, HttpQueryParas Query = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await HttpClient.DeleteAsync(Query == null ? Url : GeneratHttpQueryString(Url, Query));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpDeleteAsync(string Url, HttpQueryParas Query = null) {
      return await HttpClient.DeleteAsync(Query == null ? Url : GeneratHttpQueryString(Url, Query));
    }
    #endregion


  }
}
