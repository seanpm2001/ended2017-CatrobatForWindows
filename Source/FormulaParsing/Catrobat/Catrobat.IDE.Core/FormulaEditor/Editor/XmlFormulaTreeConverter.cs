﻿using System.Collections.Generic;
using Catrobat.IDE.Core.CatrobatObjects.Formulas;
using Catrobat.IDE.Core.CatrobatObjects.Formulas.FormulaNodes;
using System;
using System.Globalization;
using System.Linq;
using Catrobat.IDE.Core.CatrobatObjects.Variables;

namespace Catrobat.IDE.Core.FormulaEditor.Editor
{
    public class XmlFormulaTreeConverter
    {

        private readonly IDictionary<string, UserVariable> _userVariables;
        private readonly ObjectVariableEntry _objectVariable;

        public XmlFormulaTreeConverter(IEnumerable<UserVariable> userVariables, ObjectVariableEntry objectVariable)
        {
            _userVariables = userVariables.ToDictionary(variable => variable.Name);
            _objectVariable = objectVariable;
        }

        #region Convert

        public IFormulaTree Convert(XmlFormulaTree formula)
        {
            if (formula == null) return null;


            if (formula.VariableType == "NUMBER") return FormulaTreeFactory.CreateNumberNode(double.Parse(formula.VariableValue, CultureInfo.InvariantCulture));
            if (formula.VariableType == "OPERATOR") return ConvertOperatorNode(formula);
            if (formula.VariableType == "FUNCTION") return ConvertFunctionNode(formula);
            if (formula.VariableType == "SENSOR") return ConvertSensorOrObjectVariableNode(formula);
            if (formula.VariableType == "USER_VARIABLE") return ConvertUserVariableNode(formula);
            if (formula.VariableType == "BRACKET") return ConvertParenthesesNode(formula);

            if (String.IsNullOrEmpty(formula.VariableType)) return null;
            throw new NotImplementedException();
        }

        private IFormulaTree ConvertOperatorNode(XmlFormulaTree node)
        {
            // arithmetic
            if (node.VariableValue == "PLUS") return Convert(node, FormulaTreeFactory.CreateAddNode);
            if (node.VariableValue == "MINUS") return Convert(node, FormulaTreeFactory.CreateSubtractNode);
            if (node.VariableValue == "MULT") return Convert(node, FormulaTreeFactory.CreateMultiplyNode);
            if (node.VariableValue == "DIVIDE") return Convert(node, FormulaTreeFactory.CreateDivideNode);

            // relational operators
            if (node.VariableValue == "EQUAL") return Convert(node, FormulaTreeFactory.CreateEqualsNode);
            if (node.VariableValue == "NOTEQUAL") return Convert(node, FormulaTreeFactory.CreateNotEqualsNode);
            if (node.VariableValue == "SMALLER_THAN") return Convert(node, FormulaTreeFactory.CreateLessNode);
            if (node.VariableValue == "SMALLER_OR_EQUAL") return Convert(node, FormulaTreeFactory.CreateLessEqualNode);
            if (node.VariableValue == "GREATER_THAN") return Convert(node, FormulaTreeFactory.CreateGreaterNode);
            if (node.VariableValue == "GREATER_OR_EQUAL") return Convert(node, FormulaTreeFactory.CreateGreaterEqualNode);

            // logic
            if (node.VariableValue == "LOGICAL_AND") return Convert(node, FormulaTreeFactory.CreateAndNode);
            if (node.VariableValue == "LOGICAL_OR") return Convert(node, FormulaTreeFactory.CreateOrNode);
            if (node.VariableValue == "LOGICAL_NOT") return Convert(node, FormulaTreeFactory.CreateNotNode, false);

            throw new NotImplementedException();
        }

        private IFormulaTree ConvertFunctionNode(XmlFormulaTree node)
        {
            // numbers
            if (node.VariableValue == "PI") return Convert(node, FormulaTreeFactory.CreatePiNode);

            // logic
            if (node.VariableValue == "TRUE") return Convert(node, FormulaTreeFactory.CreateTrueNode);
            if (node.VariableValue == "FALSE") return Convert(node, FormulaTreeFactory.CreateFalseNode);

            // min/max
            if (node.VariableValue == "MIN") return Convert(node, FormulaTreeFactory.CreateMinNode);
            if (node.VariableValue == "MAX") return Convert(node, FormulaTreeFactory.CreateMaxNode);

            // exponential function and logarithms
            if (node.VariableValue == "EXP") return Convert(node, FormulaTreeFactory.CreateExpNode);
            if (node.VariableValue == "LOG") return Convert(node, FormulaTreeFactory.CreateLogNode);
            if (node.VariableValue == "LN") return Convert(node, FormulaTreeFactory.CreateLnNode);

            // trigonometric functions
            if (node.VariableValue == "SIN") return Convert(node, FormulaTreeFactory.CreateSinNode);
            if (node.VariableValue == "COS") return Convert(node, FormulaTreeFactory.CreateCosNode);
            if (node.VariableValue == "TAN") return Convert(node, FormulaTreeFactory.CreateTanNode);
            if (node.VariableValue == "ARCSIN") return Convert(node, FormulaTreeFactory.CreateArcsinNode);
            if (node.VariableValue == "ARCCOS") return Convert(node, FormulaTreeFactory.CreateArccosNode);
            if (node.VariableValue == "ARCTAN") return Convert(node, FormulaTreeFactory.CreateArctanNode);

            // miscellaneous functions
            if (node.VariableValue == "SQRT") return Convert(node, FormulaTreeFactory.CreateSqrtNode);
            if (node.VariableValue == "ABS") return Convert(node, FormulaTreeFactory.CreateAbsNode);
            if (node.VariableValue == "MOD") return Convert(node, FormulaTreeFactory.CreateModNode);
            if (node.VariableValue == "ROUND") return Convert(node, FormulaTreeFactory.CreateRoundNode);
            if (node.VariableValue == "RAND") return Convert(node, FormulaTreeFactory.CreateRandomNode);
            
            throw new NotImplementedException();
        }

