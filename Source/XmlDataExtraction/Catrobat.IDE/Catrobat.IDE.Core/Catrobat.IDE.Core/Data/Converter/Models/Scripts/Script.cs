﻿using System.Collections.Generic;
using System.Linq;
using Catrobat.Data.Xml.XmlObjects.Bricks;
using Catrobat.Data.Xml.XmlObjects.Scripts;
using Catrobat.IDE.Core.ExtensionMethods;
using Catrobat.IDE.Core.Xml.Converter;
using Context = Catrobat.IDE.Core.Xml.Converter.XmlProjectConverter.ConvertBackContext;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Scripts
{
    partial class Script : IXmlObjectConvertible<XmlScript, Context>
    {
        XmlScript IXmlObjectConvertible<XmlScript, Context>.ToXmlObject(Context context)
        {
            var result = ToXmlObject2(context);
            result.Bricks = new XmlBrickList
            {
                Bricks = Bricks == null ? new List<XmlBrick>() : Bricks.Select(brick => brick.ToXmlObject(context)).ToList()
            };
            context.Scripts.Add(this, result);
            return result;
        }

        protected internal abstract XmlScript ToXmlObject2(Context context);
    }
}
