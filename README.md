# FingerPass

[<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/button.png" align="right">](https://github.com/gazugafan/fingerpass/releases/latest)

<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/about.png" align="left">

### A fingerprint-enabled Windows password manager

FingerPass is a Windows password manager that fills in your password when you scan your fingerprint. You can finally put that fingerprint reader to use!

It sits quietly in your tray watching your fingerprint scanner. Once a fingerprint is read, it verifies that the fingerprint belongs to you. If it does, FingerPass checks to see if your currently focused window matches any of your saved passwords. If a matching password is found, it types it in for you and optionally presses ENTER. Easy peasy!

This isn't necessarily meant to replace any existing password managers--especially browser-based ones. Instead, it's intended to be used alongside them, and to fill in some of their gaps. You could use FingerPass to manage the passwords used in all of your other Windows programs, and you could even use FingerPass to fill in your browser-based password manager's master password. If you do this, you could set your browser-based password manager to lock your database quickly--essentially adding a missing fingerprint feature to your browser-based password manager (relying on FingerPass's locking mechanism instead).

## Install [![Github All Releases](https://img.shields.io/github/downloads/gazugafan/fingerpass/total.svg)](https://github.com/gazugafan/fingerpass/releases/latest)

[Download the latest Windows release here](https://github.com/gazugafan/fingerpass/releases/latest)

...click "FingerPassSetup.msi" and run/open it.

## Setup Your Fingerprints

FingerPass uses Window's built-in local fingerprint database, so you'll need to register your fingerprint(s) with Windows Hello if you haven't done so already. However, keep in mind that FingerPass normally takes full control of your fingerprint scanner while it's running. This means Windows Hello won't be able to access your fingerprint scanner and register new fingerprints. To get around this, right-click the FingerPass tray icon and select "Pause Fingerprinting". This frees up your fingerprint scanner so it can be used by other programs.

To register fingerprints with Windows Hello, press Start/Windows, type in "fingerprint", and select "Set up fingerprint sign-in". Then, select "Windows Hello Fingerprint" and follow the instructions. Add as many fingerprints as you like--they'll all be linked to your Windows user account and work with FingerPass. Each user on your computer has their own set of fingerprints and their own password database in FingerPass.

If you paused FingerPass, be sure to unpause it after registering your fingerprints. To do this, right-click the FingerPass tray icon again and select "Resume Fingerprinting".

## Usage

FingerPass sits in your tray. It looks like a fingerprint!

<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/tray1.png">

If it's having trouble communicating with your fingerprint scanner, it will turn red. Hover over it to see what's going wrong.

<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/tray2.png">

Otherwise, when you scan your fingerprint it will flash green for a moment (if the fingerprint is one that you registered), or red for a moment (if the fingerprint is unrecognized or belongs to a different user).

<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/tray3.png">

If it looks greyed out, that just means the password database is currently locked. If that's the case, you'll be prompted to enter your master password the next time you try to fill in a password with your fingerprint. Having to enter your master password occassionally is what keeps your password database truly secure. By default, you'll need to enter it at least once per hour, but you can increase this timeout in the settings if you'd like.

Right-click the tray icon and select "Settings" to get to the settings. There's not much to configure besides the timeout...

<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/settings.png">

More importantly, select "Manage Passwords" from the settings or tray-icon to manage your password database...

<img src="https://raw.githubusercontent.com/gazugafan/fingerpass/master/docs/database.png">

This is where you'll save all the passwords that FingerPass will type in for you later. Each password includes a Program Name and a Window Title. Later, when you scan your fingerprint, FingerPass will look through your password database for a password with a Program Name and Window Title that matches the window you're currently working in. If a match is found, the password is typed into your program for you. If you checked the "Press Enter?" box, FingerPass will even press ENTER after typing in the password for you. 

Alternatively, you can check the "Just Copy?" checkbox to have the password copied to the clipboard instead of typed in. You can then paste the password yourself using CTRL+V, etc. This is a useful workaround for programs that don't play nice accepting automated keystrokes. However, keep in mind that your password will then be left on the clipboard in plaintext, and whatever was on the clipboard previously will be overwritten.

The Program Name and Window Title don't need to be exact matches. You can use wildcard characters * or ? to match any number of characters or any single character. For example, if you enter "\*something\*" for the Window Title (without the quotes), this would match any window title containing the word "something". Matches are also always case-INsensitive.

FingerPass looks through your password database from top-to-bottom and will use the first matching Program Name and Window Title it finds. You can drag-and-drop passwords to re-order them how you like. If no match is found, it will ask you if you'd like to create a new password. Alternatively, you could put one final password at the bottom of your database with a * for the Program Name and Window Title as a last default password to use when there are no other matches found.

Finally, to make it easy to figure out what to enter for the Program Name and Window Title, you can click around to other windows you have open. The password manager will stay on top and show you exactly what FingerPass reads as that window's Program Name and Window Title. If you already have a password that FingerPass would match, it also gets listed and highlighted in green.

Oh, you can also change your master password from the database manager, export your entire password database (decrypted in plaintext), or import a password database that you had previously exported.

## How It Works

FingerPass has two main components: a Windows service and a tray icon. The Windows service is necessary to take ongoing focus of the fingerprint scanner. Without it, Windows only allows a program to acquire and maintain focus of a fingerprint scanner while the program itself has a UI window with focus.

The tray icon acts as the UI for FingerPass, and communication between the tray icon and the service is handled via a named pipe. The service keeps the tray icon up-to-date on the status of the fingerprint scanner and sends over a user ID whenever an identified fingerprint is scanned. The tray icon lets the service know when it should aquire focus of the fingerprint scanner (when the tray icon starts or resumes) or when it should let it go (when the tray icon stops or pauses). This way, the service won't have focus of the fingerprint scanner when you might need it to login to Windows.

## Is It Secure?

Pretty secure! It shares the same benefits and shortcomings as most other password managers.

However, keep in mind that **fingerprints themselves cannot be used for encryption**. That's why FingerPass asks you for a master password occassionally. This is what is used to encrypt your passwords, and it's the ONLY thing that keeps your database truly secure.

Requiring you to scan your fingerprint adds a small amount of protection on top of this, as it makes it difficult for someone else to simply walk up to your computer and get at your passwords while your database is unlocked. However, your fingerprint is NOT used, in any way, to lock or unlock your database. It should only be viewed as a convenient way to trigger that you want to fill in a password while also providing some level of assurance that it really is you (and not a passerby). It provides absolutely no cryptographic security.

More security details are at the end of this README if you're interested!

## Why, though?

Fingerprint scanners have shockingly little use in Windows. They essentially just let you login when starting your computer or switching accounts. I got tired of looking at my useless fingerprint scanner, wondering why it couldn't do more, and decided to start developing something that would let me at least scan my fingerprint to fill in a password. What started as a weekend-long project exploded into this full-blown password manager for the rest of Windows!

## Security nitty gritty

The first time you run it, FingerPass generates two different random salts used to generate two SHA256 hashes of your master password for different purposes. The first is stored with the application's settings, and is simply used to check that your master password is correct when you enter it later. The second is never stored, and is used as entropy to encrypt/decrypt the passwords in your password database. Passwords are encrypted using Microsoft's ProtectedData class, which encrypts data using the currently logged in Windows user's credentials (and supplied entropy). So, in addition to being encrypted using your master password hash, your password database is also tied to your Windows account.

Your password database is only ever stored on disk with the passwords encrypted, and in fact is only ever kept in memory with the passwords encrypted. The rest of the database (the program names and window titles) is never encrypted and should be considered public data. When you unlock your password database, your master password is hashed and the result is kept in memory. This is used to decrypt individual passwords when necessary.

Unfortunately, one of the shortcomings of FingerPass (and most other password managers) is memory management... There's really no good way to clear your passwords from RAM once they've been decrypted. Even your master password will likely be left in RAM as soon as you enter it into the password textbox. To make matters worse: if you run short on RAM, Windows could end up moving those decrypted passwords onto the hard drive's swap file.

There are some possible approaches to mitigating this, but they're difficult to implement and the actual benefits are questionable. FingerPass doesn't employ any of them for now. In any case, if someone steals your computer while it's running, or can otherwise dump your RAM, you should probably consider your password database to be in jeopardy. If your computer is stolen while powered off, the risk should be lower but might still be possible due to RAM paging. None of these exploits would be *easy*, of course, so you would likely have time on your side in a scenario like this.

FingerPass types in passwords for you using .NET's SendKeys--the updated SendInput version introduced in .NET 3.0 (not the JournalHook version). It doesn't use the clipboard.

## Special thanks

* https://github.com/JcBernack/WinBioNET for abstracting the WinBio API into a C# class. I 100% would NOT have bothered doing this without their work.
* https://github.com/dvoaviarison/fast-ipc for putting together an easy-to-use named pipe IPC mechanism. If it didn't exist when I realized I needed it, I probably would've just given up.
