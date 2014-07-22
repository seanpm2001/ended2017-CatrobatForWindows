﻿using System;
using Catrobat.IDE.Core.Services;

namespace Catrobat.IDE.Core.Models
{
    public class ProjectTemplateEntry : IComparable<ProjectTemplateEntry>
    {
        public string Name
        {
            get
            {
                return ProjectGenerator == null ? "" : ProjectGenerator.GetProjectDefaultName();
            }
        }

        public IProjectGenerator ProjectGenerator { get; set; }

        public ProjectTemplateEntry(IProjectGenerator projectGenerator)
        {
            ProjectGenerator = projectGenerator;
        }

        public int CompareTo(ProjectTemplateEntry other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}
