# AmaneroOemToolBypass

Amanero official firmware tool can't be use on cloned Combo384 USB card.

![board-front](https://github.com/sabpprook/AmaneroOemToolBypass/blob/main/pics/board-front.png)
![board-back](https://github.com/sabpprook/AmaneroOemToolBypass/blob/main/pics/board-back.png)

If trying update firmware will show the message below.

> ### Invalid or not authorized transaction!

![sshot-0001](https://github.com/sabpprook/AmaneroOemToolBypass/blob/main/pics/sshot-0001.png)

The loader will patch **ConfigTool.exe** to make firmware flashing is allowed.
* **[AmaneroOemToolBypass.exe](https://github.com/sabpprook/AmaneroOemToolBypass/releases/latest/download/AmaneroOemToolBypass.exe)**

Please install .NET 6.0 Desktop Runtime ([x86](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)) first.

***

### Best practice for third party Combo384 USB card

* **AKM4497** **AKM4499**
    + CPLD_for_1080 + firmware_1099akm
    + CPLD_1080_DSDSWAPPED + firmware_1099akm

* **ES9028PRO** **ES9038PRO**
    + CPLD_for_1080 + firmware_1099c
    + CPLD_1080_DSDSWAPPED + firmware_1099c

* **2022 New Firmware**
    + CPLD_for_1091 + firmware_2006be15r2_nodop

![sshot-0002](https://github.com/sabpprook/AmaneroOemToolBypass/blob/main/pics/sshot-0002.png)
