#  InitSender

Accepts newline-separated, base64-encoded feature inits. Including file output from [PTK led sandbox](https://github.com/Kuuuube/PTK-led-sandbox) when using the `no config` option.

Run either with or without command line args.

## Command line args

```
InitSender {tablet model} {init file} {init delay} {tablet vid} {tablet pid}
```

### Tablet Model

`0`: Custom

`1`: PTK-540WL

`2`: PTK-640

`3`: PTK-840

`4`: PTK-1240

### Init File

Relative or absolute path to file with inits.

### Init Delay

Integer to use for delay between inits in miliseconds.

### Tablet VID

Only used when Tablet Model = 0. Base10 Vendor ID for tablet.

### Tablet PID

Only used when Tablet Model = 0. Base10 Product ID for tablet.

## Building

```
$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish InitSender $options --runtime win-x64 --framework net5.0 -o build/win-x64
dotnet publish InitSender $options --runtime linux-x64 --framework net5.0 -o build/linux-x64
```