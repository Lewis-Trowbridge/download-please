# download-please

## What is this?
A gRPC server download client, designed to run on a personal server and let you fling across URLs to be downloaded using multiprocessing to allow for multiple downloads at once.

## How can I install it?
Currently, the server is packaged as a .deb file. You can download it from the latest action run (a more convenient way is maybe coming soon when I get around to it) and install using dpkg.

```bash
dpkg -i download-please.deb
```

## How do I use it?
There are two gRPC endpoints, download and status. Use the included protobuf (a proper client coming maybe soon™), to connect, using download to initate a download, and status to check up on progress.

Currently the server will simply download whatever you throw it through http, but there is more support planned for specialised downloads (again, as and when I feel like it).

## Why is the name so polite?
I'm sure the machines will appreciate the courtesy when they take over, and I don't think you want to find out otherwise.
