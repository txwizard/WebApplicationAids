/*
    ============================================================================

    Namespace Name:     WizardWrx.WebApplicationAids

    Class Name:         ConfigFileReaders

    Source Module Name: ConfigFileReaders.cs

    Synopsis:           See triple-slash XML comment.

    Renarks:            The initial implementation exposes a static method that
                        enables any assembly to query a we.config file stored at
                        a known location in a filesystem for a
                        database connection string. I created and tested the
                        method in ConfigLab, a project that i use to test new
                        algorithms.

    References:         
 
    Author:             David A. Gray

    License:            Copyright (C) 2021, David A. Gray. 
                        All rights reserved.

                        Redistribution and use in source and binary forms, with
                        or without modification, are permitted provided that the
                        following conditions are met:

                        *   Redistributions of source code must retain the above
                            copyright notice, this list of conditions and the
                            following disclaimer.

                        *   Redistributions in binary form must reproduce the
                            above copyright notice, this list of conditions and
                            the following disclaimer in the documentation and/or
                            other materials provided with the distribution.

                        *   Neither the name of David A. Gray, nor the names of
                            his contributors may be used to endorse or promote
                            products derived from this software without specific
                            prior written permission.

                        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
                        CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
                        WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
                        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
                        PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
                        David A. Gray BE LIABLE FOR ANY DIRECT, INDIRECT,
                        INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
                        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
                        SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
                        PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
                        ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
                        LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
                        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
                        IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

	----------------------------------------------------------------------------
    Revision History
	----------------------------------------------------------------------------

    Date       Version By  Description
    ---------- ------- --- -----------------------------------------------------
    2021/12/16 1.0.0   DAG Initial version.

	2021/12/19 1.0.4   DAG Fix the same object reference error that I fixed in
                           the previous version before I released version 1.0.2.

    2022/04/03 1.0.20  DAG Implement method GetAppSettingsKeysFromAnyConfig.
    ============================================================================
*/

using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

using WizardWrx.Core;   // Added for TraceLogger.WriteWithBothTimesLabeledLocalFirst


namespace WizardWrx.WebApplicationAids
{
    /// <summary>
    /// This static (Shared in Visual Basic) class exposes static (Shared in
    /// Visual Basic) methods that enable any assembly, be it EXE or or DLL, to
    /// query web.config files for values.
    /// </summary>
    public static class ConfigFileReaders
    {
        /// <summary>
        /// A member of this enumeration is the optional penmConfigFileType
        /// argument to GetAppSettingsKeysFromAnyConfig.
        /// </summary>
        public enum ConfigFileType
        {
            /// <summary>
            /// The configuration file type is unknown at compile time. The
            /// routine is on its own to figure it out.
            /// </summary>
            Unknown,

            /// <summary>
            /// The configuration file is a web configuration file.
            /// </summary>
            IsWebConfig,

            /// <summary>
            /// The configuration file belongs to an application.
            /// </summary>
            IsAppConfig,

            /// <summary>
            /// If the file name is web.config, treat it as a web configuration
            /// file. Otherwise, treat it as an application configuration file.
            /// </summary>
            DeriveFromFileName,
        };  // public enum ConfigFileType


