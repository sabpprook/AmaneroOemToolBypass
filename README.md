# AmaneroOemToolBypass

Amanero official firmware tool can't be use on third party Combo384 USB card.

If trying update firmware will show the message below.

> ### Invalid or not authorized transaction!

![](https://github.com/sabpprook/AmaneroOemToolBypass/blob/main/pics/sshot-0001.png)

The loader will patch **ConfigTool.exe** to make firmware flashing allowed.
* **[AmaneroOemToolBypass.exe](https://github.com/sabpprook/AmaneroOemToolBypass/releases/latest/download/AmaneroOemToolBypass.exe)**
*(SHA256: d8474114e0dfba252da9edc7acda4697c73d18d4752abbf9f5143545b29d9207)*

***
![sshot-0001](https://user-images.githubusercontent.com/7044575/148890993-3a00da1f-cb59-4ab3-9ae9-9b89049293dd.png)![sshot-0002](https://user-images.githubusercontent.com/7044575/148891003-8a89d834-9a34-4cd7-8624-32243761da36.png)


### Best practice for third party Combo384 USB card

* **AKM4497** **AKM4499**
    + CPLD_for_1080 + firmware_1099akm
    + CPLD_1080_DSDSWAPPED + firmware_1099akm

* **ES9028PRO** **ES9038PRO**
    + CPLD_for_1080 + firmware_1099c
    + CPLD_1080_DSDSWAPPED + firmware_1099c

![](https://github.com/sabpprook/AmaneroOemToolBypass/blob/main/pics/sshot-0002.png)