        private IFormulaTree ConvertSensorOrObjectVariableNode(XmlFormulaTree node)
        {
            // sensors
            if (node.VariableValue == "ACCELERATION_X") return Convert(node, FormulaTreeFactory.CreateAccelerationXNode);
            if (node.VariableValue == "ACCELERATION_Y") return Convert(node, FormulaTreeFactory.CreateAccelerationYNode);
            if (node.VariableValue == "ACCELERATION_Z") return Convert(node, FormulaTreeFactory.CreateAccelerationZNode);
            if (node.VariableValue == "COMPASSDIRECTION") return Convert(node, FormulaTreeFactory.CreateCompassNode);
            if (node.VariableValue == "X_INCLINATION") return Convert(node, FormulaTreeFactory.CreateInclinationXNode);
            if (node.VariableValue == "Y_INCLINATION") return Convert(node, FormulaTreeFactory.CreateInclinationYNode);

            // object variables
            if (node.VariableValue == "BRIGHTNESS") return Convert(node, FormulaTreeFactory.CreateBrightnessNode);
            if (node.VariableValue == "DIRECTION") return Convert(node, FormulaTreeFactory.CreateDirectionNode);
            if (node.VariableValue == "OBJECT_GHOSTEFFECT") return Convert(node, FormulaTreeFactory.CreateGhostEffectNode);
            if (node.VariableValue == "OBJECT_LAYER") return Convert(node, FormulaTreeFactory.CreateLayerNode);
            if (node.VariableValue == "OBJECT_X") return Convert(node, FormulaTreeFactory.CreatePositionXNode);
            if (node.VariableValue == "OBJECT_Y") return Convert(node, FormulaTreeFactory.CreatePositionYNode);
            if (node.VariableValue == "OBJECT_ROTATION") return Convert(node, FormulaTreeFactory.CreateRotationNode);
            if (node.VariableValue == "SIZE") return Convert(node, FormulaTreeFactory.CreateSizeNode);
            if (node.VariableValue == "TRANSPARENCY") return Convert(node, FormulaTreeFactory.CreateOpacityNode);

            throw new NotImplementedException();
        }

        private IFormulaTree ConvertUserVariableNode(XmlFormulaTree node)
        {
            var variable = _userVariables[node.VariableValue];
            return FormulaTreeFactory.CreateUserVariableNode(variable);
        }

        private IFormulaTree ConvertParenthesesNode(XmlFormulaTree node)
        {
            return Convert(node, FormulaTreeFactory.CreateParenthesesNode, false);
        }

        private TNode Convert<TNode>(XmlFormulaTree node, Func<TNode> creator) where TNode : ConstantFormulaTree
        {
            return creator();
        }

        private TNode Convert<TNode>(XmlFormulaTree node, Func<ObjectVariableEntry, TNode> creator) where TNode : ConstantFormulaTree
        {
            return creator(_objectVariable);
        }

        private TNode Convert<TNode>(XmlFormulaTree node, Func<IFormulaTree, TNode> creator, bool leftChild = true) where TNode : UnaryFormulaTree
        {
            return creator(Convert(leftChild ? node.LeftChild : node.RightChild));
        }

        private TNode Convert<TNode>(XmlFormulaTree node, Func<IFormulaTree, IFormulaTree, TNode> creator) where TNode : BinaryFormulaTree
        {
            return creator(Convert(node.LeftChild), Convert(node.RightChild));
        }

        #endregion

        #region ConvertBack

