﻿/*
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

namespace TechJini.TransformDocument
{
    public class TjTrDoc
    {
        /// <summary>
        /// If the template is in the form of a string
        /// </summary>
        public string Content { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// if the template is in a file
        /// </summary>
        public string Path {
            get
            {
                return _path;
            }
            set
            {
                Name = value;
                _path = value;
            }
        }

        /// <summary>
        /// is template in string, file, 
        /// </summary>
        public TjTrDocTypes Type { get; set; }

        /// <summary>
        /// if the template is stored in a url
        /// </summary>
        public Uri Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                Name = _url.AbsolutePath;
            }
        }

        /// <summary>
        /// the values that needs to be applied to the template
        /// </summary>
        public object Values { get; set; }

        string _path;
        Uri _url;
    }
}
