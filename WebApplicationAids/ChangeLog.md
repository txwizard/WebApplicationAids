# WizardWrxWebApplicationAids Change Log

This file is a cumulative history of releases arranged from newest to oldest.

This update resolves the issue that dependencies aren't advertised. Henceforth,
release will coincide with bug fixes and feature updates.

## 2023-07-26, Version 1.1.40

This update refreshes the dependencies, all of which now target Microsoft .NET
Framework version 4.8.

The code is otherwise unchanged, since the library already targets that version.

## 2022-05-21, Version 1.0.31

To compensate for an apparent change in the behavior of the NuGet Package Manager
implementation in Visual Studio 2019, all NuGet in the WizardWrx .NET API
constellation of libraries, along with WizardWrx NuGet packages that depend upon
them, are being re-released with dependencies named explicitly in the generated
.nuspec file that goes into the package. This is accomplished by adding the
IncludeReferencedProjects to the NuGet Pack command line. Otherwise, the program
source code is unchanged, nor are the binaries.

## 2022-04-04, Version 1.0.29

This version incorporates a new static method, `GetAppSettingsKeysFromAnyConfig`,
that returns a `SortedDictionary` that contains an alphabetical list of the
sections and keys in a configuration file. When the application configuration is
divided into sections, each is preserved and processed independently by returning
a nested `SortedDictionary` for each section. In such circumstances, the top-level
`SortedDictionary` is an alphabetical index of the named sections.

Unit test assembly `WebApplicationAids_TestStand` does double duty as a
demonstration of how to use the routine to read the application settings section
of any application or web configuration file and how nested application settings
sections are represented.

The demonstrations consist of a flat `web.config` file and the application
configuration file of a console application that is divided into two named
sections. A further demonstration involving the flat `web.config` file shows how
the method can selectively list keys. The example selects keys from the AWS S3
bucket keys and other keys that control the interface to an application that can
send electronic mail to one or more email addresses, high volume mail merge for
a Web application.

## 2021-12-19, Version 1.0.8

The NuGet package specification designates the target framework 4.8 explicitly.

## 2021-12-19, Version 1.0.7

This release resolves a bug that caused the initial release to fail when the
`providerName` attribute is absent.

## 2021-12-17, Version 1.0.1

This is the first release, which is incompletely tested, but sufficiently so to
meet the immediate objective that prompted its creation.