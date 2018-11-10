# Vision
The Helper project provides common utilities of limited scope that several Maseya projects rely on.

## Philosphy
The purpose of the Helper project is create simple classes, interfaces, and data structures that are seen frequently in the Maseya organization. Some of the features included are:
- Floating point ARGB color data structure with several blend mode functions and instantiation by other color spaces such as CMY and from hue, saturation, and luminosity.
- An invertible dictionary class that maintains a one-to-one correspondence between its keys and values.
- Basic static math functions not found in system.Math class.
- String formatting functions that are automatically culture aware.
- Byte array suffix tree.
- Functions that return exception messages for common errors such as out or range exceptions. These messages are culture-aware.
- Pixel data types whose backing stores are common pixel formats. This allows for casting from bitmap pointers to the desired pixel type for easier drawing.


## Open Source
This project is being built with the goal of being completely open source, and enforcing all programs that use it be open source too. Open source allows anyone to contribute, fix, and improve the project at any time. It also allows anyone to use the project for their own purposes.

For more information on the value of open source, read the [Open Source Guide](https://opensource.guide/).

## Cross platform
In conjunction with open source, cross platform is an essential component of this project; it should be accessible to anyone on any OS. Making cross-platform projects also becomes easier for open source projects where users with different setups can contribute.

## Full documentation
Sometimes, the biggest challenge of contributing to a new project is understanding what all of the code does. For this reason, it is our philosophy that every function, module, and component be documented and outlined, and have relevant example code to show users how its used and why. We utilize the [Sandcastle Help File Builder (SHFB)](https://github.com/EWSoftware/SHFB/releases) to build the documentation. SHFB uses formatted comments in source code files to build documentation pages in many formats, including HTML, Markdown, and Windows Help File.

## Community driven environment
Please follow the [Code of Conduct](CODE_OF_CONDUCT.md) for a more detailed explanation of working in a community project.

## Accessibility
Avoid hard coded strings and other culture-restrictive coding styles. Take advantage of resource files as much as possible.