        public XmlFormulaTree ConvertBack(IFormulaTree formula)
        {
            if (formula == null) return null;
            var type = formula.GetType();

            // numbers
            if (type == typeof(FormulaNodeNumber)) return XmlFormulaTreeFactory2.CreateNumberNode(((FormulaNodeNumber)formula).Value);
            if (type == typeof(FormulaNodePi)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreatePiNode);

            // arithmetic
            if (type == typeof(FormulaNodeAdd)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateAddNode);
            if (type == typeof(FormulaNodeSubtract)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateSubtractNode);
            if (type == typeof(FormulaNodeMultiply)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateMultiplyNode);
            if (type == typeof(FormulaNodeDivide)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateDivideNode);

            // relational operators
            if (type == typeof(FormulaNodeEquals)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateEqualsNode);
            if (type == typeof(FormulaNodeNotEquals)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateNotEqualsNode);
            if (type == typeof(FormulaNodeLess)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateLessNode);
            if (type == typeof(FormulaNodeLessEqual)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateLessEqualNode);
            if (type == typeof(FormulaNodeGreater)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateGreaterNode);
            if (type == typeof(FormulaNodeGreaterEqual)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateGreaterEqualNode);

            // logic
            if (type == typeof(FormulaNodeAnd)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateAndNode);
            if (type == typeof(FormulaNodeOr)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateOrNode);
            if (type == typeof(FormulaNodeNot)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateNotNode);
            if (type == typeof(FormulaNodeTrue)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateTrueNode);
            if (type == typeof(FormulaNodeFalse)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateFalseNode);

            // min/max
            if (type == typeof(FormulaNodeMin)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateMinNode);
            if (type == typeof(FormulaNodeMax)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateMaxNode);

            // exponential function and logarithms
            if (type == typeof(FormulaNodeExp)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateExpNode);
            if (type == typeof(FormulaNodeLog)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateLogNode);
            if (type == typeof(FormulaNodeLn)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateLnNode);

            // trigonometric functions
            if (type == typeof(FormulaNodeSin)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateSinNode);
            if (type == typeof(FormulaNodeCos)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateCosNode);
            if (type == typeof(FormulaNodeTan)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateTanNode);
            if (type == typeof(FormulaNodeArcsin)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateArcsinNode);
            if (type == typeof(FormulaNodeArccos)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateArccosNode);
            if (type == typeof(FormulaNodeArctan)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateArctanNode);

            // miscellaneous functions
            if (type == typeof(FormulaNodeSqrt)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateSqrtNode);
            if (type == typeof(FormulaNodeAbs)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateAbsNode);
            if (type == typeof(FormulaNodeMod)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateModNode);
            if (type == typeof(FormulaNodeRound)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateRoundNode);
            if (type == typeof(FormulaNodeRandom)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateRandomNode);

            // sensors
            if (type == typeof(FormulaNodeAccelerationX)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateAccelerationXNode);
            if (type == typeof(FormulaNodeAccelerationY)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateAccelerationYNode);
            if (type == typeof(FormulaNodeAccelerationZ)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateAccelerationZNode);
            if (type == typeof(FormulaNodeCompass)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateCompassNode);
            if (type == typeof(FormulaNodeInclinationX)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateInclinationXNode);
            if (type == typeof(FormulaNodeInclinationY)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateInclinationYNode);

            // object variables
            if (type == typeof(FormulaNodeBrightness)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateBrightnessNode);
            if (type == typeof(FormulaNodeDirection)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateDirectionNode);
            if (type == typeof(FormulaNodeGhostEffect)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateGhostEffectNode);
            if (type == typeof(FormulaNodeLayer)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateLayerNode);
            if (type == typeof(FormulaNodePositionX)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreatePositionXNode);
            if (type == typeof(FormulaNodePositionY)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreatePositionYNode);
            if (type == typeof(FormulaNodeRotation)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateRotationNode);
            if (type == typeof(FormulaNodeSize)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateSizeNode);
            if (type == typeof(FormulaNodeOpacity)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateOpacityNode);

            // user variables
            if (type == typeof(FormulaNodeUserVariable)) return XmlFormulaTreeFactory2.CreateUserVariableNode(((FormulaNodeUserVariable)formula).Variable);

            // brackets
            if (type == typeof(FormulaNodeParentheses)) return ConvertBack(formula, XmlFormulaTreeFactory2.CreateParenthesesNode);

            throw new NotImplementedException();
        }

        private XmlFormulaTree ConvertBack(IFormulaTree node, Func<XmlFormulaTree> constructor)
        {
            return constructor();
        }

        private XmlFormulaTree ConvertBack(IFormulaTree node, Func<XmlFormulaTree, XmlFormulaTree> constructor)
        {
            return constructor(ConvertBack(node.Children.ElementAt(0)));
        }

        private XmlFormulaTree ConvertBack(IFormulaTree node, Func<XmlFormulaTree, XmlFormulaTree, XmlFormulaTree> constructor)
        {
            return constructor(ConvertBack(node.Children.ElementAt(0)), ConvertBack(node.Children.ElementAt(1)));
        }

        #endregion

    }
}
