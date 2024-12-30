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

Simply put this plugin in `/config/plugins/thetvapp` and restart Jellyfin. Jellyfin will take care of the rest.

Tested on Jellyfin 10.10.3,

## License

This project is GPLv3 licensed, like the rest of Jellyfin.

## TODO

If you are better at C# than I am, please feel free to contribute to this project with a PR.

- [ ] Add better marquee pictures to the channels (currently using the same image for all channels)
- [ ] Fetch channel numbers
- [ ] Add DVR functionality
- [ ] Add more specific tags to programs: `IsLive`, `IsForKids`, `IsNews`, `IsSports`, etc.
