using EnumsNET;
using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration
{
    public class SelectionOptionDescriptor : ISelectionOptionDescriptor
    {
        public string DisplayName { get; }
        public bool Private { get; }
        public string SerializeAs { get; }
        public int NumericValue { get; }
        public string EnumName { get; }
        public Type EnumType { get; }

        internal SelectionOptionDescriptor(EnumMember selectionEnum)
        {
            if (!selectionEnum.Attributes.Has<SelectionOptionAttribute>())
            {
                throw new MissingMemberException("Selection options must all be decorated!");
            }
            if (selectionEnum.GetUnderlyingValue().GetType() != typeof(int))
            {
                throw new InvalidCastException("Selection options must have underlying value of type Int32.");
            }
            var selection = selectionEnum.Attributes.Get<SelectionOptionAttribute>();
            this.DisplayName = selection.DisplayName ?? selectionEnum.Name;
            this.EnumName = selectionEnum.Name;
            this.EnumType = selectionEnum.Value.GetType();
            this.Private = selection.Private;
            this.SerializeAs = selection.SerializeAs;
            this.NumericValue = selectionEnum.ToInt32();
        }
    }
}
