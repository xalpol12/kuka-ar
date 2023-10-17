# KUKA AR app 🦾
Android app to visualize coords systems on KUKA robots in realtime.

![example workflow](https://github.com/xalpol12/kuka-ar-all/actions/workflows/main.yml/badge.svg)

## Install

### Download 🚚 📦 
Clone project repository to your local drive.

### Build process 🏗️ 
1. Go to project directory:
    - **kukaComm:** `java/kukaComm`
    - **testSocket:** `java/testSocket`

    More information about which one to choose you will find in [_avaliable server configurations section_](#avaliable-server-configurations).

2. Enter command:

        mvn clean install
3. Open project in Unity editor.
4. Plug you phone and turn on developer mode <br />
    If you have any question go to [Troubleshooting](#troubleshooting).
5. In the editor go to:

        File > Build Settings... > Select Android as Platform > Build and Run
6. Wait until finish and enjoy.

## Avaliable configurations 📚 

### `KukaComm` 
Dedicated server side application to serve as midman between KUKA robot and Android application. We recommend to use it whenever you have an access to psychical robot.

### `TestSocket`
Side project that was created and developed for easier Andoid app debugging. It's sending data similar in values and identical in form as `KukaComm` but those values are faked. Which can be useful when performing tasks without robot or robot connection access.

### `Andorid app`
In order to recevie data and work properly, both applications have to be connected to the same network.

## Troubleshooting 🐛 💡 

### Connection refused 📶 
Check following: 
- is Android app pointing server IP address

        More options icon > Reconfigure server > visible IP address
    More options icon can be found in left upper corner of your phone.  
- is server running and has a connection with the Internet
- are you in the same network as server

### Unity editor is not showing your device 📴 
#### `Developer options disabled`
Check do you have `Developer options` enabled on your device.

        Settings > System > About phone - click it serveral times

Usual amout of click is between 7 and 10. Remember that location of `About phone` tab can differ between manufacturers.

#### `ADB is not detection device`
Go to

     Settings > Developer Options > Revoke USB debugging authorization
if `Revoke USB debugging authorization` is not avaliable do not worry.
Turn off and then on USB debugging and confirm all popups.

### Other problems ⚠️ 
#### `Play store check`
If your device will show window with options to send it for Google Play store check, please skip it as this app currently is not avaliable in production ready state.

#### `App install problem`
Check do you have install from unown sources enabled in your device settings. If so then try to install it several time and if popup with warning will occure ignore it. If this won't help try to get a previous version.

### `Problem not found`
If you will encounter any bug, issue or if you are missing any feature feel free to submit an [issue](https://github.com/xalpol12/kuka-ar-all/issues). Describe a problem or feature as precisly as you can and mark with proper label.

## Contribution :accessibility: 
Any contribution is welcome. If there are not any issues currently requested by users feel free to submit new one, and get started working over it.