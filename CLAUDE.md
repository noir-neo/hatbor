# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Hatbor is a Unity-based VRM avatar rendering and streaming application that receives motion capture data via VMC protocol and streams the rendered output through Spout (Windows) or Syphon (macOS).

## Build and Development Commands

### Unity Project
- **Unity Version**: Check `ProjectSettings/ProjectVersion.txt` for required Unity version
- **Build Platform**: Primarily macOS, with Windows support
- **Build Process**: Use Unity Editor's Build Settings or Unity command line

### macOS Post-Build Process
After building from Unity, run the post-build script for signing and packaging:
```bash
./post-process-build-mac.sh
```
This script handles:
- Code signing with Apple Developer ID
- Notarization process
- DMG and PKG installer creation
- Requires proper Apple developer credentials

## Architecture Overview

### Dependency Injection with VContainer
The project uses VContainer for dependency injection. The main entry point is `MainLifetimeScope` which registers all core systems:
- Systems requiring lifecycle management use `RegisterEntryPoint<T>()`
- Singletons are registered with `Register<T>().AsSingleton()`
- Child containers are created dynamically (e.g., for avatar loading)

### Core System Interactions
1. **VMC Server** (`VmcServer.cs`) receives OSC messages with motion capture data
2. **Avatar System** (`AvatarLoader.cs`, `AvatarRig.cs`) applies motion data to VRM models
3. **Configuration System** (`ConfigStore.cs`) manages all settings via reactive properties
4. **Texture Streaming** (`TextureStreamingSender.cs`) broadcasts output via Spout/Syphon
5. **UI System** (`ConfigRoot.cs`) provides runtime configuration using UI Toolkit

### Configuration Pattern
All configuration classes:
- Implement `IConfigurable` interface
- Use `InspectableReactiveProperty<T>` for reactive values
- Are automatically serialized to JSON in PlayerPrefs
- Support runtime modification through UI

### VMC Protocol Integration
The VMC server processes OSC messages at `/VMC/Ext/*` paths:
- Root transform and bone positions
- Blend shape values for expressions
- Camera pose and FOV data
- Availability status monitoring

### Avatar Loading
- Dynamically loads VRM files from disk
- Creates child DI container per avatar (`AvatarInstaller`)
- Properly disposes resources on avatar change
- Default avatar in StreamingAssets

### Platform-Specific Features
- **Windows**: Spout texture sharing (`SpoutSender.cs`)
- **macOS**: Syphon texture sharing (`SyphonSender.cs`)
- Texture sender interface (`ITextureSender`) abstracts platform differences

## Key Dependencies

### Package Dependencies (via Git)
- **UniTask**: Async/await operations
- **UniRx**: Reactive programming
- **UniVRM**: VRM avatar support
- **VContainer**: Dependency injection
- **uOSC**: OSC protocol for VMC

### Unity Packages
- Universal Render Pipeline (URP)
- Input System
- UI Toolkit
- TextMeshPro

## Development Patterns

### Reactive Properties
Configuration values use `ReactiveProperty<T>` from UniRx:
- Changes automatically propagate to dependent systems
- UI bindings update in real-time
- Subscribe to value changes with `.Subscribe()`

### Async Operations
Use UniTask for asynchronous operations:
- Avatar loading
- File I/O operations
- Non-blocking asset loading

### UI Development
The project uses Unity's UI Toolkit (not UGUI):
- UXML files define UI structure
- Runtime bindings via `RuntimeBindingExtensions`
- Dynamic UI generation for configuration properties

### Performance Monitoring
Built-in profiler system tracks:
- Frame rate (`FrameRateProfilerRecorder`)
- VMC message count (`VmcServerProfilerRecorder`)
- Display via `PerformanceGroup` in UI

## Assembly Structure

The project is split into multiple assemblies for modularity:
- `Hatbor.Avatar`: Avatar loading and management
- `Hatbor.Camera`: Camera control systems
- `Hatbor.Config`: Configuration management
- `Hatbor.VMC`: VMC protocol implementation
- `Hatbor.TextureStreaming.Spout/Syphon`: Platform-specific streaming
- `Hatbor.UI`: UI components and bindings
- `Hatbor.Rig.VMC`: VMC-specific rig implementations

## Testing

Uses Unity Test Framework. Test assemblies reference:
- `UnityEngine.TestRunner`
- `UnityEditor.TestRunner`

Run tests through Unity Test Runner window or command line.