        /// <summary>
        /// Get the connection string identified by name by string
        /// <paramref name="pstrConnectionStringName"/> from the official
        /// connectionStrings section of the web.config file specified by the
        /// relative or absolute file named in string
        /// <paramref name="pstrAbsoluteConfigFileName"/>.
        /// </summary>
        /// <param name="pstrAbsoluteConfigFileName">
        /// This string must be either an absolute (fully qualified) file name
        /// or a name that is valid relative to the current working directory.
        /// </param>
        /// <param name="pstrConnectionStringName">
        /// This string must contain the "name" property of a node in the
        /// connectionStrings section of a well-formed web.config file.
        /// </param>
        /// <returns>
        /// <para>
        /// If it succeeds, the return value is a ConnectionStringSettings
        /// object that has valid name and connectionString properties.
        /// Otherwise, the return value is NULL.
        /// </para>
        /// <para>
        /// Its providerName property may have a value, or it may be null.
        /// </para>
        /// <para>
        /// Regardless, the returned ConnectionStringSettings object can be
        /// treated as if it had been read from a web.config file in the usual
        /// way.
        /// </para>
        /// </returns>
        /// <remarks>
        /// This method works by treating the web.config file as a standard XML
        /// document.
        /// </remarks>
        public static ConnectionStringSettings GetConnectionStringFromWebConfig (
            string pstrAbsoluteConfigFileName ,
            string pstrConnectionStringName )
        {
            IEnumerator inodeEnumerator1 = GetXmlNodeElementEnumerator ( pstrAbsoluteConfigFileName );

            while ( inodeEnumerator1.MoveNext ( ) )
            {
                XmlElement xmlGrandChildOfRootNode = ( XmlElement ) inodeEnumerator1.Current;

                if ( xmlGrandChildOfRootNode.LocalName == @"connectionStrings" )
                {
                    IEnumerator inodeEnumerator3 = xmlGrandChildOfRootNode.GetEnumerator ( );

                    while ( inodeEnumerator3.MoveNext ( ) )
                    {
                        XmlElement xmlGreatGrandChildOfRootNode = ( XmlElement ) inodeEnumerator3.Current;

                        switch ( xmlGreatGrandChildOfRootNode.Attributes.Count )
                        {
                            case MagicNumbers.PLUS_THREE:
                                {   // Constrain the scope of the four objects declared herein.
                                    XmlNode xmlNameAttribute = xmlGreatGrandChildOfRootNode.Attributes.GetNamedItem ( @"name" );
                                    XmlNode xmlConnectionStringAttribute = xmlGreatGrandChildOfRootNode.Attributes.GetNamedItem ( @"connectionString" );
                                    XmlNode xmlProviderNameAttribute = xmlGreatGrandChildOfRootNode.Attributes.GetNamedItem ( @"providerName" );

                                    if ( xmlNameAttribute.Value == pstrConnectionStringName )
                                    {
                                        ConnectionStringSettings rconnSettings = new ConnectionStringSettings
                                        {
                                            Name = pstrConnectionStringName ,
                                            ConnectionString = xmlConnectionStringAttribute.Value ,
                                            ProviderName = xmlProviderNameAttribute.Value
                                        };

                                        return rconnSettings;
                                    }   // if ( xml1.Value == pstrConnectionStringName )
                                }   // All four objects go out of scope as the function happens also to return.

                                break;  // case MagicNumbers.PLUS_THREE:

                            case MagicNumbers.PLUS_TWO:
                                {   // Constrain the scope of the four objects declared herein.
                                    XmlNode xmlNameAttribute = xmlGreatGrandChildOfRootNode.Attributes.GetNamedItem ( @"name" );
                                    XmlNode xmlConnectionStringAttribute = xmlGreatGrandChildOfRootNode.Attributes.GetNamedItem ( @"connectionString" );

                                    if ( xmlNameAttribute.Value == pstrConnectionStringName )
                                    {
                                        ConnectionStringSettings rconnSettings = new ConnectionStringSettings
                                        {
                                            Name = pstrConnectionStringName ,
                                            ConnectionString = xmlConnectionStringAttribute.Value
                                        };

                                        return rconnSettings;
                                    }   // if ( xml1.Value == pstrConnectionStringName )
                                }   // All four objects go out of scope as the function happens also to return.

                                break;  // case MagicNumbers.PLUS_TWO:

                            default:
                                return null;    // This should throw an exception.
                        }   // switch ( xmlGrandChildOfRootNode.Attributes.Count )
                    }   // while ( inodeEnumerator3.MoveNext ( ) )
                }   // if ( xmlGrandChildOfRootNode.LocalName == @"connectionStrings" )
            }   // while ( inodeEnumerator2.MoveNext ( ) )

            return null;    // This should throw an exception.
        }   // public static ConnectionStringSettings


