/*
    ============================================================================

    Namespace Name:     WizardWrx.WebApplicationAids

    Class Name:         ConfigFileReaders

    Source Module Name: ConfigFileReaders.cs

    Synopsis:           See triple-slash XML comment.

    Renarks:            The initial implementation exposes a static method that
                        was enables any assembly to query a we.config file
                        stored at a known location in a filesystem for a
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
    ============================================================================
*/

using System.Configuration;
using System.Xml;


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
			XmlDocument xml = new XmlDocument ( );
			xml.Load ( pstrAbsoluteConfigFileName );
			XmlNodeList xmlRootElement = xml.GetElementsByTagName ( @"configuration" );

			System.Collections.IEnumerator inodeEnumerator1 = xmlRootElement.GetEnumerator ( );

			while ( inodeEnumerator1.MoveNext ( ) )
			{   // Since the generic enumerator returns an equally generic object, the type cast must be made explicitly.
				XmlElement xmlChildOfRootNode = ( XmlElement ) inodeEnumerator1.Current;

				if ( xmlChildOfRootNode.Name == @"configuration" )
				{
					System.Collections.IEnumerator inodeEnumerator2 = xmlChildOfRootNode.GetEnumerator ( );

					while ( inodeEnumerator2.MoveNext ( ) )
					{
						XmlElement xmlGrandChildOfRootNode = ( XmlElement ) inodeEnumerator2.Current;

						if ( xmlGrandChildOfRootNode.LocalName == @"connectionStrings" )
						{
							System.Collections.IEnumerator inodeEnumerator3 = xmlGrandChildOfRootNode.GetEnumerator ( );

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
				}   // if ( xmlChildOfRootNode.Name == @"configuration" )
			}   // while ( inodeEnumerator1.MoveNext ( ) )

			return null;    // This should throw an exception.
		}   // public static string GetConnectionStringFromWebConfig
	}   // public static class ConfigFileReaders
}   // partial WizardWrx.WebApplicationAids