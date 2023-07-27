/*
    ============================================================================

    Namespace Name:     WebApplicationAids_TestStand

    Module Name:        Program.cs

    Class Name:         Program

    Synopsis:           This command line utility exercises the routines in the
                        WizardWrx.WebApplicationAids class.

    Remarks:            This class module implements the Program class, which is
                        composed of the static void Main method, which is
                        functionally equivalent to the main() routine of a
                        standard C program.

    Created:            Friday, 17 December 2021

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

    Date       Version By  Synopsis
    ---------- ------- --- -----------------------------------------------------
    2021/12/17 1.0.0   DAG This is the first version.

    2021/12/19 1.0.3   DAG Extend the unit tests to cover an app.config file and
                           a two-property connection string that contains just
                           the bare minimum, name and connectionString
                           properties.

    2022/04/03 1.0.36  DAG Extend the unit test to cover a new method,
                           GetAppSettingsKeysFromAnyConfig.

    2023/07/26 1.1.42  DAG Point to the configuration files that I copied into
                           the /NOTES directory hidden in the solution tree.
    ============================================================================
*/


using System;                                                                   // Apart from it being the mother of all namespaces, the Console class lives in this mamespace.
using System.Collections.Generic;

using WizardWrx;                                                                // This is the root namespace of the WizardWrx .NET API, which includes most of the symbolic constants.

using WizardWrx.ConsoleAppAids3;												// This library provides the startup and shutdown messages and keeps up with the application status code.
using WizardWrx.DLLConfigurationManager;                                        // This library defines the BaseStateManager and ApplicationExceptionLogger classes.

using WizardWrx.WebApplicationAids;                                             // This is the library under test.


namespace WebApplicationAids_TestStand
{
    class Program
    {
        static string [ ] s_astrErrorMessages =
        {
        };  // static string [ ] s_astrErrorMessages

        static ConsoleAppStateManager s_theApp;


        static readonly string [ ] s_astrKeyFilterList = new string [ ]
        {
            @"AWSFileBucketName" ,
            @"AWSSupportBucketName" ,
            @"DownloadDirectory" ,
            @"MessageGearsAWSAccountId" ,
            @"MessageGearsAWSCanonicalId" ,
            @"MessageGearsEndPoint" ,
            @"MGAWSAccountKey" ,
            @"MGAWSSecretKey" ,
            @"MyAWSAccountKey" ,
            @"MyAWSBucket" ,
            @"MyAWSEventQueueUrl" ,
            @"MyAWSSecretKey" ,
            @"MyMessageGearsAccountId" ,
            @"MyMessageGearsApiKey" ,
            @"NumberOfEventPollerThreads" ,
            @"SQSMaxBatchSize" ,
            @"SQSMaxErrorRetry" ,
            @"SQSVisibilityTimeoutSecs"
        };  // static readonly string [ ] s_astrKeyFilterList

        const string SAMPLE_APP_CONFIG_FILENAME = @"D:\Source_Code\Visual_Studio\Projects\SalesTalk\Source\salestalk\LeadLife\LeadLife.Operations\User_Activity_Report\App.config";
        const string SAMPLE_WEB_CONFIG_FILENAME = @"D:\Source_Code\Visual_Studio\Projects\WizardWrx_Libs\WebApplicationAids\NOTES\SalesAcceleration_Web_20230326_172832.config";

