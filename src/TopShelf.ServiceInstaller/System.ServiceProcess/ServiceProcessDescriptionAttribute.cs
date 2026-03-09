#if NETSTANDARD2_0
using System.ComponentModel;

namespace System.ServiceProcess
{
    /// <summary>Specifies a description for a property or event.</summary>
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class ServiceProcessDescriptionAttribute : DescriptionAttribute
    {
        private bool replaced;

        /// <summary>Gets the description stored in this attribute.</summary>
        public override string Description
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    base.DescriptionValue = base.Description;
                }
                return base.Description;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ServiceProcessDescriptionAttribute"/> class.</summary>
        /// <param name="description">The description text.</param>
        public ServiceProcessDescriptionAttribute(string description)
            : base(description)
        {
        }
    }
}

#endif
