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
    class DocumentManager
    {
        static ConcurrentDictionary<string, string> ResourceStrings = new ConcurrentDictionary<string, string>();

        internal static string LoadResourceString(string name, DocumentType type)
        {
            string value;
            if (!ResourceStrings.TryGetValue(name, out value))
            {
                switch (type)
                {
                    case DocumentType.Embedded:
                        value = LoadEmbedded(name);
                        break;
                    case DocumentType.File:
                        value = LoadFile(name);
                        break;
                    default:
                        throw new Exception();
                }
                ResourceStrings[name] = value;
            }
            return value;
        }

        internal static string LoadResourceString(string name, DocumentType type, IDictionary<string, object> values)
        {
            string value = LoadResourceString(name, type);
            foreach (var key in values.Keys)
            {
                var val = values[key];
                value = value.Replace("@{" + key + "}", val != null ? val.ToString() : "");
            }
            return value;
        }

        internal static string LoadResourceString(string name, DocumentType type, object values)
        {
            return LoadResourceString(name, type, Map(values));
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
            return LoadResourceString(asset.Name, asset.Type, asset.Values);
        }

        static string LoadEmbedded(string name)
        {
            var assembly = typeof(DocumentManager).Assembly;
            using (var sr = new StreamReader(assembly.GetManifestResourceStream(name)))
            {
                return sr.ReadToEnd();
            }
        }

        static string LoadFile(string name)
        {
            using (StreamReader streamReader = new StreamReader(name))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
