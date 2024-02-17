<img align="center" src="https://cdn.lypd0.com/FoxDrop/logo.png">


<h1 align="center">FoxDrop - A Malware Dropper Proof-of-Concept</h1>
<p align="center">
  <a href="#"><img alt="forksBDG" src="https://img.shields.io/github/forks/lypd0/FoxDrop?style=for-the-badge"></a>
  <a href="#"><img alt="starsBDG" src="https://img.shields.io/github/stars/lypd0/FoxDrop?style=for-the-badge"></a>
  <a href="#"><img alt="licenseBDG" src="https://img.shields.io/github/license/lypd0/FoxDrop?style=for-the-badge"></a>
  <a href="#"><img alt="languageBDG" src="https://img.shields.io/badge/LANGUAGE-CSHARP-green?style=for-the-badge"></a>
<h3 align="center">Usage on unauthorized systems is strictly forbidden</h3>

## Overview
FoxDrop is an educational proof-of-concept (PoC) malware dropper designed to illustrate the functionality of a dropper and its potential impact on a system. This project aims to provide insights into malware behavior, showcasing how droppers can silently infiltrate systems, execute payloads, and maintain persistence.

## Features
FoxDrop offers a range of features, including but not limited to:

- **Stealthy Execution**: FoxDrop operates discreetly, avoiding detection by traditional security measures.
- **Persistence**: It ensures its presence on the system by adding a run policy for execution on system reboot.
- **Dynamic Payload Retrieval**: FoxDrop fetches payloads from specified URLs, allowing for flexible and adaptable malware behavior.
- **Registry Manipulation**: To evade detection, FoxDrop creates decoy registry keys, camouflaging its activities.
- **Payload Integrity Checks**: FoxDrop hashes downloaded payloads and compares them to stored hashes to prevent re-execution.
- **Obfuscation Techniques**: FoxDrop supports Base64 decoding, adding an additional layer of obfuscation to its operation.

## Usage
Using FoxDrop for educational purposes involves several steps:
1. **Compilation**: Compile the provided source code using a C# compiler to generate the executable binary.
2. **Execution**: Execute the compiled binary on a target system to observe its behavior.
3. **Customization**: Customize the payload download URLs and other variables in the source code for experimentation.

## Disclaimer
FoxDrop is intended for educational purposes only. The author does not endorse or encourage any illegal or malicious use of this software. Any misuse or unethical use of FoxDrop is strictly prohibited.

## Author
FoxDrop is developed by Luigi Fiore, also known as lypd0.

## License
This project is licensed under the MIT License, allowing for exploration, modification, and distribution. Refer to the [LICENSE](LICENSE) file for detailed licensing terms.

## Troubleshooting and Debugging
FoxDrop includes built-in troubleshooting and debugging features to aid in understanding its behavior. These features provide insights into FoxDrop's configuration and highlight any potential issues that may affect its operation.

FoxDrop includes built-in troubleshooting and debugging features to aid in understanding its behavior:
```cpp
[╒═■] Build Information: 
[├] Assembly Info: FoxDrop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
[├] Execution Path: C:\Path\To\FoxDrop.exe
[└] Polymorphic Seed: 8bc8f9bfb6b6a42640a37dcf9aa9a22b

[*] Performing Checks... 
· Polymorphism Disabled, Checking static name ... OK.
· Checking Delay ... OK.

Press ENTER to exit.
```

## Debug Log
During execution, FoxDrop logs debug information to provide visibility into its operations. This debug log offers detailed information about FoxDrop's actions, facilitating analysis and comprehension of its behavior.

```cpp
[*] Waiting 300000 ms ...
[*] First execution on system: TRUE.
[*] Folder "92cbea44" created in the Registry.
[*] Created BEA key.
[*] Created "ShortcutName" key with value "Windows Audio Device Graph Isolation".
[*] Created "Path" key with value "C:\Windows\System32\audiodg.exe".
[*] Reading content from "https://pastebin.com/raw/b4SYNyAT".
[*] Loaded following data: "https://malicious.website/malware.exe;C:\Users\User\Downloads\malware.exe".
[*] Downloading & executing payload from "https://malicious.website/malware.exe"
[*] Downloaded payload to C:\Users\User\Downloads\malware.exe
[*] Payload started from C:\Users\User\Downloads\malware.exe
[*] Payload hash (md5) added to list to prevent re-execution.
```

## Conclusion
FoxDrop serves as a valuable educational tool for understanding malware droppers and their implications. By exploring its features and behavior, users can gain insights into malware tactics and enhance their cybersecurity knowledge and defenses.
