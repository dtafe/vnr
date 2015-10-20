using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HRM.Infrastructure.Utilities
{
    public class RestServiceClient<T> where T : class
    {
        #region Init
        private CookieContainer _cookieContainer = new CookieContainer();
        private readonly int _maxCookiesSize = 4906 * 2;
        private string UserLogin { get; set; }

        //public RestServiceClient(string token)
        //{
        //}

        //public RestServiceClient()
        //{
        //}
        public RestServiceClient(string _userLogin)
        {
            this.UserLogin = _userLogin;
        }
        #endregion

        #region Các Phương Thưc Lấy Dữ Liệu (Get...)
        public IEnumerable<T> GetAll(string uri, string route)
        {
            HttpResponseMessage response = RequestData(uri, route);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<T> GetAll(string uri, string route, string routeValueName, int key)
        {
            HttpResponseMessage response = RequestData(uri, route + "?" + routeValueName + "=" + key.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        public T Post<TModel>(string uri, string route, TModel model)
        {
            HttpResponseMessage response = RequestPostData<TModel>(uri, route, model);

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }


        public IEnumerable<T> PostAll<TModel>(string uri, string route, TModel model)
        {
            HttpResponseMessage response = RequestPostData<TModel>(uri, route, model);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        public T Get(string uri, string route)
        {
            HttpResponseMessage response = RequestData(uri, route);

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);

                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        public T Get(string uri, string route, Guid key)
        {
            HttpResponseMessage response = RequestData(uri, route + key.ToString());

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }
        public T Get(string uri, string route, string key)
        {
            HttpResponseMessage response = RequestData(uri, route + key);

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        private HttpResponseMessage RequestData(string uri, string route)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = this._cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(uri);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add(HeaderObject.UserLogin, UserLogin);

                    return client.GetAsync(route).Result;
                }
            }
        }

        private HttpResponseMessage RequestPostData<TModel>(string uri, string route, TModel model)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = this._cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(uri);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add(HeaderObject.UserLogin, UserLogin);

                    return client.PostAsJsonAsync<TModel>(route, model).Result;
                }
            }
        }


        #endregion

        #region Các Phương Thức Cập Nhật Dữ Liệu (Update...)
        //public T Put(string uri, string route, T model)
        //{
        //    HttpResponseMessage response = PutData(uri, route, model);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        //var result = response.Content.ReadAsStringAsync().Result;
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        //        //var t = serializer.Deserialize<T>(result);
        //        var result = response.Content.ReadAsAsync<T>().Result;
        //        return result;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //private HttpResponseMessage PutData(string uri, string route, T obj)
        //{
        //    using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = this._cookieContainer })
        //    {
        //        using (HttpClient client = new HttpClient(handler))
        //        {
        //            client.BaseAddress = new Uri(uri);

        //            // Add an Accept header for JSON format.
        //            client.DefaultRequestHeaders.Accept.Add(
        //                new MediaTypeWithQualityHeaderValue("application/json"));

        //            return client.PutAsJsonAsync<T>(route, obj).Result;
        //        }
        //    }
        //}
        public T Put<TCondition>(string uri, string route, TCondition model)
        {
            HttpResponseMessage response = PutData(uri, route, model);

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        private HttpResponseMessage PutData<TCondition>(string uri, string route, TCondition obj)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = this._cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(uri);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add(HeaderObject.UserLogin, UserLogin);
                    
                    return client.PutAsJsonAsync<TCondition>(route, obj).Result;
                }
            }
        }
        #endregion

        #region Các Phương Thức Thêm Dữ Liệu (Insert...)
        public T Post(string uri, string route, T model)
        {
            HttpResponseMessage response = PostData(uri, route, model);

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        private HttpResponseMessage PostData(string uri, string route, T obj)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = this._cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(uri);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add(HeaderObject.UserLogin, UserLogin);

                    return client.PostAsJsonAsync<T>(route, obj).Result;
                }
            }
        }
        #endregion

        #region Các Phương Thức Xóa Dữ Liệu (Delete...)
        /// <summary>
        /// Phương thức xóa chỉ bật IsDelete = true, nhận vào id của đối tượng
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="route"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Remove(string uri, string route, Guid id)
        {
            HttpResponseMessage response = DeleteData(uri, route + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Phương thức xóa vĩnh viễn nhận vào id của đối tượng
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="route"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Delete(string uri, string route, Guid id)
        {
            HttpResponseMessage response = DeleteData(uri, route + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }

        public T Delete(string uri, string route, string id)
        {
            HttpResponseMessage response = DeleteData(uri, route + id);

            if (response.IsSuccessStatusCode)
            {
                //var result = response.Content.ReadAsStringAsync().Result;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                //var t = serializer.Deserialize<T>(result);
                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
            else
            {
                return null;
            }
        }
        //public T DeleteSelected(string uri, string route, string id)
        //{
        //    HttpResponseMessage response = DeleteData(uri, route+id);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        //var result = response.Content.ReadAsStringAsync().Result;
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        //        //var t = serializer.Deserialize<T>(result);
        //        var result = response.Content.ReadAsAsync<T>().Result;
        //        return result;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        private HttpResponseMessage DeleteData(string uri, string route)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = this._cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(uri);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Remove(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add(HeaderObject.UserLogin, UserLogin);
                    
                    return client.DeleteAsync(route).Result;
                }
            }
        }
        #endregion

        public void SetCookies(System.Web.HttpCookieCollection cookies, string url)
        {
            Uri uri = new Uri(url);
            for (int i = 0; i < cookies.Count; i++)
            {
                System.Web.HttpCookie cookie = cookies[i];
                _cookieContainer.MaxCookieSize = _maxCookiesSize;
                if (cookie.Value != null)
                    _cookieContainer.Add(uri, new Cookie(cookie.Name, cookie.Value));
            }
        }
    }
}
