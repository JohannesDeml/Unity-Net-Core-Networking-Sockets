# NetCoreServer Unity Client

![Unity Editor Screenshot](./Docs/preview.png)

*Proof of Concept implementation for using [NetCoreServer](https://github.com/chronoxor/NetCoreServer) with a unity client*

## Features

* Supports UDP & TCP
* UdpClient and TcpClient from NetCoreServer Repo
* Precompiled server executables for windows for easy testing
* MemoryStream for small GC overhead
* Easy to use queue for fetching the last received messages
* Works with local and remote server



## Tested Platforms

* Android ✅
* Windows ✅



## Setup

* Unity 2019.3.13f1 (Should work in older versions as well)
* Precompiled servers an in [ServerWindows](./ServerWindows), run the bat you want to test
* Open Scenes/NetworkExampleClient.unity and hit play. 



## TODO

* Split messages correctly for TCP



## Libraries

* [NetCoreServer](https://github.com/chronoxor/NetCoreServer) by [Chronoxor](https://github.com/chronoxor/)



## License

* MIT - see [LICENSE](./LICENSE)
