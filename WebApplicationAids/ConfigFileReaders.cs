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

    License:            Copyright (C) 2021-2022, David A. Gray. 
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

    2022/04/03 1.0.29  DAG Implement method GetAppSettingsKeysFromAnyConfig.
    ============================================================================
*/


using System;

using System.Collections;
using System.Collections.Generic;
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
        /// <exception cref="InvalidOperationException">
        /// An InvalidOperationException Exception arises when the sections are
        /// nested more than two deep.
        /// </exception>
        public static SortedDictionary<string,object> GetAppSettingsKeysFromAnyConfig (
            string pstrAbsoluteConfigFileName ,            
            string [ ] pastrKeyNames = null )
        {
            HashSet<string> shsKeyNames = null;

            if ( pastrKeyNames != null )
            {
                shsKeyNames = new HashSet<string> ( pastrKeyNames.Length );
                int intNKeyNames = pastrKeyNames.Length;

                for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                          intJ < intNKeyNames ;
                          intJ++ )
                {
                    if ( !shsKeyNames.Contains ( pastrKeyNames [ intJ ] ) )
                    {
                        shsKeyNames.Add ( pastrKeyNames [ intJ ] );
                    }   // if ( !shsKeyNames.Contains ( pastrKeyNames [ intJ ] ) )
                }   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < intNKeyNames ; intJ++ )
            }   // if ( pastrKeyNames != null)

            SortedDictionary<string , object> rdctAppSettings = new SortedDictionary<string , object> ( );

            IEnumerator inodeEnumerator1 = GetXmlNodeElementEnumerator ( pstrAbsoluteConfigFileName );
            bool fLooking4AppSettingsSection = true;

            while ( fLooking4AppSettingsSection && inodeEnumerator1.MoveNext ( ) )
            {
                XmlElement xmlChildOfRootNode = ( XmlElement ) inodeEnumerator1.Current;

                string strChildOfRootNameName = xmlChildOfRootNode.Name;

                if ( strChildOfRootNameName == Properties.Resources.APP_SETTINGS_NAME_APP_CONFIG || strChildOfRootNameName == Properties.Resources.APP_SETTINGS_NAME_WEB_CONFIG )
                {
                    fLooking4AppSettingsSection = false;
                    int intChildNodeCount = xmlChildOfRootNode.ChildNodes.Count;
                    TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                        string.Format (
                            Properties.Resources.TRACEMSG_1 ,                   // Format Control String
                            strChildOfRootNameName ,                            // Format Item 0: In method GetAppSettingsKeysFromAnyConfig, found XML Node named {0}
                            intChildNodeCount ) );                              // Format Item 1: with {1} children.

                    for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                              intJ < intChildNodeCount ;
                              intJ++ )
                    {
                        if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlElement )
                        {
                            XmlElement xmlAppSettingsKey = ( XmlElement ) xmlChildOfRootNode.ChildNodes [ intJ ];
                            int intGrandchildCount = xmlAppSettingsKey.ChildNodes.Count;

                            if ( intGrandchildCount == ListInfo.LIST_IS_EMPTY )
                            {
                                AddElementToDoctionary (
                                    rdctAppSettings ,
                                    intChildNodeCount ,
                                    intJ ,
                                    xmlAppSettingsKey,
                                    shsKeyNames );
                            }   // TRUE (The element is childless.) block, if ( intGrandchildCount == ListInfo.LIST_IS_EMPTY )
                            else
                            {   // Create a new section, which gets its own SortedDictionary object.
                                SortedDictionary<string , object> dctNamedConfigSection = new SortedDictionary<string , object> ( );
                                rdctAppSettings.Add (
                                    xmlAppSettingsKey.Name ,                    // string key
                                    dctNamedConfigSection );                    // object value

                                for ( int intK = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                                          intK < intGrandchildCount ;
                                          intK++ )
                                {
                                    if ( xmlAppSettingsKey.ChildNodes [ intJ ] is XmlElement )
                                    {
                                        XmlElement xmlAppSettingsSectionKey = ( XmlElement ) xmlAppSettingsKey.ChildNodes [ intK ];
                                        int intGreatGrandchildCount = xmlAppSettingsSectionKey.ChildNodes.Count;

                                        switch ( intGreatGrandchildCount )
                                        {   // xmlAppSettingsSectionKey is a configuration value; add it to the list.
                                            case MagicNumbers.ZERO:
                                                AddElementToDoctionary (
                                                    dctNamedConfigSection ,
                                                    intGreatGrandchildCount ,
                                                    intK ,
                                                    xmlAppSettingsSectionKey ,
                                                    shsKeyNames );

                                                break;  // Case MagicNumbers.ZERO:

                                            case MagicNumbers.PLUS_ONE:
                                                if ( xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_NAME && xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_SECOND_ELEMENT ].Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_SERIALIZEAS )
                                                {   // The value lives in the first and only child node.
                                                    if ( IncludeKeyInList ( xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Value , shsKeyNames ) )
                                                    {
                                                        dctNamedConfigSection.Add (
                                                            xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Value ,       // string key
                                                            xmlAppSettingsSectionKey.ChildNodes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].InnerText );  // object value
                                                    }   // if ( IncludeKeyInList ( xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Value , shsKeyNames ) )
                                                }   // TRUE (anticipated outcome) block, if ( xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_NAME && xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_SECOND_ELEMENT ].Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_SERIALIZEAS )
                                                else
                                                {
                                                    throw new InvalidOperationException (
                                                        string.Format ( Properties.Resources.ERRMSG_SECTION_UNEXPECTED_GEOMETRY ,               // Format Control String
                                                        xmlAppSettingsSectionKey ,                                                              // Format Item 0: XML Node {0}
                                                        xmlAppSettingsKey.Name ,                                                                // Format Item 1: of parent node {1}
                                                        xmlAppSettingsSectionKey.Attributes [ 0 ].Name ,                                        // Format Item 2: has unexpected attributes named {2}
                                                        xmlAppSettingsSectionKey.Attributes [ 1 ].Name ) );                                     // Format Item 3: and {3}.
                                                }   // FALSE (unanticipated outcome) block, if ( xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ].Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_NAME && xmlAppSettingsSectionKey.Attributes [ ArrayInfo.ARRAY_SECOND_ELEMENT ].Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_SERIALIZEAS )

                                                break;  // Case MagicNumbers.PLUS_ONE

                                            default:
                                                throw new InvalidOperationException (
                                                    string.Format (
                                                        Properties.Resources.ERRMSG_SECTIONS_NESTED_TOO_DEEPLY ,                                // Format Control String
                                                        xmlAppSettingsSectionKey.Name ,                                                         // Format Item 0: XML Element named {0}
                                                        xmlAppSettingsKey.Name ) );                                                             // Format Item 1: of element named {1}
                                        }   // switch ( intGreatGrandchildCount )
                                    }   // if ( xmlAppSettingsKey.ChildNodes [ intJ ] is XmlElement )
                                }   // for ( int intK = ArrayInfo.ARRAY_FIRST_ELEMENT ; intK < intGrandchildCount ; intK++ )
                            }   // FALSE (The element has children.) block, if ( intGrandchildCount == ListInfo.LIST_IS_EMPTY )
                        }   // TRUE (anticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is System.Xml.XmlElement )
                        else
                        {
                            if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlComment )
                            {
                                XmlComment comment = ( XmlComment ) xmlChildOfRootNode.ChildNodes [ intJ ];
                                TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                    string.Format (
                                        Properties.Resources.TRACEMSG_2 ,       // Format Control String
                                        ArrayInfo.OrdinalFromIndex ( intJ ) ,   // Format Item 0: Node {0}
                                        intChildNodeCount ,                     // Format Item 1: of {1}
                                        comment.Name ,                          // Format Item 2: Name = {2}
                                        comment.ChildNodes.Count ,              // Format Item 3: Children = {3}
                                        comment.InnerText ) );                  // Format Item 4: InnerText = {4}
                            }   // TRUE (anticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlComment )
                            else
                            {
                                TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                                    string.Format (
                                        Properties.Resources.TRACEMSG_3 ,       // Format Control String
                                        ArrayInfo.OrdinalFromIndex ( intJ ) ,   // Format Item 0: Node {0}
                                        intChildNodeCount ,                     // Format Item 1: of {1}
                                        xmlChildOfRootNode.ChildNodes [ intJ ].GetType ( ).FullName ) );    // Format Item 2: Type = {2}
                            }   // FALSE (unanticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is XmlComment )
                        }   // FALSE (unanticipated outcome) block, if ( xmlChildOfRootNode.ChildNodes [ intJ ] is System.Xml.XmlElement )
                    }   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < intChildNodeCount ; intJ++ )
                }   // if ( STRcHOLDoFrOOTnODEnAME == Properties.Resources.APP_SETTINGS_NAME_APP_CONFIG || STRcHOLDoFrOOTnODEnAME == Properties.Resources.APP_SETTINGS_NAME_WEB_CONFIG )
            }   // while ( fLooking4AppSettingsSection && inodeEnumerator1.MoveNext ( ) )

            return rdctAppSettings;
        }   // public static NameValueCollection GetAppSettingsKeysFromAnyConfig


        /// <summary>
        /// <para>
        /// Assuming that <paramref name="pxmlAppSettingsKey"/> has 2 attributes
        /// named "key" and "value" respectively, construct a new key/value pair
        /// and append it to <paramref name="pdctAppSettings"/>,
        /// </para>
        /// <para>
        /// The other two arguments, <paramref name="pntChildNodeCount"/> and
        /// <paramref name="pintJ"/>, go into the trace logger to give the other
        /// information context.
        /// </para>
        /// </summary>
        /// <param name="pdctAppSettings">
        /// The SortedDictionary of objects, which receive the values, keyed by
        /// the string node names, to which the node is appended.
        /// </param>
        /// <param name="pntChildNodeCount">
        /// The count of child nodes goes into the trace message.
        /// </param>
        /// <param name="pintJ">
        /// The index of the FOR loop from which this routine is called goes
        /// into the trace message.
        /// </param>
        /// <param name="pxmlAppSettingsKey">
        /// This XmlElement node contains the name and value to append to the
        /// <paramref name="pdctAppSettings"/>.
        /// </param>
        /// <param name="pshsKeyNames">
        /// Optional HashSet of strings, each of which is a key to return in the
        /// list. When this argument is omitted or null, all keys are returned.
        /// </param>
        private static void AddElementToDoctionary (
            SortedDictionary<string , object> pdctAppSettings ,
            int pntChildNodeCount ,
            int pintJ ,
            XmlElement pxmlAppSettingsKey ,
            HashSet<string> pshsKeyNames )
        {
            if ( pxmlAppSettingsKey.Attributes.Count == MagicNumbers.PLUS_TWO )
            {
                XmlAttribute xmlNameAttribute = pxmlAppSettingsKey.Attributes [ ArrayInfo.ARRAY_FIRST_ELEMENT ];
                XmlAttribute xmlValueAttribute = pxmlAppSettingsKey.Attributes [ ArrayInfo.ARRAY_SECOND_ELEMENT ];

                if ( xmlNameAttribute.Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_KEY && xmlValueAttribute.Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_VALUE )
                {
                    if ( IncludeKeyInList ( xmlNameAttribute.Value, pshsKeyNames ) )
                    {
                        pdctAppSettings.Add (
                            xmlNameAttribute.Value ,                                // string key
                            xmlValueAttribute.Value );                              // object value
                        TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                            string.Format (
                                Properties.Resources.TRACEMSG_4 ,                   // Format Control String
                                ArrayInfo.OrdinalFromIndex ( pintJ ) ,              // Format Item 0: Node {0}
                                pntChildNodeCount ,                                 // Format Item 1: of {1}
                                pxmlAppSettingsKey.Name ,                           // Format Item 2: Name = {2}
                                pxmlAppSettingsKey.ChildNodes.Count ,               // Format Item 3: Children = {3}
                                xmlNameAttribute.Value ,                            // Format Item 4: Name = {4}
                                xmlValueAttribute.Value ) );                        // Format Item 5: Value = {5}
                    }
                }   // TRUE (anticipated outcome) block, if ( xmlNameAttribute.Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_KEY && xmlValueAttribute.Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_VALUE )
                else
                {
                    TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                        string.Format (
                            Properties.Resources.TRACEMSG_5 ,                   // Format Control String
                            ArrayInfo.OrdinalFromIndex ( pintJ ) ,              // Format Item 0: Node {0}
                            pntChildNodeCount ,                                 // Format Item 1: of {1}
                            pxmlAppSettingsKey.Name ,                           // Format Item 2: Name = {2}
                            pxmlAppSettingsKey.ChildNodes.Count ,               // Format Item 3: Children = {3}
                            xmlNameAttribute.Name ,                             // Format Item 4: Attribute0Name = {4}
                            xmlValueAttribute.Name ) );                         // Format Item 5: Attribute1Name = {5}
                }   // FALSE (unanticipated outcome) block, if ( xmlNameAttribute.Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_KEY && xmlValueAttribute.Name == Properties.Resources.XML_ATTRIBUTE_NAME_IS_VALUE )
            }   // TRUE (anticipated outcome) block, if ( xmlAppSettingsKey.Attributes.Count == MagicNumbers.PLUS_TWO )
            else
            {
                TraceLogger.WriteWithBothTimesLabeledLocalFirst (
                    string.Format (
                        Properties.Resources.TRACEMSG_6 ,                       // Format Control String
                        ArrayInfo.OrdinalFromIndex ( pintJ ) ,                  // Format Item 0: Node {0}
                        pntChildNodeCount ,                                     // Format Item 1: of {1}
                        pxmlAppSettingsKey.Name ,                               // Format Item 2: Name = {2}
                        pxmlAppSettingsKey.ChildNodes.Count ,                   // Format Item 3: Children = {3}
                        pxmlAppSettingsKey.Attributes.Count ,                   // Format Item 4: Actual attribute count = {4}
                        MagicNumbers.PLUS_TWO ) );                              // Format Item 5: Expected attribute count = {5}
            }   // FALSE (unanticipated outcome) block, if ( xmlAppSettingsKey.Attributes.Count == MagicNumbers.PLUS_TWO )
        }   // private static void AddElementToDoctionary


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

                if ( xmlChildOfRootNode.Name == Properties.Resources.APP_SETTINGS_NAME_CONFIGURATION )
                {
                    return xmlChildOfRootNode.GetEnumerator ( );
                }   // if ( xmlChildOfRootNode.Name == Properties.Resources.APP_SETTINGS_NAME_CONFIGURATION )
            }   // while ( enumerator1.MoveNext ( ) )

            return null;
        }   // private static IEnumerator GetXmlNodeElementEnumerator


        /// <summary>
        /// Pass in the HashSet that was supplied to the calling routine if
        /// there is one. When <paramref name="pshsKeyNames"/> is either empty
        /// or the null reference, all keys are returned.
        /// </summary>
        /// <param name="pstrKeyName">
        /// The string representation of the current key to match against the
        /// list in <paramref name="pshsKeyNames"/> if present.
        /// </param>
        /// <param name="pshsKeyNames">
        /// Pass in an optional HashSet of keys to include in the report. When
        /// this argument is an empty HashSet or a null reference, the method
        /// returns True, causing all keys to be returned.
        /// </param>
        /// <returns>
        /// When <paramref name="pshsKeyNames"/> is empty or a null reference,
        /// return TRUE, causing all keys to be returned. Otherwise, return TRUE
        /// only when <paramref name="pstrKeyName"/> is valid (neither a null
        /// reference, nor the empty string) and it is in the
        /// <paramref name="pshsKeyNames"/> list.
        /// </returns>
        private static bool IncludeKeyInList (
            string pstrKeyName ,
            HashSet<string> pshsKeyNames = null )
        {
            if ( string.IsNullOrEmpty ( pstrKeyName ) )
            {
                return true;
            }   // TRUE (The key name is empty.) block, if ( string.IsNullOrEmpty ( pstrKeyName ) )
            else
            {
                if ( pshsKeyNames == null )
                {
                    return true;
                }   // TRUE (There is no key list.) block, if ( pshsKeyNames == null )
                else
                {
                    if ( pshsKeyNames.Count == ListInfo.LIST_IS_EMPTY )
                    {
                        return true;
                    }   // TRUE (The key list is empty.) block, if ( pshsKeyNames.Count == ListInfo.LIST_IS_EMPTY )
                    else
                    {
                        return pshsKeyNames.Contains ( pstrKeyName );
                    }   // FALSE (The key list contains values.) block, if ( pshsKeyNames.Count == ListInfo.LIST_IS_EMPTY )
                }   // FALSE (The caller supplied a key list.) block, if ( pshsKeyNames == null )
            }   // FALSE (The key name contains text.) block, if ( string.IsNullOrEmpty ( pstrKeyName ) )
        }   // private static bool IncludeKeyInList
    }   // public static class ConfigFileReaders
}   // partial WizardWrx.WebApplicationAids