/*
 * Copyright 2016 Jatin Kumar, TechJini Solutions Private Limited
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TechJini.TransformDocument
{
    public class TransformHttpResult : IHttpActionResult, ITransformDocument
    {
        private Document assetInfo;

        public TransformHttpResult(Document asset)
        {
            assetInfo = asset;
        }

        public Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(GetResponseMessage());
        }

        public HttpResponseMessage GetResponseMessage()
        {
            var html = TransformDocument.LoadResourceString(assetInfo);

            return new HttpResponseMessage()
            {
                Content = new StringContent(html, Encoding.UTF8, "text/html")
            };
        }

    }
}
