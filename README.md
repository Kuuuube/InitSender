#  InitSender

Accepts newline-separated, base64-encoded feature inits or output inits. Including file output from [PTK led sandbox](https://github.com/Kuuuube/PTK-led-sandbox) when using the `no config` option.

Run either with or without command line args.

## Command line args

```
InitSender {tablet model} {init file} {init delay} {auto repeat} {init type} {tablet vid} {tablet pid}
```

### Tablet Model

`0`: Custom

`1`: PTK-540WL

`2`: PTK-640

`3`: PTK-840

`4`: PTK-1240

### Init File

Relative or absolute path to the file containing newline-separated, base64-encoded feature inits or output inits.

### Init Delay

Integer to use for delay between inits in miliseconds. (Use 0 for smoothest animations.)

### Auto Repeat

Whether or not to loop through the init file.

`1`: Auto repeat

`2`: Do not auto repeat

### Init Type

Type of init to send. For Wacom tablets you likely want Feature Init. For Xencelabs tablets you likely want Output Init.

`1`: Feature Init

`2`: Output Init

### Tablet VID

Base10 integer containing the Vendor ID for tablet. Only used when Tablet Model = 0.

### Tablet PID

Base10 integer containing the Product ID for tablet. Only used when Tablet Model = 0.

## Dependencies

.Net Runtime 6.0 x64: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Building

```
$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish InitSender $options --runtime win-x64 --framework net6.0 -o build/win-x64
dotnet publish InitSender $options --runtime linux-x64 --framework net6.0 -o build/linux-x64
```