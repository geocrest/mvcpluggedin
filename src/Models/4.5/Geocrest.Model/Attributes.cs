// -----------------------------------------------------------------------
// <copyright file="BaseNotMappedAttribute.cs" company="Geocrest Mapping, LLC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Geocrest.Model
{
    using System;

    /// <summary>
    /// Provides an attribute that lets the framework know that the property should not be mapped
    /// to a database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseNotMappedAttribute :Attribute  
    {
    }
    /// <summary>
    /// Provides an attribute that lets the framework know that the property should be explicitly 
    /// mapped to a database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseMappedAttribute : Attribute
    {
    }
    /// <summary>
    /// Provides an attribute that lets the framework know information about the property's
    /// column in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseColumnAttribute : Attribute { }
}
