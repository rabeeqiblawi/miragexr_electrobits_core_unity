# MirageXR.ElectroBits

A Unity package for prototyping and testing virtual electronic circuits. This package provides a WebSocket-based communication system for simulating electronic components.

## Features

- Real-time pin state management
- Digital and analog pin support
- Text-based communication
- WebSocket integration
- Event-driven architecture

## Installation

1. In Unity, go to Window > Package Manager
2. Click the "+" button in the top-left corner
3. Select "Add package from git URL"
4. Enter the repository URL
5. Click "Add"

Required Dependencies:
- Newtonsoft.Json
- WebSocketSharp

## Quick Start

```csharp

```

## API Reference

### VirtualPinsManager
- `SetDigitalPin(int index, int value)`: Set digital pin state
- `SetTextPin(int index, string value)`: Send text data
- `ReadPin(int index)`: Get pin state
- `Pins`: List of all available pins

### PinData
- `pinIndex`: Pin identifier
- `ioMode`: Input/Output mode
- `digitalValue`: Digital state (0/1)
- `analogValue`: Analog value
- `textValue`: String data
- `activeValueType`: Current value type

## License

This package is licensed for testing and prototyping purposes only.
See LICENSE.md for details.

## Contact

- Name: Rabee Qiblawi
- Email: rabeeqiblawi@gmail.com

## Contributing

This is a testing/prototyping package. For modifications or extended usage, please contact the author.

## Version History

- 1.0.0: Initial release
  - Basic pin management
  - WebSocket communication
  - Digital/Analog/Text support
