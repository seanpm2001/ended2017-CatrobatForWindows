﻿using System.Xml.Linq;
using Catrobat.IDE.Core.Xml.XmlObjects.Formulas;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Properties
{
    public partial class XmlPointInDirectionBrick : XmlBrick
    {
        public XmlFormula Degrees { get; set; }

        public XmlPointInDirectionBrick() {}

        public XmlPointInDirectionBrick(XElement xElement) : base(xElement) {}

        internal override void LoadFromXml(XElement xRoot)
        {
            //Degrees = new XmlFormula(xRoot.Element("degrees"));
            Degrees = new XmlFormula(xRoot.Element(XmlConstants.Degrees));
        }

        internal override XElement CreateXml()
        {
            //var xRoot = new XElement("pointInDirectionBrick");
            //var xRoot = new XElement("brick");
            //xRoot.SetAttributeValue("type", "pointInDirectionBrick");
            var xRoot = new XElement(XmlConstants.Brick);
            xRoot.SetAttributeValue(XmlConstants.Type, XmlConstants.XmlPointInDirectionBrickType);

            //var xVariable = new XElement("degrees");
            var xVariable = new XElement(XmlConstants.Degrees);
            xVariable.Add(Degrees.CreateXml());
            xRoot.Add(xVariable);

            return xRoot;
        }

        internal override void LoadReference()
        {
            if (Degrees != null)
                Degrees.LoadReference();
        }
    }
}