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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace TechJini.TransformDocument
{
    public class TransformDocument
    {
        static ConcurrentDictionary<string, string> ResourceStrings = new ConcurrentDictionary<string, string>();

        public static string LoadContentString(string value, IDictionary<string, object> values)
        {
            foreach (var key in values.Keys)
            {
                var val = values[key];
                value = value.Replace("@{" + key + "}", val != null ? val.ToString() : "");
            }
            return value;
        }

        public static string LoadContentString(string content, object values)
        {
            return LoadContentString(content, Map(values));
        }

        static IDictionary<string, object> Map(object values)
        {
            var dictionary = values as IDictionary<string, object>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(values));
                }
            }

            return dictionary;
        }

        public static string LoadResourceString(Document asset)
        {
            string value;

            if (asset.Type == DocumentType.Content)
            {
                return LoadContentString(asset.Content, asset.Values);
            }

            if (!ResourceStrings.TryGetValue(asset.Name, out value))
            {
                switch (asset.Type)
                {
                    case DocumentType.Embedded:
                        value = LoadEmbeddedString(asset.Name, asset.Values);
                        break;
                    case DocumentType.File:
                        value = LoadFileString(asset.Path, asset.Values);
                        break;
                    case DocumentType.Url:
                        value = LoadUrlString(asset.Url, asset.Values);
                        break;
                    default:
                        throw new Exception();
                }
                ResourceStrings[asset.Name] = value;
            }
            return value;
        }

        public static string LoadEmbeddedString(string name, object value)
        {
            string content;
            var assembly = typeof(TransformDocument).Assembly;
            using (var sr = new StreamReader(assembly.GetManifestResourceStream(name)))
            {
                content = sr.ReadToEnd();
            }

            return LoadContentString(content, value);
        }

        public static string LoadFileString(string name, object value)
        {
            string content;
            using (StreamReader streamReader = new StreamReader(name))
            {
                content = streamReader.ReadToEnd();
            }

            return LoadContentString(content, value);
        }

        static string LoadUrlString(Uri name, object value)
        {
            throw new Exception();
        }
    }
}
