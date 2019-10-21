﻿using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.AutoEFCore.Tests.Common.SpecimenBuilders
{
    public class IgnoredVirtualMembersSpecimenBuilder : ISpecimenBuilder
    {
        public IgnoredVirtualMembersSpecimenBuilder()
            : this(new IsVirtualMemberSpecification())
        {
        }

        public IgnoredVirtualMembersSpecimenBuilder(IRequestSpecification virtualMemberSpecification)
        {
            VirtualMemberSpecification = virtualMemberSpecification;
        }

        public IRequestSpecification VirtualMemberSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!VirtualMemberSpecification.IsSatisfiedBy(request))
                return new NoSpecimen();

            return new OmitSpecimen();
        }

        private class IsVirtualMemberSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is PropertyInfo property
                       && property.GetMethod.IsVirtual;
            }
        }
    }
}