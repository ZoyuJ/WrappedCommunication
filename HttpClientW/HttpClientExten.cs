namespace HttpClient {
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Net.Http;
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.WebUtilities;
  using HttpFormParas = System.Collections.Generic.Dictionary<System.String, System.Object>;
  using HttpHeaderParas = System.Collections.Generic.Dictionary<System.Net.HttpRequestHeader, System.String>;
  using HttpQueryParas = System.Collections.Generic.Dictionary<System.String, System.String>;
  using HttpResponseHandle = System.Action<System.Net.Http.HttpResponseMessage>;
  using HttpRequestErrorHandle = System.Action<System.Exception>;
  public partial class HttpClientExten {
    public HttpClientExten() { }
    public HttpClient AvailableHttpClient(int Timeout = 30) {
      var Client = new HttpClient();
      Client.Timeout = TimeSpan.FromSeconds(Timeout);
      return Client;
    }

    #region FormKits
    public static MultipartFormDataContent AssembleHttpForm_Multipart_form_data(HttpFormParas FormData) {
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
    public static string AssembleHttpQueryString(string Uri, HttpQueryParas QueryData) {
      return QueryHelpers.AddQueryString(Uri, QueryData);
    }
    public static HttpClient AssembleHttpHeader(HttpClient Client, HttpHeaderParas HeaderData) {
      if (HeaderData != null && HeaderData.Count > 0) {
        foreach (var item in HeaderData) {
          Client.DefaultRequestHeaders.Add(Enum.GetName(typeof(System.Net.HttpRequestHeader), item.Key), item.Value);
        }
      }
      return Client;
    }

    public static HttpRequestMessage DefaultMessage(HttpMethod Method) { return new HttpRequestMessage() { Method = Method }; }
    public static HttpRequestMessage AddUriAndQuery(HttpRequestMessage Message, string Uri, HttpQueryParas Query = null) {
      Message.RequestUri = new Uri(Query == null ? Uri : AssembleHttpQueryString(Uri, Query));
      return Message;
    }
    public static HttpRequestMessage AddForm(HttpRequestMessage Message, HttpFormParas Form = null) {
      if (Form != null) {
        Message.Content = AssembleHttpForm_Multipart_form_data(Form);
      }
      return Message;
    }
    public static HttpRequestMessage AddHeaders(HttpRequestMessage Message, HttpHeaderParas Header = null) {
      if (Header != null) {
        foreach (var item in Header) {
          Message.Headers.Add(Enum.GetName(typeof(System.Net.HttpRequestHeader), item.Key), item.Value);
        }
      }
      return Message;
    }
    #endregion
    #region Exten
    public async Task HttpPostAsync(string Url, HttpQueryParas Query = null, HttpFormParas Form = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await AvailableHttpClient().PostAsync(
          Query == null ? Url : AssembleHttpQueryString(Url, Query),
          Form == null ? null : AssembleHttpForm_Multipart_form_data(Form));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpPostAsyncTask(string Url, HttpQueryParas Query = null, HttpFormParas Form = null) {
      return await AvailableHttpClient().PostAsync(
         Query == null ? Url : AssembleHttpQueryString(Url, Query),
         Form == null ? null : AssembleHttpForm_Multipart_form_data(Form));
    }

    public async Task HttpGetAsync(string Url, HttpQueryParas Query = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await AvailableHttpClient().GetAsync(Query == null ? Url : AssembleHttpQueryString(Url, Query));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpGetAsyncTask(string Url, HttpQueryParas Query = null) {
      return await AvailableHttpClient().GetAsync(Query == null ? Url : AssembleHttpQueryString(Url, Query));
    }

    public async Task HttpPutAsync(string Url, HttpQueryParas Query = null, HttpFormParas Form = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await AvailableHttpClient().PutAsync(
        Query == null ? Url : AssembleHttpQueryString(Url, Query),
        Form == null ? null : AssembleHttpForm_Multipart_form_data(Form));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpPutAsyncTask(string Url, HttpQueryParas Query = null, HttpFormParas Form = null) {
      return await AvailableHttpClient().PutAsync(
              Query == null ? Url : AssembleHttpQueryString(Url, Query),
              Form == null ? null : AssembleHttpForm_Multipart_form_data(Form));
    }

    public async Task HttpDeleteAsync(string Url, HttpQueryParas Query = null, HttpResponseHandle Response = null, HttpRequestErrorHandle Catched = null) {
      try {
        var ResponseMessage = await AvailableHttpClient().DeleteAsync(Query == null ? Url : AssembleHttpQueryString(Url, Query));
        Response?.Invoke(ResponseMessage);
      }
      catch (Exception e) {
        Catched?.Invoke(e);
      }
    }
    public async Task<HttpResponseMessage> HttpDeleteAsyncTask(string Url, HttpQueryParas Query = null) {
      return await AvailableHttpClient().DeleteAsync(Query == null ? Url : AssembleHttpQueryString(Url, Query));
    }
    #endregion


  }
}
