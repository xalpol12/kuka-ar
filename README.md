# KUKA AR app 🦾
![example workflow](https://github.com/xalpol12/kuka-ar-all/actions/workflows/main.yml/badge.svg)

Project created to visualize coordinate systems on KUKA robots in realtime.

## Install

### Download 🚚 📦 
Download current version from `Releases` tab OR clone project repository to your local drive.

### Manual build process 🏗️

> Skip this section if you downloaded .jar and .apk files from `Releases` tab

1. Go to project directory:
    - **kukaComm:** `java/kukaComm`
    - **testSocket:** `java/testSocket`

    *More information about which one to choose can be found in [_Available configurations_](/README.md#avaliable-configurations-📚) section.*

2. Enter command:

        mvn clean install

3. Open project in Unity editor.

4. Plug you phone and turn on developer mode 

    *If you have any questions go to [Troubleshooting](#troubleshooting) section.*

5. In the editor go to:

        File > Build Settings... > Select Android as Platform > Build and Run

6. Wait until the build is finished and enjoy.

## Avaliable configurations 📚 

### `KukaComm` 
Dedicated server side application that serves as a middle-man between KUKA robot and Android application. We recommend using it whenever you have an access to physical robot.

### `TestSocket`
Side project that was created for an easier Android app development. It sends data similar in values and identical in form as `KukaComm`, but those values are mocked on the server side. It is used when implementing new features or debugging an Android app. Eliminates the need for a robot or a robot connection access.

### `Android app`
In order to receive data and work properly, both applications have to be connected to the same local network.

## Troubleshooting 🐛 💡 

### Connection refused 📶 
Check if: 
- Android app is configured with a correct server IP address 

        More options icon > Reconfigure server > *here enter correct IP address*

    *More options icon can be found in left upper corner of your phone.*
- Chosen Spring server (either [KukaCommm](/README.md#kukacomm) or [TestSocket](/README.md#testsocket) is running 
- Android device is connected to the the same network as a server

### Unity editor is not showing your device 📴 
#### `Developer options disabled`
Check if you have `Developer options` enabled on your device.

        Settings > System > About phone - click on it several times

Usual amout of click is between 7 and 10. Remember that location of `About phone` tab can differ between manufacturers.

#### `ADB is not detecting device`
Go to

     Settings > Developer Options > Revoke USB debugging authorization

if `Revoke USB debugging authorization` is not available, do not worry.
Turn off and then on USB debugging and confirm all popups.

### Other problems ⚠️ 
#### `Play store check`
If your device shows a window with option to send the app for Google Play store check, please skip it as this app currently is not avaliable in production ready state.

#### `App install problem`
Check if you have "install from unknown sources" enabled in your device settings. If so, then try to install it several time and if popup with warning occurs - ignore it. If this won't help try to install a previous version.

### `Problem not found in this list?`
If you encounter any bugs, issues or if you are missing any feature - feel free to submit an [issue](https://github.com/xalpol12/kuka-ar-all/issues). Describe a problem or feature as precisly as you can and mark it with proper label.

## Contributions :accessibility: 
Any contribution is welcome. If there are no issues currently requested by users, feel free to submit new one and get started working on it.