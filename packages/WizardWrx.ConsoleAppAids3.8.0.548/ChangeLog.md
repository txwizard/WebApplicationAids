﻿# WizardWrx.ConsoleAppAids3 Change Log

This file is a running history of fixes and improvements from version 7.0
onwards. Changes are documented for the newest version first. Within each
version, classes are listed alphabetically.

# Version 8.0.533 Updated 07/06/2021

Correct the documentation URL shown in the ReadMe file, which was missing a
required trailing backslash.

The repository is otherwise unchanged.

# Version 8.0.533 Released 05/02/2021 03:05:40

Build against the most recent WizardWrx .NET API library constellation.

Going forward, this library will be periodically refreshed by building it
against newer versions of the WizardWrx .NET API. If ChangeLog.md (this file) is
unchanged, it is safe to assume that the only change is a refresh of its
dependencies.

# Version 8.0, Released 03/05/2021 18:55:45

This update of the `WizardWrx.ConsoleAppAids3` library defines a new method,
`LoadBasicErrorMessages`, that loads the appropriate string described by the
following table.

|Message Text                      |Common Resource| Common Code |Code Value|
|----------------------------------|---------------|-------------|----------|
|The task completed successfully.  |ERRMSG_SUCCESS |ERROR_SUCCESS|0x00000000|
|A run-time exception has occurred.|ERRMSG_RUNTIME |ERROR_RUNTIME|0x00000001|

Additional error messages can be loaded by passing in an optional array of
strings, which must follow the order of exit code assignments, which do double
duty as an index into the arry of messages. If you need only those two, both of
which are required to enable other parts of the library to work correctly, pass
in a NULL.

# Version 7.2

This update of the `WizardWrx.ConsoleAppAids3` library resolves an issue that
allowed a calling routine to instantiate an uninitialized, utterly useless
instance of the `ConsoleAppStateManager` object. In so doing, the bug breaks a
fundamental tenet of the Singleton design pattern which it is intended to
implement. Although the compiler allowed the instance to be created, it would
fail with a null reference exception on first use.

Additionally, this release implements Semantic Version Numbering and the related
Deterministic compiler switch.

This build also incorporates the current stable versions of the `WizardWrx .NET
API` asseblies, which finally fully implement Semantic Version Numbering as of a
few days ago.

# Version 7.1

Reset the console row indicator when `ScrollUp` is called on a
`FixedConsoleWriter` object, so that the object need not be re-created when you
want to reuse it further down screen. I am calling this a point release,
although it is only a bug fix.

This is the first release to be accompanied by a NuGet package.

# Version 7.0

Following is a summary of changes made in version 7.0, released Monday, 26 November 2018.

## Class: WizardWrx.ConsoleAppStateManager

Mark `GetTheSingleInstance` as a New (override) method, eliminating a compiler
warning.

## Class: DisplayAids

Replace the `WizardWrx.DllServices2.dll` monolith with the constellation of DLLs
that replaced it, which also requires upgrading the target framework version
from 2.0 to 3.5 Client Profile.

## Class: FixedConsoleWriter

Replace the `WizardWrx.DllServices2.dll` monolith with the constellation of DLLs
that replaced it, which also requires upgrading the target framework version
from 2.0 to 3.5 Client Profile.

This library supersedes [WizardWrx.ConsoleAppAids2](https://github.com/txwizard/ConsoleAppAids2),
which is hereby deprected. I stopped using its predecessor well over a year ago. Since this library is
essentially the same code base, the revisions made prior to the upgrade remain as headnotes in
each source file.