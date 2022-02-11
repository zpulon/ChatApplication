using ApiCore.JsonFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiCore.Utils
{
    /// <summary>
    /// HTTP请求
    /// </summary>
    public class HttpClientActuator
    {

        private IHttpClientFactory _httpClientFactory;
        private IHttpContextAccessor _httpAccessor;
        private IJsonHelper _iJsonHelper;
        private ILogger<HttpClientActuator> _logger;

        private string defaultToken = nameof(defaultToken);

        /// <summary>
        /// Http请求实例
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public HttpClientActuator(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpAccessor, ILogger<HttpClientActuator> logger, IJsonHelper jsonHelper)
        {
            _httpClientFactory = httpClientFactory;
            _httpAccessor = httpAccessor;
            _logger = logger;
            _iJsonHelper = jsonHelper;
        }

        public HttpClientActuator() { }
        public async Task<TResponse> Post<TResponse>(string url, object body, NameValueCollection queryString = null)
        where TResponse : class, new()
        {
            return await Execute<TResponse>(url, _iJsonHelper.ToJson(body), queryString, HttpMethod.Post, token: defaultToken);
        }

        public async Task<string> Post(string url, string body, NameValueCollection queryString = null)
        {
            return await Execute<string>(url, body, queryString, HttpMethod.Post, token: defaultToken);
        }

        public async Task<TResponse> Get<TResponse>(string url, NameValueCollection queryString)
        where TResponse : class, new()
        {
            return await Execute<TResponse>(url, "", queryString, HttpMethod.Get, token: defaultToken);
        }

        /// <summary>
        /// GET方法获取
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public async Task<TResponse> GetWithToken<TResponse>(string url, string token = "", NameValueCollection queryString = null)
        where TResponse : class, new()
        {
            return await Execute<TResponse>(url, "", queryString, HttpMethod.Get, token);
        }

        public async Task<string> Post(string url, object body, NameValueCollection queryString = null)
        {
            return await Execute<string>(url, _iJsonHelper.ToJson(body), queryString, HttpMethod.Post, token: defaultToken);
        }

        public async Task<TResult> PostWithToken<TResult>(string url, object body, string token = "")
        {
            return await Execute<TResult>(url, _iJsonHelper.ToJson(body), null, HttpMethod.Post, token);
        }

        public async Task<TResult> PutWithToken<TResult>(string url, object body, string token = "")
        {
            return await Execute<TResult>(url, _iJsonHelper.ToJson(body), null, HttpMethod.Put, token);
        }

        public async Task<TResult> PostWithTokenEx<TResult>(string url, object body, string token = "", NameValueCollection queryString = null)
        {
            return await Execute<TResult>(url, _iJsonHelper.ToJson(body), queryString, HttpMethod.Post, token);
        }

        public async Task<TResult> SubmitForm<TResult>(string url, Dictionary<string, string> formData)
        {
            var nameValue = new NameValueCollection();
            foreach (var item in formData)
            {
                nameValue.Add(item.Key, item.Value);
            }
            return await Execute<TResult>(url, "", nameValue, HttpMethod.Post, token: defaultToken, contentType: "application/x-www-form-urlencoded");
        }

        private async Task<TResult> Execute<TResult>(string url, string body, NameValueCollection args, HttpMethod method, string token, string contentType = "application/json")
        {
            string returnContent = "";
            var client = _httpClientFactory.CreateClient();
            try
            {
                if (args?.Count > 0)
                {
                    url = CreateUrl(url, args);
                }
                if (string.IsNullOrEmpty(token))
                {
                    token = _httpAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                }
                SetRequest(client);
                if (token != defaultToken)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Regex.Replace(token, @"bearer\s*", "", RegexOptions.IgnoreCase).Trim(' '));
                }
               
                var res = new HttpResponseMessage();
                if (method == HttpMethod.Get)
                {
                    res = await client.GetAsync(url);
                }
                else if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    if (contentType == "application/x-www-form-urlencoded")
                    {
                        var formdata = new Dictionary<string, string>();
                        foreach (var item in args.AllKeys)
                        {
                            formdata.Add(item, args[item]);
                        }
                        res = await (method == HttpMethod.Post ? client.PostAsync(url, new FormUrlEncodedContent(formdata)) : client.PutAsync(url, new FormUrlEncodedContent(formdata)));
                    }
                    else
                    {
                        res = HttpMethod.Post == method ? await client.PostAsync(url, new StringContent(body, Encoding.UTF8, contentType)) : await client.PutAsync(url, new StringContent(body, Encoding.UTF8, contentType));
                    }
                }
                else if (method == HttpMethod.Delete)
                {
                    res = await client.DeleteAsync(url);
                }
                returnContent = await res.Content.ReadAsStringAsync();
                if (typeof(TResult).Name == "String")
                {
                    return (TResult)(object)returnContent;
                }
                var reuslt = _iJsonHelper.ToObject<TResult>(returnContent);
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"[{DateTime.Now:yy-MM-dd HH:mm:ss}]网络请求失败，响应StatusCode：{(int)res.StatusCode} 请求url:{url},  请求参数:{body}, 请求头:{ _iJsonHelper.ToJson(client.DefaultRequestHeaders) }, 响应结果:{returnContent}");
                    throw new HttpRequestException("网络请求失败，响应StatusCode：" + (int)res.StatusCode);
                }
                return reuslt;
            }
            catch (Exception e)
            {
                _logger.LogError($"[{DateTime.Now:yy-MM-dd HH:mm:ss}]AuthenticationSDK请求接口出错 请求url:{url}, 请求参数:{body}, 请求头:{ _iJsonHelper.ToJson(client.DefaultRequestHeaders) }, 响应结果:{returnContent}, 错误: {e.ToString()}");
                throw e;
            }
        }
        /// <summary>
        /// 设置请求头（从HTTP上下文中集成头信息）
        /// <para>默认继承以x-或xkj-开始的所有头信息</para>
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/></param>
        private void SetRequest(HttpClient client)
        {
            HttpContext httpContext = null;
            if (_httpAccessor != null)
            {
                httpContext = _httpAccessor.HttpContext;
            }

            if (httpContext == null || httpContext.Request == null || httpContext.Request.Headers == null || client == null)
                return;

           
        }
        /// <summary>
        /// 创建请求地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="qs"></param>
        /// <returns></returns>
        public static string CreateUrl(string url, NameValueCollection qs)
        {
            if (qs != null && qs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                List<string> kl = qs.AllKeys.ToList();
                foreach (string k in kl)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(k).Append("=");
                    if (!String.IsNullOrEmpty(qs[k]))
                    {

                        sb.Append(System.Net.WebUtility.UrlEncode(qs[k]));
                    }
                }
                if (url.Contains("?"))
                {
                    url = url + "&" + sb.ToString();
                }
                else
                {
                    url = url + "?" + sb.ToString();
                }
            }

            return url;

        }
    }
}
