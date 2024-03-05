# PyEngineNET

PyEngineNET is a cross-platform .NET library which provides .NET/Python interop capabilities.

## Setup

### .NET

If you have AOT enabled in your project, you will need to add the following line to your csproj file, in order to prevent .NET from trimming required functionality related to MessagePack serialization:

```xml
<TrimMode>partial</TrimMode>
```

### Python

PyEngineNET relies on a small Python code driver which provides inter-process communication to the .NET process in order to facilitate the interop capabilities. Because of this, you will need to make sure you have Python (3+) installed on your system, and install the following Python packages:

```bash
pip install pywin32
pip install msgpack
```
