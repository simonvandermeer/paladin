# Paladin

## Running Crypt of the NecroDancer

It's a lot easier to debug Paladin by running Crypt of the NecroDancer (CotN) in windowed mode. To run CotN in windowed mode through Steam set the launch options to the following:

> "C:\Program Files (x86)\Steam\steamapps\common\Crypt of the NecroDancer\NecroDancer.exe" || 960 540 %command%

## Building and running the agent

> docker build --tag paladin .
> docker run --rm paladin

## Notes:

https://codingvision.net/c-inject-a-dll-into-a-process-w-createremotethread
https://en.wikipedia.org/wiki/Shellcode#:~:text=In%20hacking%2C%20a%20shellcode%20is,exploitation%20of%20a%20software%20vulnerability.
https://www.andreafortuna.org/2019/03/06/a-simple-windows-code-injection-example-written-in-c/
