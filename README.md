# NetCoreServer Unity Client

![Unity Editor Screenshot](./Docs/preview.png)

*Lightweight Unity client for [NetCoreServer](https://github.com/chronoxor/NetCoreServer)*
[![openupm](https://img.shields.io/npm/v/com.deml.netcore-networking?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.deml.netcore-networking/) [![](https://img.shields.io/github/release-date/JohannesDeml/Unity-Net-Core-Networking-Sockets.svg)](https://github.com/supyrb/JohannesDeml/Unity-Net-Core-Networking-Sockets) [![Unity 2018.1 or later](https://img.shields.io/badge/unity-2018.1%20or%20later-green.svg?logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)



## Features

* Supports SSL, TCP & UDP
* Client code from NetCoreServer with wrappers for unity usage
* Precompiled server executables for windows for easy testing
* Small GC overhead - Usage of MemoryStream
* Async send and receive - Does not block the Main Thread
* Easy to use - Queue for fetching the last received messages
* Tested with local and remote server

| Platform | SSL  | TCP  | UDP  |
| -------- | :--: | :--: | :--: |
| Editor   |  ✔️   |  ✔️   |  ✔️   |
| Android  |  ✔️   |  ✔️   |  ✔️   |
| iOS      |  ✔️   |  ✔️   |  ✔️   |
| Windows  |  ✔️   |  ✔️   |  ✔️   |



## Installation

You can either install the package through the package manager with [OpenUPM](https://openupm.com/) (Recommended) or download the scripts as a unity package

### OpenUPM

```sh
# Install openupm-cli
$ npm install -g openupm-cli

# Enter your unity project folder
$ cd YOUR_UNITY_PROJECT_FOLDER

# Add package to your project
$ openupm openupm add com.deml.netcore-networking
```

### Download

[Latest Unity Package](../../releases/latest)



## Setup

* Unity 2020.1.0f1 (Works with older versions as well)
* Precompiled servers an in [ServerWindows](./ServerWindows), run the bat you want to test
* Open `Samples/EchoClient/Scenes/NetworkExampleClient.unity` and hit play. 



## TODO

* Split messages back into the sent chunks
* Repeating Asnyc sending results in some packages not being sent for UDP
* Support Websockets



## Libraries

* [NetCoreServer](https://github.com/chronoxor/NetCoreServer) by [Chronoxor](https://github.com/chronoxor/)



## License

* MIT - see [LICENSE](./LICENSE)