        /// <summary>
        /// Extract the application settings keys from a configuration file.
        /// </summary>
        /// <param name="pstrAbsoluteConfigFileName">
		/// This string must be either an absolute (fully qualified) file name
		/// or a name that is valid relative to the current working directory.
        /// </param>
        /// <param name="penmConfigFileType">
        /// This optional argument offers a hint in the form of a ConfigFileType
        /// enumeration member, to help identify the structure of the configuration
        /// file.
        /// </param>
        /// <param name="pastrKeyNames">
        /// Pass in the list of names to include, or pass a null reference to
        /// cause all keys to be returned.
        /// </param>
        /// <returns>
        /// This method returns a System.Collections.Specialized.NameValueCollection
        /// that contains the keys specified by <paramref name="pastrKeyNames"/>,
        /// or all keys if <paramref name="pastrKeyNames"/> is null, contained
        /// in configuration file <paramref name="pstrAbsoluteConfigFileName"/>.
        /// </returns>
        public static NameValueCollection GetAppSettingsKeysFromAnyConfig (
            string pstrAbsoluteConfigFileName ,
            ConfigFileType penmConfigFileType = ConfigFileType.Unknown ,
            string [ ] pastrKeyNames = null )
        {
            NameValueCollection rnvcAppSettings = new NameValueCollection ( );

            IEnumerator inodeEnumerator1 = GetXmlNodeElementEnumerator ( pstrAbsoluteConfigFileName );
            bool fLooking4AppSettingsSection = true;

            while ( fLooking4AppSettingsSection && inodeEnumerator1.MoveNext ( ) )
            {
                XmlElement xmlChildOfRootNode = ( XmlElement ) inodeEnumerator1.Current;

                if ( xmlChildOfRootNode.Name == @"applicationSettings" || xmlChildOfRootNode.Name == @"appSettings" )
                {
                    fLooking4AppSettingsSection = false;
                    int intChildNodeCount = xmlChildOfRootNode.ChildNodes.Count;
                    TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                        string.Format (
                            @"In method GetAppSettingsKeysFromAnyConfig, found XML Node named {0} with {1} children." ,
                            xmlChildOfRootNode.Name ,
                            intChildNodeCount ) );

                    for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                              intJ < intChildNodeCount ;
                              intJ++ )
                    {
                        if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlElement )
                        {
                            XmlElement xmlAppSettingsKey = ( XmlElement ) xmlChildOfRootNode.ChildNodes [ intJ ];

                            if ( xmlAppSettingsKey.Attributes.Count == MagicNumbers.PLUS_TWO )
                            {
                                XmlAttribute xmlNameAttribute = ( XmlAttribute ) xmlAppSettingsKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ];
                                XmlAttribute xmlValueAttribute = ( XmlAttribute ) xmlAppSettingsKey.Attributes [ ArrayInfo.ARRAY_SECOND_ELEMENT ];
                                
                                if ( xmlNameAttribute.Name == @"key" && xmlValueAttribute.Name == @"value" )
                                {
                                    rnvcAppSettings.Add (
                                        xmlNameAttribute.Value ,
                                        xmlValueAttribute.Value );
                                    TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                        string.Format (
                                            @"    Node {0} of {1}: Name = {2}, Children = {3}, Name = {4}, Value = {5}" ,                                       // Format Control String
                                            ArrayInfo.OrdinalFromIndex ( intJ ) ,                                                                               // Format Item 0: Node {0}
                                            intChildNodeCount ,                                                                                                 // Format Item 1: of {1}
                                            xmlAppSettingsKey.Name ,                                                                                            // Format Item 2: Name = {2}
                                            xmlAppSettingsKey.ChildNodes.Count ,                                                                                // Format Item 3: Children = {3}
                                            xmlNameAttribute.Value ,                                                                                            // Format Item 4: Name = {4}
                                            xmlValueAttribute.Value ) );                                                                                        // Format Item 5: Value = {5}
                                }   // TRUE (anticipated outcome) block, if ( xmlNameAttribute.Name == @"key" && xmlValueAttribute.Name == @"value" )
                                else
                                {
                                    TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                        string.Format (
                                            @"    Node {0} of {1}: Name = {2}, Children = {3}, Attribute0Name = {4}, Attribute1Name = {5}" ,                    // Format Control String
                                            ArrayInfo.OrdinalFromIndex ( intJ ) ,                                                                               // Format Item 0: Node {0}
                                            intChildNodeCount ,                                                                                                 // Format Item 1: of {1}
                                            xmlAppSettingsKey.Name ,                                                                                            // Format Item 2: Name = {2}
                                            xmlAppSettingsKey.ChildNodes.Count ,                                                                                // Format Item 3: Children = {3}
                                            xmlNameAttribute.Name ,                                                                                             // Format Item 4: Attribute0Name = {4}
                                            xmlValueAttribute.Name ) );                                                                                         // Format Item 5: Attribute1Name = {5}
                                }   // FALSE (unanticipated outcome) block, if ( xmlNameAttribute.Name == @"key" && xmlValueAttribute.Name == @"value" )
                            }   // TRUE (anticipated outcome) block, if ( xmlAppSettingsKey.Attributes.Count == MagicNumbers.PLUS_TWO )
                            else
                            {
                                TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                    string.Format (
                                        @"    Node {0} of {1}: Name = {2}, Children = {3}, Actual attribute count = {4}, Expected attribute count = {5}" ,      // Format Control String
                                        ArrayInfo.OrdinalFromIndex ( intJ ) ,                                                                                   // Format Item 0: Node {0}
                                        intChildNodeCount ,                                                                                                     // Format Item 1: of {1}
                                        xmlAppSettingsKey.Name ,                                                                                                // Format Item 2: Name = {2}
                                        xmlAppSettingsKey.ChildNodes.Count ,                                                                                    // Format Item 3: Children = {3}
                                        xmlAppSettingsKey.Attributes.Count ,                                                                                    // Format Item 4: Actual attribute count = {4}
                                        MagicNumbers.PLUS_TWO ) );                                                                                              // Format Item 5: Expected attribute count = {5}
                            }   // FALSE (unanticipated outcome) block, if ( xmlAppSettingsKey.Attributes.Count == MagicNumbers.PLUS_TWO )
                        }   // TRUE (anticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is System.Xml.XmlElement )
                        else
                        {
                            if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlComment )
                            {
                                XmlComment comment = ( XmlComment ) xmlChildOfRootNode.ChildNodes [ intJ ];
                                TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                    string.Format (
                                        @"    Node {0} of {1}: Name = {2}, Children = {3}, InnerText = {4}" ,               // Format Control String
                                        ArrayInfo.OrdinalFromIndex ( intJ ) ,                                               // Format Item 0: Node {0}
                                        intChildNodeCount ,                                                                 // Format Item 1: of {1}
                                        comment.Name ,                                                                      // Format Item 2: Name = {2}
                                        comment.ChildNodes.Count ,                                                          // Format Item 3: Children = {3}
                                        comment.InnerText ) );                                                              // Format Item 4: InnerText = {4}
                            }   // TRUE (anticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlComment )
                            else
                            {
                                TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                    string.Format (
                                        @"    Node {0} of {1}: Type = {2}" ,                                                // Format Control String
                                        ArrayInfo.OrdinalFromIndex ( intJ ) ,                                               // Format Item 0: Node {0}
                                        intChildNodeCount ,                                                                 // Format Item 1: of {1}
                                        xmlChildOfRootNode.ChildNodes [ intJ ].GetType ( ).FullName ) );                    // Format Item 2: Type = {2}
                            }   // FALSE (unanticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlComment )
                        }   // FALSE (unanticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is System.Xml.XmlElement )
                    }   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < intChildNodeCount ; intJ++ )
                }   // if ( xmlChildOfRootNode.Name == @"applicationSettings" || xmlChildOfRootNode.Name == @"appSettings" )
            }   // while ( fLooking4AppSettingsSection && inodeEnumerator1.MoveNext ( ) )

            return rnvcAppSettings;
        }   // public static NameValueCollection GetAppSettingsKeysFromAnyConfig


        /// <summary>
        /// Get the System.Collections.IEnumerator for the configuration element
        /// (node) of the XML configuration document specified by string
        /// <paramref name="pstrAbsoluteConfigFileName"/>.
        /// </summary>
        /// <param name="pstrAbsoluteConfigFileName">
		/// This string must be either an absolute (fully qualified) file name
		/// or a name that is valid relative to the current working directory.
        /// This parameter is a pass-through from the caller's argument list.
        /// </param>
        /// <returns>
        /// The return value is the System.Collections.IEnumerator for a web or
        /// application configuration file.
        /// </returns>
        private static IEnumerator GetXmlNodeElementEnumerator ( string pstrAbsoluteConfigFileName )
        {
            XmlDocument xml = new XmlDocument ( );
            xml.Load ( pstrAbsoluteConfigFileName );
            XmlNodeList xmlRootElement = xml.GetElementsByTagName ( @"configuration" );
            IEnumerator enumerator1 = xmlRootElement.GetEnumerator ( );

            while ( enumerator1.MoveNext ( ) )
            {   // Since the generic enumerator returns an equally generic object, the type cast must be explicit.
                XmlElement xmlChildOfRootNode = ( XmlElement ) enumerator1.Current;

                if ( xmlChildOfRootNode.Name == @"configuration" )
                {
                    return xmlChildOfRootNode.GetEnumerator ( );
                }   // if ( xmlChildOfRootNode.Name == @"configuration" )
            }   // while ( enumerator1.MoveNext ( ) )

            return null;
        }   // private static IEnumerator GetXmlNodeElementEnumerator
    }   // public static class ConfigFileReaders
}   // partial WizardWrx.WebApplicationAids