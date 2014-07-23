﻿using Catrobat.Data.Xml.XmlObjects.Formulas;
using Catrobat.IDE.Core.Xml.Converter;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Formulas.Tree
{
    abstract partial class FormulaTree : IXmlObjectConvertible<XmlFormula>
    {
        XmlFormula IXmlObjectConvertible<XmlFormula>.ToXmlObject()
        {
            return new XmlFormula
            {
                FormulaTree = ToXmlObject2()
            };
        }

        protected internal abstract XmlFormulaTree ToXmlObject2();
    }
}
