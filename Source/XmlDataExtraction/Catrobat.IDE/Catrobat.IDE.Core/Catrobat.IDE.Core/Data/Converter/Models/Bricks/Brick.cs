﻿using Catrobat.Data.Xml.XmlObjects.Bricks;
using Catrobat.IDE.Core.Xml.Converter;
using Context = Catrobat.IDE.Core.Xml.Converter.XmlProjectConverter.ConvertBackContext;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Bricks
{
    partial class Brick : IXmlObjectConvertible<XmlBrick, Context>
    {
        XmlBrick IXmlObjectConvertible<XmlBrick, Context>.ToXmlObject(Context context)
        {
            // prevents endless loops
            XmlBrick result;
            if (context.Bricks.TryGetValue(this, out result)) return result;

            result = ToXmlObject2(context);
            context.Bricks[this] = result;
            return result;
        }

        protected internal abstract XmlBrick ToXmlObject2(Context context);
    }
}
