#define WIN32_LEAN_AND_MEAN // Exclude rarely-used stuff from Windows headers

#include <thread>
#include <vector>
#include <cstddef>
#include <string>
#include <format>
#include <functional>
#include <fstream>
#include <chrono>

#include <windows.h>

#include "generated/IpcProtocol.pb.h"

constexpr auto BufferSize = 512;

void StartServer();
DWORD WINAPI ServerLoop(LPVOID lpParam);
DWORD WINAPI ClientLoop(LPVOID lpParam);
void HandleMessage(const char(& const message)[512], DWORD messageSize);

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        StartServer();
        //MessageBox(nullptr, L"Shellcode injected and ready.", L"Paladin", MB_OK);
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        // Don't need to release any threads because it will be done automatically on process exit.
        // We don't have to worry about DETACH because this dll will only be attached once and never
        // explicitly detached.
        // See also: https://docs.microsoft.com/en-us/windows/win32/dlls/dynamic-link-library-best-practices#general-best-practices
        break;
    }

    return TRUE;
}

void StartServer()
{
    DWORD threadId;
    auto threadHandle = CreateThread(
        nullptr,          // default security attributes
        0,                // use default stack size  
        ServerLoop,       // thread function name
        nullptr,          // argument to thread function 
        0,                // use default creation flags 
        &threadId);       // returns the thread identifier 

    if (threadHandle == nullptr)
    {
        MessageBox(nullptr, std::format(L"CreateThread failed, GLE={}", GetLastError()).c_str(), L"Paladin", MB_OK);
        return;
    }
}

DWORD WINAPI ServerLoop(LPVOID lpParam)
{
    while (true)
    {
        auto pipeHandle = CreateNamedPipe(L"\\\\.\\pipe\\paladin",
            PIPE_ACCESS_DUPLEX,
            PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT,
            PIPE_UNLIMITED_INSTANCES,
            BufferSize,
            BufferSize,
            0,
            nullptr);

        if (pipeHandle == INVALID_HANDLE_VALUE)
        {
            // TODO: Provide feedback with GetLastError().
            MessageBox(nullptr, std::format(L"CreateNamedPipe failed, GLE={}", GetLastError()).c_str(), L"Paladin", MB_OK);
            return 1;
        }

        // Wait for the client to connect; if it succeeds, 
        // the function returns a nonzero value. If the function
        // returns zero, GetLastError returns ERROR_PIPE_CONNECTED. 
        auto connected = ConnectNamedPipe(pipeHandle, nullptr) ?
            TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);

        if (!connected)
        {
            CloseHandle(pipeHandle);

            MessageBox(nullptr, std::format(L"ConnectNamedPipe failed, GLE={}", GetLastError()).c_str(), L"Paladin", MB_OK);
            return 1;
        }

        //MessageBox(nullptr, L"Client connected.", L"Paladin", MB_OK);

        // Create a thread for this client.
        DWORD threadId;
        auto threadHandle = CreateThread(
            nullptr,    // default security attributes
            0,          // use default stack size  
            ClientLoop, // thread function name
            pipeHandle, // argument to thread function 
            0,          // use default creation flags 
            &threadId); // returns the thread identifier 

        if (threadHandle == nullptr)
        {
            MessageBox(nullptr, std::format(L"CreateThread failed, GLE={}", GetLastError()).c_str(), L"Paladin", MB_OK);
            return 1;
        }
    }

    return 0;
}

// This routine is a thread processing function to read from and reply to a client
// via the open pipe connection passed from the main loop. Note this allows
// the main loop to continue executing, potentially creating more threads of
// of this procedure to run concurrently, depending on the number of incoming
// client connections.
DWORD WINAPI ClientLoop(LPVOID lpParam)
{
    //MessageBox(nullptr, L"Client loop.", L"Paladin", MB_OK);
    auto pipeHandle = (HANDLE)lpParam;

    while (true)
    {
        // Read client requests from the pipe.
        char messageBuffer[BufferSize];
        DWORD bytesRead = 0;
        auto succeeded = ReadFile(
            pipeHandle,
            &messageBuffer,
            BufferSize,
            &bytesRead,
            NULL);

        if (!succeeded || bytesRead == 0)
        {
            auto lastError = GetLastError();

            if (lastError == ERROR_BROKEN_PIPE)
            {
                // Client disconnected.
                break;
            }

            // TODO: Provide feedback with GetLastError().
            MessageBox(nullptr, std::format(L"ReadFile failed, GLE={}", lastError).c_str(), L"Paladin", MB_OK);
            break;
        }
        
        HandleMessage(messageBuffer, bytesRead);

        // Write the reply to the pipe. 
        //fSuccess = WriteFile(
        //    hPipe,        // handle to pipe 
        //    pchReply,     // buffer to write from 
        //    cbReplyBytes, // number of bytes to write 
        //    &cbWritten,   // number of bytes written 
        //    NULL);        // not overlapped I/O 

        //if (!fSuccess || cbReplyBytes != cbWritten)
        //{
        //    _tprintf(TEXT("InstanceThread WriteFile failed, GLE=%d.\n"), GetLastError());
        //    break;
        //}
    }

    // Flush the pipe to allow the client to read the pipe's contents 
    // before disconnecting. Then disconnect the pipe, and close the 
    // handle to this pipe instance. 
    FlushFileBuffers(pipeHandle);
    DisconnectNamedPipe(pipeHandle);
    CloseHandle(pipeHandle);

    return 0;
}

void Move(void* entityAddress, int32_t xOffset, int32_t yOffset)
{
    __asm
    {
        mov ecx,entityAddress
        mov eax,[ecx]
        push yOffset
        push xOffset
        call [eax+0x180]
    }
}

void HandleMessage(const char (& const message)[512], DWORD messageSize)
{
    auto now = std::chrono::system_clock::now();
    auto nowString = std::format(L"{}", now);
    std::replace(nowString.begin(), nowString.end(), ':', 'T'); // Replace illegal file name char ':'. 
    auto filePath = std::format(L"C:/temp/paladin/messages/{}.txt", nowString);

    IpcProtocol::Move moveAction;

    std::ofstream myfile(filePath);
    if (myfile.is_open())
    {
        myfile << "Message size: " << messageSize << std::endl;
        myfile.write(message, messageSize);
        myfile.close();
    }
    else
    {
        MessageBox(nullptr, std::format(L"Paladin: Could not open log file ({}).", errno).c_str(), L"Paladin", MB_OK);
    }

    if (moveAction.ParseFromArray(message, messageSize))
    {
        Move((void*)moveAction.entity_address(), moveAction.x_offset(), moveAction.y_offset());
    }
    else
    {
        MessageBox(nullptr, L"Paladin: Could not read action.", L"Paladin", MB_OK);
    }
}
