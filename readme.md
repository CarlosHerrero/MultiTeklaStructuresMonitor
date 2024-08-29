# MultiTeklaStructuresMonitor

## Getting Started

To get started with this project, follow these steps:

1. **Build the Project**: Run the build command to compile the project.
2. **Run MultiTeklaStructuresMonitor**: Execute the `MultiTeklaStructuresMonitor` application. This should be enough to demonstrate the functionality.

## Documentation

### What is MultiTeklaStructuresMonitor?

The idea behind this project is to demonstrate how external clients, such as Revit, can create driver applications that connect to multiple versions of Tekla Structures without needing to recompile each tool with the appropriate Tekla NuGet versions.

### Key Features

- **Version Compatibility**: The application uses an executable app configuration patching mechanism to ensure the driver app communicates with the correct Tekla Structures version.
- **Support for Multiple Versions**: Versions since 2021 are demonstrated with this example, including both GAC and GACless builds.

This approach allows for seamless integration and communication with various Tekla Structures versions, simplifying the development and maintenance of driver applications.