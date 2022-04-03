/*
    ============================================================================

    Namespace Name:     WebApplicationAids_TestStand

    Module Name:        Program.cs

    Class Name:         Program

    Synopsis:           This command line utility exercises the routines in the
                        WizardWrx.ConsoleAppAids3 class.

    Remarks:            This class module implements the Program class, which is
                        composed exclusively of the static void Main method,
                        which is functionally equivalent to the main() routine
                        of a standard C program.

                        The initial version number is set to 3.2, to correspond
                        with the version of the library that was current when it
                        came into being.

    Created:            Friday, 17 December 2021

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

    Date       Version By  Synopsis
    ---------- ------- --- -----------------------------------------------------
    2021/12/17 1.0.0   DAG This is the first version.

    2021/12/19 1.0.3   DAG Extend the unit tests to cover an app.config file and
                           a two-property connection string that contains just
                           the bare minimum, name and connectionString
                           properties.

    2022/03/27 1.0.10  DAG Extend the unit test to cover a new method,
                           GetAppSettingsKeysFromAnyConfig.
    ============================================================================
*/


using System;                                                                   // Apart from it being the mother of all namespaces, the Console class lives in this mamespace.

using WizardWrx;

using WizardWrx.ConsoleAppAids3;												// This is the library under test.
using WizardWrx.DLLConfigurationManager;
using WizardWrx.WebApplicationAids;


namespace WebApplicationAids_TestStand
{
    class Program
    {
        static string [ ] s_astrErrorMessages =
        {
        };  // static string [ ] s_astrErrorMessages

        static ConsoleAppStateManager s_theApp;


        static void Main ( string [ ] pastrArgs )
        {
            const string SAMPLE_APP_CONFIG_FILENAME = @"D:\Source_Code\Visual_Studio\Projects\SalesTalk\Source\salestalk\LeadLife\LeadLife.Operations\User_Activity_Report\App.config";
            const string SAMPLE_WEB_CONFIG_FILENAME = @"D:\SalesTalk\_Say2Sell\Free_Trial\Web_SalesAcceleration_20211110_112338.config";

            //  ----------------------------------------------------------------
            //  This must happen first thing to fully simulate the conditions
            //  that arose in the code that exposed this bug.
            //
            //  See the comment written just above the commented-out method.
            //  ----------------------------------------------------------------

            s_theApp = ConsoleAppStateManager.GetTheSingleInstance ( );
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
                Console.WriteLine ( $"    Begin Test {ArrayInfo.OrdinalFromIndex ( intJ )} of {intTestCount}:" );
                Console.WriteLine ( $"        Absolute Configuration File Name = {pastrAppConfigFileNames [ intJ ]}" );

                try
                {
                    System.Collections.Specialized.NameValueCollection nvcAppSettings1 = ConfigFileReaders.GetAppSettingsKeysFromAnyConfig ( pastrAppConfigFileNames [ intJ ] );
                    Console.WriteLine ( $"        Count of Matching Settings = {nvcAppSettings1.Count}{Environment.NewLine}" );

                    for ( int intK = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                              intK < nvcAppSettings1.Count ;
                              intK++ )
                    {
                        int intOrdinal = ArrayInfo.OrdinalFromIndex ( intK );
                        string strKeyName = nvcAppSettings1.AllKeys [ intJ ];
                        string strKeyValue = nvcAppSettings1 [ nvcAppSettings1.AllKeys [ intJ ] ];
                        string strLineTerminator = LineTerminator ( intK , nvcAppSettings1.AllKeys.Length );
                        Console.WriteLine ( 
                            @"        Setting {0}: Name = {1}, Value = {2}{3}" ,// Format Control String
                            intOrdinal ,                                        // Format Item 0: Setting {0}
                            strKeyName ,                                        // Format Item 1: Name = {1}
                            strKeyValue ,                                       // Format Item 2: Value = {2}
                            strLineTerminator );                                // Format Item 3: {3}
                    }   // for ( int intK = ArrayInfo.ARRAY_FIRST_ELEMENT ; intK < nvcAppSettings1.Count ; intK++ )
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
        }   /// private static void ExerciseGetAppSettingsKeysFromAnyConfig

        private static string LineTerminator ( int intK , int length )
        {
            return Logic.IsLastForIterationLT ( intK , length ) ? Environment.NewLine : SpecialStrings.EMPTY_STRING;
        }   // private static string LineTerminator
    }   // class Program
}   // namespace WebApplicationAids_TestStand