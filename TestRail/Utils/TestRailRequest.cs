using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TestRail.Enums;

namespace TestRail.Utils
{
    /// <summary>
    /// Wrapper class for building and sending an Http Web Request.
    /// </summary>
    public class TestRailRequest
    {
        private readonly HttpWebRequest _request;
        private string _boundary;
        private byte[] _boundaryBytes;

        /// <summary>constructor</summary>
        /// <param name="url">url for the request</param>
        /// <param name="requestType">designate the request being built as a GET or POST request</param>
        public TestRailRequest(string url, string requestType)
        {
            _request = (HttpWebRequest)WebRequest.Create(url);
            _request.AllowAutoRedirect = true;
            _request.UserAgent = "TestRail Client for .NET";
            _request.Method = requestType;
        }

        /// <summary>add headers to the request</summary>
        /// <param name="headers">key value pairs to be added to the headers</param>
        public void AddHeaders(IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                _request.Headers[header.Key] = header.Value;
            }
        }

        /// <summary>what type of data the request will accept</summary>
        /// <param name="accept">set the type, ex: 'application/json'</param>
        public void Accepts(string accept)
        {
            _request.Accept = accept;
        }

        /// <summary>what type of data the request contains</summary>
        /// <param name="contentType">set the type, ex: 'application/json'</param>
        public void ContentType(ContentType contentType)
        {
            switch (contentType)
            {
                case Enums.ContentType.Json:
                    _request.ContentType = contentType.GetStringValue();
                    break;
                case Enums.ContentType.Multipart:
                    _request.ContentType = $"{contentType.GetStringValue()}; boundary={_GetFormDataBoundary()}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
            }
        }

        /// <summary>add a body to the request</summary>
        /// <param name="bodyString">the text to add to the body</param>
        public void AddBody(string bodyString)
        {
            var byteArray = Encoding.UTF8.GetBytes(bodyString);

            _request.ContentLength = byteArray.Length;

            var requestDataStream = _request.GetRequestStream();

            requestDataStream.Write(byteArray, 0, byteArray.Length);
            requestDataStream.Close();
        }

        public void AttachFile(string filePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>send the request and get a response</summary>
        /// <typeparam name="T">the type to deserialize to</typeparam>
        /// <returns>if successful, will return a new RequestResult object</returns>
        public RequestResult<T> Execute<T>()
        {
            var response = (HttpWebResponse)_request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
            {
                var responseFromServer = reader.ReadToEnd();

                return new RequestResult<T>(response.StatusCode, responseFromServer);
            }
        }

        private string _GetFormDataBoundary()
        {
            return _boundary ?? (_boundary = $"---------------------------{DateTime.Now.Ticks:x}");
        }

        private byte[] _GetBoundaryBytes()
        {
            return _boundaryBytes ?? (_boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + _boundary + "\r\n"));
        }
    }
}