        static void Main ( string [ ] pastrArgs )
        {
            //  ----------------------------------------------------------------
            //  This must happen first thing to fully simulate the conditions
            //  that arose in the code that exposed this bug.
            //
            //  See the comment written just above the commented-out method.
            //  ----------------------------------------------------------------

            s_theApp = ConsoleAppStateManager.GetTheSingleInstance ( );
            s_theApp.DisplayBOJMessage ( );

            //CmdLneArgsBasic cmdArgs = new CmdLneArgsBasic (
            //    s_achrValidSwitches ,
            //    s_astrNamedArgs ,
            //    CmdLneArgsBasic.ArgMatching.CaseInsensitive );
            //cmdArgs.AllowEmptyStringAsDefault = CmdLneArgsBasic.BLANK_AS_DEFAULT_ALLOWED;

            //  ----------------------------------------------------------------
            //  The default value of the AppSubsystem property is GUI, which
            //  disables output to the console. Since ReportException returns
            //  the message that would have been written, you still have the
            //  option of displaying or discarding it. If EventLoggingState is
            //  set to Enabled, the message is written into the Application
            //  Event log, where it is preserved until the event log record is
            //  purged by the aging rules or some other method.
            //  ----------------------------------------------------------------

            s_theApp.BaseStateManager.AppExceptionLogger.OptionFlags = s_theApp.BaseStateManager.AppExceptionLogger.OptionFlags
                                                                       | ExceptionLogger.OutputOptions.Stack
                                                                       | ExceptionLogger.OutputOptions.EventLog
                                                                       | ExceptionLogger.OutputOptions.StandardError;

            //  ----------------------------------------------------------------
            //  In the beginning, each program had its own complete table of
            //  error messages. Though the first two status codes and the
            //  corresponding messages were always reserved, the texts didn't
            //  become standardized for about another year, after which life
            //  got between me and creating an improved method that loads these
            //  two messages, then appends application-specific messages.
            //
            //  LoadErrorMessageTable is deprecated in favor of a new method,
            //  LoadBasicErrorMessages, that takes an optional short list of the
            //  other messages.
            //  ----------------------------------------------------------------

            s_theApp.LoadBasicErrorMessages ( s_astrErrorMessages );

            string strAbsoluteConfigFileName = SAMPLE_WEB_CONFIG_FILENAME;
            string strConnectionStringName = @"ApplicationServices";
            System.Configuration.ConnectionStringSettings connectionStringSettings = ConfigFileReaders.GetConnectionStringFromWebConfig ( strAbsoluteConfigFileName , strConnectionStringName );

            Console.WriteLine ( @"{0}Reading Connection String from Web Config File{0}" , Environment.NewLine );
            Console.WriteLine ( @"    Absolute Name of Web Config File = {0}" , strAbsoluteConfigFileName );
            Console.WriteLine ( @"    Name of Connection String        = {0}" , strConnectionStringName );
            Console.WriteLine ( @"    Connection String Name           = {0}" , connectionStringSettings.Name );
            Console.WriteLine ( @"    Connection String Value          = {0}" , connectionStringSettings.ConnectionString );
            Console.WriteLine ( @"    Connection String Provider Name  = {0}" , connectionStringSettings.ProviderName );
            Console.WriteLine ( @"{0}Connection String successfully read from Web Config File.{0}" , Environment.NewLine );

            strAbsoluteConfigFileName = SAMPLE_APP_CONFIG_FILENAME;
            strConnectionStringName = @"FreeTrialDashboard.Properties.Settings.ABACUS_Connection";
            connectionStringSettings = ConfigFileReaders.GetConnectionStringFromWebConfig ( strAbsoluteConfigFileName , strConnectionStringName );

            Console.WriteLine ( @"Reading Connection String from Web Config File{0}" , Environment.NewLine );
            Console.WriteLine ( @"    Absolute Name of Web Config File = {0}" , strAbsoluteConfigFileName );
            Console.WriteLine ( @"    Name of Connection String        = {0}" , strConnectionStringName );
            Console.WriteLine ( @"    Connection String Name           = {0}" , connectionStringSettings.Name );
            Console.WriteLine ( @"    Connection String Value          = {0}" , connectionStringSettings.ConnectionString );
            Console.WriteLine ( @"    Connection String Provider Name  = {0}" , connectionStringSettings.ProviderName );
            Console.WriteLine ( @"{0}Connection String successfully read from Web Config File.{0}" , Environment.NewLine );

            ExerciseGetAppSettingsKeysFromAnyConfig (
                new string [ ]
                {
                    SAMPLE_APP_CONFIG_FILENAME ,
                    SAMPLE_WEB_CONFIG_FILENAME
                } );

            //  ----------------------------------------------------------------
            //  Render the final report, clean up, and shut down.
            //  ----------------------------------------------------------------

            s_theApp.NormalExit ( ConsoleAppStateManager.NormalExitAction.WaitForOperator );
		}   //  static void Main


        /// <summary>
        /// Exercise the new GetAppSettingsKeysFromAnyConfig method.
        /// </summary>
        /// <param name="pastrAppConfigFileNames">
        /// Array of strings, each of which represents the name of a configuration file
        /// </param>
        private static void ExerciseGetAppSettingsKeysFromAnyConfig ( string [ ] pastrAppConfigFileNames )
        {
            Console.WriteLine ( $"{Environment.NewLine}Static Method GetAppSettingsKeysFromAnyConfig Exercises:{Environment.NewLine}" );

            int intTestCount = pastrAppConfigFileNames.Length;

            for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                      intJ < intTestCount ;
                      intJ++ )
            {
                Console.WriteLine ( $"    Begin Test {ArrayInfo.OrdinalFromIndex ( intJ )} of {intTestCount}:{Environment.NewLine}" );
                Console.WriteLine ( $"        Absolute Configuration File Name = {pastrAppConfigFileNames [ intJ ]}" );

                try
                {
                    SortedDictionary<string , object> dctAppSettings1 = ConfigFileReaders.GetAppSettingsKeysFromAnyConfig ( pastrAppConfigFileNames [ intJ ] );
                    EnumerateAppSettings ( dctAppSettings1 );

                    if ( pastrAppConfigFileNames [ intJ ] == SAMPLE_WEB_CONFIG_FILENAME )
                    {
                        Console.WriteLine ( $"        Repeat the previous test using static list of selected keys containing {s_astrKeyFilterList.Length} key names" );
                        SortedDictionary<string , object> dctAppSettings2 = ConfigFileReaders.GetAppSettingsKeysFromAnyConfig (
                            pastrAppConfigFileNames [ intJ ] ,                  // string    pstrAbsoluteConfigFileName
                            s_astrKeyFilterList );                              // string [] pastrKeyNames              = null
                        EnumerateAppSettings ( dctAppSettings2 );
                    }   // if ( pastrAppConfigFileNames [ intJ ] == SAMPLE_WEB_CONFIG_FILENAME )
                }
                catch ( Exception exAllKinds )
                {
                    s_theApp.BaseStateManager.AppExceptionLogger.ReportException ( exAllKinds );
                }
                finally
                {
                    Console.WriteLine ( $"    Test {ArrayInfo.OrdinalFromIndex ( intJ )} of {intTestCount} completed{Environment.NewLine}" );
                }
            }   // for ( int intJ = WizardWrx.ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < intTestCount ; intJ++ )

