# TheTVApp for Jellyfin

_My first C# project!_

![Screenshot 2024-12-29 at 11.30.02 PM.png](screenshots/Screenshot%202024-12-29%20at%2011.30.02%E2%80%AFPM.png)

TheTVApp (https://thetvapp.to) is an aggregation service for FTA (free-to-air; https://en.wikipedia.org/wiki/Free-to-air) TV channels, but the interface on their website leaves much to be desired. This plugin enables the channels on TheTVApp to be played on Jellyfin.

After installing the plugin, no additional configuration is required, and you can immediately start watching the channels after performing a simple guide refresh.

## Building

To build the plugin, run the following commands:

```bash
dotnet build --configuration Release
```

The plugin will be output to `Jellyfin.Plugin.TheTVApp/bin/Release/net8.0/Jellyfin.Plugin.TheTVApp.dll`.

## Installation

If you built the plugin yourself, use the `dll` file(s) you generated. If you didn't, download the latest nightly build [here](https://nightly.link/regulad/jellyfin-plugin-thetvapp/workflows/build.yaml/master/build-artifact.zip).

The files needed are:

- `Jellyfin.Plugin.TheTVApp.dll`
- `OpenAI.dll`
- `System.Memory.Data.dll`
- `System.ClientModel.dll`

- Put the files in `/config/plugins/thetvapp` (or the directory for your installation method) and restart Jellyfin. Jellyfin will take care of the rest.

Tested on Jellyfin 10.10.3.

## Setup

This plugin requires a little bit of configuration. To pick up channels, the plugin needs an OpenAI API key so it can deobfuscate the encryption key for the HLS streams.

It uses the typical configuration flow for a plugin.

To get the key, go to [OpenAI's API website](https://platform.openai.com/docs/overview) and sign up for a key.

## Challenges

TheTVApp encrypts their HLS Stream URLs with a custom Vigenère-like cipher, but the key itself is obfuscated in the client-side JavaScript.

There's no easy way to write a program that can extract the key from the JavaScript, so instead we use an LLM with a finely tuned system prompt to extract the key.

You can see the system prompt [here](llm/prompt.md) and my reverse engineering journal [here](re/README.md).

## License

This project is GPLv3 licensed, like the rest of Jellyfin.

## TODO

If you are better at C# than I am, please feel free to contribute to this project with a PR.

- [ ] Add better marquee pictures to the channels (currently using the same image for all channels)
- [ ] Fetch channel numbers
- [ ] Add DVR functionality
- [ ] Add more specific tags to programs: `IsLive`, `IsForKids`, `IsNews`, `IsSports`, etc.
