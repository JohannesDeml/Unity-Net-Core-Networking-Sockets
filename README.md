# NetCoreServer Unity Client

![Unity Editor Screenshot](docs/preview.png)

*Proof of Concept implementation for using [NetCoreServer](https://github.com/chronoxor/NetCoreServer) with a unity client*

## Features

* Supports UDP & TCP
* Precompiled server executables for windows for easy testing
* MemoryStream for small GC overhead
* Easy to use queue for fetching the last received messages



## Setup

* Unity 2019.3.13f1 (Should work in older versions as well)
* UdpClient and TcpClient from NetCoreServer Repo



## TODO

* Split messages correctly for TCP
* Put TCP and UDP client wrappers in own thread
* Nonblocking connect to server

## Libraries

* [NetCoreServer](https://github.com/chronoxor/NetCoreServer) by [Chronoxor](https://github.com/chronoxor/)

## License

* MIT - see [LICENSE](./LICENSE.md)