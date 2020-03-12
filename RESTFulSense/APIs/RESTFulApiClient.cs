﻿// Copyright (c) Hassan Habib.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RESTFulSense.Services;

namespace RESTFulSense.APIs
{
    public class RESTFulApiClient : HttpClient
    {
        public async ValueTask<T> GetContentAsync<T>(string relativeUrl)
        {
            HttpResponseMessage responseMessage = await GetAsync(relativeUrl);
            ValidationService.ValidateHttpResponse(responseMessage);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        public async ValueTask<T> PostContentAsync<T>(string relativeUrl, T content)
        {
            StringContent contentString = StringifyJsonifyContent(content);

            HttpResponseMessage responseMessage =
               await PostAsync(relativeUrl, contentString);

            ValidationService.ValidateHttpResponse(responseMessage);

            return await DeserializeResponseContent<T>(responseMessage);
        }


        public async ValueTask<T> PutContentAsync<T>(string relativeUrl, T content)
        {
            StringContent contentString = StringifyJsonifyContent(content);

            HttpResponseMessage responseMessage =
               await PutAsync(relativeUrl, contentString);

            ValidationService.ValidateHttpResponse(responseMessage);

            return await DeserializeResponseContent<T>(responseMessage);
        }


        public async ValueTask DeleteContentAsync(string relativeUrl)
        {
            HttpResponseMessage responseMessage = await DeleteAsync(relativeUrl);
            ValidationService.ValidateHttpResponse(responseMessage);
        }

        private static async ValueTask<T> DeserializeResponseContent<T>(HttpResponseMessage responseMessage)
        {
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }

        private static StringContent StringifyJsonifyContent<T>(T content)
        {
            string serializedRestrictionRequest = JsonConvert.SerializeObject(content);

            var contentString =
                new StringContent(serializedRestrictionRequest, Encoding.UTF8, "text/json");

            return contentString;
        }
    }
}