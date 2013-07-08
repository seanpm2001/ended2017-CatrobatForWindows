﻿using System.IO;
using System.Text;

namespace Catrobat.Core
{
    public class XmlStringWriter : StringWriter
    {
        private readonly Encoding _encoding;

        public XmlStringWriter(StringBuilder builder, Encoding encoding)
            : base(builder)
        {
            this._encoding = encoding;
        }

        public XmlStringWriter(Encoding encoding)
        {
            this._encoding = encoding;
        }

        public XmlStringWriter()
        {
            _encoding = Encoding.UTF8;
        }

        public override Encoding Encoding
        {
            get { return _encoding; }
        }
    }
}