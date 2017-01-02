﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Models.Bricks;

namespace Catrobat.IDE.Core.Utilities.Helpers
{
    public static class VariableHelper
    {
        public static ObservableCollection<GlobalVariable> GetGlobalVariableList(Program project)
        {
            return project.GlobalVariables;
        }

        public static ObservableCollection<LocalVariable> GetLocalVariableList(Program project, Sprite sprite)
        {
            return sprite != null ? sprite.LocalVariables : 
                new ObservableCollection<LocalVariable>();
        }

        public static void DeleteGlobalVariable(Program project, GlobalVariable variable)
        {
            foreach (var currentSprite in project.Sprites)
            {

                foreach (var currentScript in currentSprite.Scripts)
                {

                    foreach (var currentBrick in currentScript.Bricks)
                    {
                        if (currentBrick is SetVariableBrick || currentBrick is ChangeVariableBrick)
                        {
                            ((VariableBrick)currentBrick).Variable = null;
                        }

                    }
                }
            }

            if(project.Background.Count >= 1)
            {
                var currentSprite = project.Background[0];
                foreach (var currentScript in currentSprite.Scripts)
                {

                    foreach (var currentBrick in currentScript.Bricks)
                    {
                        if (currentBrick is SetVariableBrick || currentBrick is ChangeVariableBrick)
                        {
                            ((VariableBrick)currentBrick).Variable = null;
                        }

                    }
                }
            }
                
            project.GlobalVariables.Remove(variable);
        }

        public static void DeleteLocalVariable(Program project, Sprite sprite, LocalVariable variable)
        {
            foreach (var currentScript in sprite.Scripts)
            {

                foreach (var currentBrick in currentScript.Bricks)
                {
                    if (currentBrick is SetVariableBrick || currentBrick is ChangeVariableBrick)
                    {
                        ((VariableBrick)currentBrick).Variable = null;
                    }

                }
            }

            if (project.Background.Count >= 1)
            {
                var currentSprite = project.Background[0];
                foreach (var currentScript in currentSprite.Scripts)
                {

                    foreach (var currentBrick in currentScript.Bricks)
                    {
                        if (currentBrick is SetVariableBrick || currentBrick is ChangeVariableBrick)
                        {
                            ((VariableBrick)currentBrick).Variable = null;
                        }

                    }
                }
            }

            sprite.LocalVariables.Remove(variable);
        }

        public static void AddGlobalVariable(Program project, GlobalVariable variable)
        {
            project.GlobalVariables.Add(variable);
        }

        public static void AddLocalVariable(Program project, Sprite sprite, LocalVariable variable)
        {
            sprite.LocalVariables.Add(variable);
        }

        public static bool IsVariableLocal(Program project, Variable variable)
        {
            return variable is LocalVariable;
        }

        public static GlobalVariable CreateUniqueGlobalVariable()
        {
            return new GlobalVariable { Name = "global_" + Guid.NewGuid().ToString() };
        }

        public static LocalVariable CreateUniqueLocalVariable(Sprite sprite)
        {
            return new LocalVariable { Name = sprite.Name + "_" + Guid.NewGuid().ToString() };
        }

        public static bool VariableNameExists(Program project, Sprite sprite, string variableName)
        {
            return project.GlobalVariables.Concat<Variable>(sprite.LocalVariables)
                .Any(variable => variable.Name == variableName);
        }

        public static bool VariableNameExistsCheckSelf(Program project, Sprite sprite, Variable self, string variableName)
        {
            return project.GlobalVariables.Concat<Variable>(sprite.LocalVariables)
                .Any(variable => !ReferenceEquals(variable, self) && variable.Name == variableName);
        }
    }
}