            Console.WriteLine ( $"Static Method GetAppSettingsKeysFromAnyConfig Exercised:{Environment.NewLine}" );
        }


        /// <summary>
        /// List the values returned by the builder.
        /// </summary>
        /// <param name="pdctAppSettings1">
        /// Pass in a reference to the SortedDictionary returned by
        /// GetAppSettingsKeysFromAnyConfig.
        /// </param>
        private static void EnumerateAppSettings ( SortedDictionary<string , object> pdctAppSettings1 )
        {
            Console.WriteLine ( $"        Count of Matching Settings = {pdctAppSettings1.Count}{Environment.NewLine}" );
            int intOrdinal = ListInfo.LIST_IS_EMPTY;

            foreach ( KeyValuePair<string , object> kvpFlat in pdctAppSettings1 )
            {
                if ( kvpFlat.Value is string )
                {
                    string strKeyName = kvpFlat.Key;
                    string strKeyValue = kvpFlat.Value.ToString ( );
                    string strLineTerminator = LineTerminator ( intOrdinal , pdctAppSettings1.Count );
                    Console.WriteLine (
                        @"        Setting {0,2}: Name = {1}, Value = {2}{3}" ,                 // Format Control String
                        ++intOrdinal ,                                                          // Format Item 0: Setting {0,-2}
                        strKeyName ,                                                            // Format Item 1: Name = {1}
                        strKeyValue ,                                                           // Format Item 2: Value = {2}
                        strLineTerminator );                                                    // Format Item 3: {3}
                }   // TRUE (The structure of the application settings section is flat, as it usually is in a web.config file.) block, if ( kvp.Value is string )
                else
                {
                    if ( kvpFlat.Value is SortedDictionary<string , object> )
                    {
                        Console.WriteLine (
                            @"    Configuration section Name = {0}, keys are as follows:{1}" ,  // Format Control String
                            kvpFlat.Key ,                                                       // Format Item 0: section Name = {0}
                            Environment.NewLine );                                              // Format Item 1: keys are as follows:(1)
                        SortedDictionary<string , object> dctConfigSection = ( SortedDictionary<string , object> ) kvpFlat.Value;
                        int intSectionOrdinal = ListInfo.LIST_IS_EMPTY;

                        foreach ( KeyValuePair<string , object> kvpConfigSection in dctConfigSection )
                        {
                            string strKeyName = kvpConfigSection.Key;
                            string strKeyValue = kvpConfigSection.Value.ToString ( );
                            string strLineTerminator = LineTerminator ( intSectionOrdinal , dctConfigSection.Count );
                            Console.WriteLine (
                                @"            Setting {0,2}: Name = {1}, Value = {2}{3}" ,     // Format Control String
                                ++intSectionOrdinal ,                                           // Format Item 0: Setting {0,-2}
                                strKeyName ,                                                    // Format Item 1: Name = {1}
                                strKeyValue ,                                                   // Format Item 2: Value = {2}
                                strLineTerminator );                                            // Format Item 3: {3}
                        }   // foreach ( KeyValuePair<string , object> kvpConfigSection in dctConfigSection )
                    }   // TRUE (anticipated outcome) block, if ( kvp.Value is SortedDictionary<string , object> )
                    else
                    {
                        Console.WriteLine (
                            @"    Configuration section {0} has an unexpected type of {1}" ,    // Format Control String
                            kvpFlat.Key ,                                                           // Format Item 0: Configuration section {0}
                            kvpFlat.Value.GetType ( ).FullName );                                   // Format Item 1: has an unexpected type of {1}
                    }   // FALSE (unanticipated outcome) block, if ( kvp.Value is SortedDictionary<string , object> )
                }   // FALSE (The structure of the application settings section is hierarchical, as if often the case in a app.config file.) block, if ( kvp.Value is string )
            }   // foreach ( KeyValuePair<string , object> kvp in dctAppSettings1 )
        }   // private static void EnumerateAppSettings


        private static string LineTerminator ( int intK , int length )
        {
            return Logic.IsLastForIterationLT ( intK , length ) ? Environment.NewLine : SpecialStrings.EMPTY_STRING;
        }   // private static string LineTerminator
    }   // class Program
}   // namespace WebApplicationAids_TestStand