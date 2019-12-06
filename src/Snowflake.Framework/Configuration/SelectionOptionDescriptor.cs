using System;
using System.Collections.Generic;
using System.Text;
using EnumsNET;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class SelectionOptionDescriptor : ISelectionOptionDescriptor
    {
        /// <inheritdoc/>
        public string DisplayName { get; }

        /// <inheritdoc/>
        public bool Private { get; }

        /// <inheritdoc/>
        public string SerializeAs { get; }

        /// <inheritdoc/>
        public int NumericValue { get; }

        /// <inheritdoc/>
        public string EnumName { get; }

        /// <inheritdoc/>
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
            this.DisplayName = selection?.DisplayName ?? selectionEnum.Name;
            this.EnumName = selectionEnum.Name;
            this.EnumType = selectionEnum.Value.GetType();
            this.Private = selection?.Private ?? true;
            this.SerializeAs = selection?.SerializeAs ?? String.Empty;
            this.NumericValue = selectionEnum.ToInt32();
        }
    }
}
