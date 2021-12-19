# WizardWrx.WebApplicationAids ReadMe

Get connection string by name from any configuration file, either web.config or
app.config, for any application.

## Purpose of This Library

The purpose of this class library is to expedite development of applications
that target any version of the Microsoft .NET Framework, from version 4.8 up, by
making connection strings, and eventually other settings, accessible to any DLL
or executable assembly, regardless of the platform targeted by the configuration
file. This need arose once again when a requirement arose for a satellite 
assembly to be able to obtain connection strings from the web.config file of the
application that will consume it.

By extension, this library makes web.config files accessible to desktop
applications and vice versa.

Though the runtime will undoubtedly bluster about potential compatibility issues
due to the target being the full-featured .NET Framework, you __should__ be able
to use this library in .NET Core projects.

## Versioning

This library and its unit test assembly use SemVer versioning, and I endeavor to
avoid breaking changes, and succeed almost every time. In well over a decade of
development, there have been only one or two __unavoidable__ breaking changes.

## Using This Library

Since there are no name collisions, you may safely set references this library
and any or all of the other WizardWrx libraries, and you _should_ be safe with
the Base Class Library namespaces, too.

Detailed API documentation is at <https://txwizard.github.io/WebApplicationAids/>.

For those who just want to use them, debug and release builds of the libraries
and the unit test program are available as archives off the project root
directory.

*	`WizardWrx_WebApplicationAids_Binaries_Debug.7z` is the debug build of the binaries.

*	`WizardWrx_WebApplicationAids_Binaries_Release.7z` is the release build of the binaries.

There is a DLL, PDB, and XML file for each library. To derive maximum benefit,
including support for the Visual Studio managed code debugger and IntelliSense
in the text editor, take all three.

__Important__: This library has two dependencies, both licensed under the same
BSD license, published on GitHub, and available separately as NuGet packages.
This distribution includes package configuration files so that your build system
can update the dependencies.

## Change Log

See `ChangeLog.md` for a cumulative history of changes, listed from newest to
oldest.

## Road Map

For the most part, the constellation of class libraries of which this is the 
newest member evolves to acommodate needs as they arise in my development work.
Nevertheless, I have a road map, and it has a few well-marked stops, of which
the most significant is extending it to be able to read the application settings
from either an app.config or a web.config.

## Contributing

Though I created this library to meet my individual development needs, I have
put a good bit of thought and care into its design. Moreover, since I will not
live forever, and I want these libraries to outlive me, I would be honored to
add contributions from others to it. My expectations are few, simple, easy to
meet, and intended to maintain the consistency of the code base and its API.

1.	__Naming Conventions__: I use Hungarian notation. Some claim that it has
outlived its usefulness. I think it remains useful because it encodes data
about the objects to which the names are applied that follows them wherever they
go, and convey it without help from IntelliSense.

2.	__Coding Style__: I have my editor set to leave spaces around every token.
This spacing has helped me quicly spot misplaced puncuation, such as the right
bracket that closes an array initializer that is in the wrong place, and it
makes mathematical expressions easier to read and mentally parse.

3.	__Comments__: I comment liberally and very deliberately. Of particular
importance are the comments that I append to the bracket that closes a block. It
does either or both of two things: link it to the opening statement, and
document which of two paths an __if__ statement is expected to follow most of
the time. When blocks get nested two, three, or four deep, they really earn
their keep.

4.	__Negative Conditions__: I do my best to avoid them, because they almost
always cause confusion. Astute observers will notice that they occur from time
to time, because there are _a few cases_ where they happen to be less confusing.

5.	__Array Initializers__: Arrays that have more than a very few initializers,
or that are initialized to long strings, are laid out as lists, with line
comments, if necessary, that describe the intent of each item.

6.	__Format Item Lists__: Lists of items that are paired with format items in
calls to `string.Format`, `Console.WriteLine`, and their relatives, are laid out
as arrays, even if there are too few to warrant one, and the comments show the
corresponding format item in context. This helps ensure that the items are
listed in the correct order, and that all format items are covered.

7.	__Symbolic Constants__: I use symbolic constants to document what a literal
value means in the context in which it appears, and to disambiguate tokens that
are easy to confuse, suzh as `1` and `l` (lower case L), `0` and `o` (lower case O),
literal spaces (1 and 2 spaces are common), underscores, the number `-1`, and so
forth. Literals that are widely applicable are defined in a set of classes that
comprise the majority of the root `WizardWrx` namespace. All of the aforementioned
constants are defined in `WizardWrx.Common.dll`, the sole direct dependency of this
library.

8.	__Argument Lists__: I treat argument lists as arrays, and often comment each
argument with the name of the paramter that it represents. These lists help
guarantee that a long list of positional arguments is specified in the correct
order, especially when several are of the same type (e. g., two or more integer
arguments).

9.	__Triple-slash Comments__: These go on _everything_, even private members and
methods, so that everything supports IntelliSense, and it's easy to apply a tool
(I use DocFX.) to generate reference documentation.

With respect to the above items, you can expect me to be a nazi, though I shall
endeavor to give contributors a fair hearing when they have a good case.
Otherwise, please exercise your imagination and submit your pull requests. I'll
take care of rolling the contributions into the appropriate packages, and
contributors will get prominent credit on the package page in the official public
repository. If you skim the headnotes of the code, you'll see that I take great
pains to give others credit when I icorporate their code into my projects, even
to the point of disclaiming copyright or leaving their copyright notice intact.
Along the same lines, the comments are liberally sprinkled with references to
articles and Stack Overflow discussions that contributed to the work.