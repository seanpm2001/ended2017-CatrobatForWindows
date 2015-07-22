﻿using System.Xml.Linq;
using Catrobat.IDE.Core.Utilities.Helpers;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Variables
{
    public class XmlUserVariableReference : XmlObjectNode
    {
        private string _reference;

        public XmlUserVariable UserVariable { get; set; }

        public XmlUserVariableReference()
        {
        }

        public XmlUserVariableReference(XElement xElement)
        {
            LoadFromXml(xElement);
        }

        internal override void LoadFromXml(XElement xRoot)
        {
            //_reference = xRoot.Attribute("reference").Value;
            _reference = xRoot.Attribute(XmlConstants.Reference).Value;
        }

        internal override XElement CreateXml()
        {
            //var xRoot = new XElement("userVariable");
            var xRoot = new XElement(XmlConstants.UserVariable);

            //xRoot.Add(new XAttribute("reference", ReferenceHelper.GetReferenceString(this)));
            xRoot.Add(new XAttribute(XmlConstants.Reference, ReferenceHelper.GetReferenceString(this)));

            return xRoot;
        }

        internal override void LoadReference()
        {
            if(UserVariable == null)
                UserVariable = ReferenceHelper.GetReferenceObject(this, _reference) as XmlUserVariable;
            if (string.IsNullOrEmpty(_reference))
                _reference = ReferenceHelper.GetReferenceString(this);
        }
    }
}
