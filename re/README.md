# ThatTVApp Reverse Engineering Log

## Page Types

### 1. Listing Pages

The ThatTVApp website is a web application that sources FTA TV channels from around the US. It exposes a central webpage for "Live TV," as well as each of the "Big 4" sports leagues (NFL, NBA, MLB, NHL) in the US, and college football.

This makes the 5 listing pages:

- TV: https://thetvapp.to/tv
- NFL: https://thetvapp.to/nfl
- NBA: https://thetvapp.to/nba
- MLB: https://thetvapp.to/mlb
- NHL: https://thetvapp.to/nhl
- NCAAF: https://thetvapp.to/ncaaf

#### Included Data

<details>
<summary>HTML</summary>

```html
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="description" content="Watch live TV from ABC, CBS, NBC, FOX, Fox News, CNN, ESPN and popular cable networks in the
    US.">
    <meta name="keywords" content="Live TV, ABC Live, CBS Live, CNN live, Live News">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="7M2cR7uwhShXrTWinYYf2mKqgC8azyPyG8e9YOKg">
        <title>All Live TV Channels - Thetvapp.to</title>
    <!-- Scripts -->
    <link rel="preload" as="style" href="https://thetvapp.to/build/assets/app-24fac727.css" /><link rel="modulepreload" href="https://thetvapp.to/build/assets/app-bcabbe84.js" /><link rel="stylesheet" href="https://thetvapp.to/build/assets/app-24fac727.css" /><script type="module" src="https://thetvapp.to/build/assets/app-bcabbe84.js"></script>
	<!-- Bootstrap CSS -->
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet">

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js"></script>
    </head>

<body>
    <!-- navbar -->
    <header class="p-3 text-bg-dark">
        <div class="container">
            <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                <a class="navbar-brand" href="/">
                    <img src="https://thetvapp.to/img/TheTVApp.svg" width="80%" class="d-inline-block align-top"
                        alt="">
                </a>

                <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                    <li><a href="/" class="nav-link px-2 text-white">Home</a></li>
                    <li><a href="https://thetvapp.to/tv" class="nav-link px-2 text-white">Live TV</a></li>
                    <li><a href="https://thetvapp.to/nba" class="nav-link px-2 text-white">NBA</a></li>
                    <li><a href="https://thetvapp.to/mlb" class="nav-link px-2 text-white">MLB</a></li>
                    <li><a href="https://thetvapp.to/nhl" class="nav-link px-2 text-white">NHL</a></li>
                    <li><a href="https://thetvapp.to/nfl" class="nav-link px-2 text-white">NFL</a></li>
                    <li><a href="https://thetvapp.to/ncaaf" class="nav-link px-2 text-white">NCAAF</a></li>
                    <li><a href="https://thetvapp.to/ncaab" class="nav-link px-2 text-white">NCAAB</a>
                    </li>
                    <li><a href="https://thetvapp.to/soccer" class="nav-link px-2 text-white">Soccer</a></li>
                    <li><a href="https://thetvapp.to/ppv" class="nav-link px-2 text-white">PPV</a></li>
                </ul>



                <div class="text-end">
                    <a href="https://thetvapp.to/contact"><button type="button"
                            class="btn btn-outline-light me-2">Contact Us</button></a>


					<a href="https://discord.gg/keZ3RwNq4b" target="_blank"><button type="button" class="btn btn-success">Join Our Discord</button></a>
                </div>

				<div class="m-2">
                    <a href="https://thetvsub.to" target="_blank" class="btn btn-primary">Subscribe Now</a>
                </div>
            </div>
        </div>
    </header>
    <!-- navbar -->

    <div class="container my-4">

            <div class="row">
        <h3>Live TV Channels</h3>
        <div class="col-lg-8 mt-5">
            <ol class="list-group list-group-numbered">
                                    <a href="/tv/ae-live-stream/" class="list-group-item">A&amp;E</a>
                                    <a href="/tv/acc-network-live-stream/" class="list-group-item">ACC Network</a>
                                    <a href="/tv/amc-live-stream/" class="list-group-item">AMC</a>
                                    <a href="/tv/american-heroes-channel-live-stream/" class="list-group-item">American Heroes Channel</a>
                                    <a href="/tv/animal-planet-live-stream/" class="list-group-item">Animal Planet</a>
                                    <a href="/tv/bbc-america-live-stream/" class="list-group-item">BBC America</a>
                                    <a href="/tv/bbc-world-news-hd-live-stream/" class="list-group-item">BBC World News HD</a>
                                    <a href="/tv/bet-live-stream/" class="list-group-item">BET</a>
                                    <a href="/tv/bet-her-live-stream/" class="list-group-item">BET Her</a>
                                    <a href="/tv/big-ten-network-live-stream/" class="list-group-item">Big Ten Network</a>
                                    <a href="/tv/bloomberg-tv-live-stream/" class="list-group-item">Bloomberg TV</a>
                                    <a href="/tv/boomerang-live-stream/" class="list-group-item">Boomerang</a>
                                    <a href="/tv/bravo-live-stream/" class="list-group-item">Bravo</a>
                                    <a href="/tv/cartoon-network-live-stream/" class="list-group-item">Cartoon Network</a>
                                    <a href="/tv/cbs-sports-network-live-stream/" class="list-group-item">CBS Sports Network</a>
                                    <a href="/tv/cinemax-live-stream/" class="list-group-item">Cinemax</a>
                                    <a href="/tv/cmt-live-stream/" class="list-group-item">CMT</a>
                                    <a href="/tv/cnbc-live-stream/" class="list-group-item">CNBC</a>
                                    <a href="/tv/cnn-live-stream/" class="list-group-item">CNN</a>
                                    <a href="/tv/comedy-central-live-stream/" class="list-group-item">Comedy Central</a>
                                    <a href="/tv/cooking-channel-live-stream/" class="list-group-item">Cooking Channel</a>
                                    <a href="/tv/crime-investigation-hd-live-stream/" class="list-group-item">Crime &amp; Investigation HD</a>
                                    <a href="/tv/cspan-live-stream/" class="list-group-item">CSPAN</a>
                                    <a href="/tv/cspan-2-live-stream/" class="list-group-item">CSPAN 2</a>
                                    <a href="/tv/destination-america-live-stream/" class="list-group-item">Destination America</a>
                                    <a href="/tv/discovery-live-stream/" class="list-group-item">Discovery</a>
                                    <a href="/tv/discovery-family-channel-live-stream/" class="list-group-item">Discovery Family Channel</a>
                                    <a href="/tv/discovery-life-live-stream/" class="list-group-item">Discovery Life</a>
                                    <a href="/tv/disney-channel-east-live-stream/" class="list-group-item">Disney Channel (East)</a>
                                    <a href="/tv/disney-junior-live-stream/" class="list-group-item">Disney Junior</a>
                                    <a href="/tv/disney-xd-live-stream/" class="list-group-item">Disney XD</a>
                                    <a href="/tv/e-live-stream/" class="list-group-item">E!</a>
                                    <a href="/tv/espn-live-stream/" class="list-group-item">ESPN</a>
                                    <a href="/tv/espn2-live-stream/" class="list-group-item">ESPN2</a>
                                    <a href="/tv/espnews-live-stream/" class="list-group-item">ESPNews</a>
                                    <a href="/tv/espnu-live-stream/" class="list-group-item">ESPNU</a>
                                    <a href="/tv/food-network-live-stream/" class="list-group-item">Food Network</a>
                                    <a href="/tv/fox-business-network-live-stream/" class="list-group-item">Fox Business Network</a>
                                    <a href="/tv/fox-news-channel-live-stream/" class="list-group-item">FOX News Channel</a>
                                    <a href="/tv/fox-sports-1-live-stream/" class="list-group-item">FOX Sports 1</a>
                                    <a href="/tv/fox-sports-2-live-stream/" class="list-group-item">FOX Sports 2</a>
                                    <a href="/tv/freeform-live-stream/" class="list-group-item">Freeform</a>
                                    <a href="/tv/fuse-hd-live-stream/" class="list-group-item">Fuse HD</a>
                                    <a href="/tv/fx-live-stream/" class="list-group-item">FX</a>
                                    <a href="/tv/fx-movie-live-stream/" class="list-group-item">FX Movie</a>
                                    <a href="/tv/fxx-live-stream/" class="list-group-item">FXX</a>
                                    <a href="/tv/fyi-live-stream/" class="list-group-item">FYI</a>
                                    <a href="/tv/golf-channel-live-stream/" class="list-group-item">Golf Channel</a>
                                    <a href="/tv/hallmark-live-stream/" class="list-group-item">Hallmark</a>
                                    <a href="/tv/hallmark-drama-hd-live-stream/" class="list-group-item">Hallmark Drama HD</a>
                                    <a href="/tv/hallmark-movies-mysteries-hd-live-stream/" class="list-group-item">Hallmark Movies &amp; Mysteries HD</a>
                                    <a href="/tv/hbo-2-east-live-stream/" class="list-group-item">HBO 2 East</a>
                                    <a href="/tv/hbo-comedy-hd-live-stream/" class="list-group-item">HBO Comedy HD</a>
                                    <a href="/tv/hbo-east-live-stream/" class="list-group-item">HBO East</a>
                                    <a href="/tv/hbo-family-east-live-stream/" class="list-group-item">HBO Family East</a>
                                    <a href="/tv/hbo-signature-live-stream/" class="list-group-item">HBO Signature</a>
                                    <a href="/tv/hbo-zone-hd-live-stream/" class="list-group-item">HBO Zone HD</a>
                                    <a href="/tv/hgtv-live-stream/" class="list-group-item">HGTV</a>
                                    <a href="/tv/history-live-stream/" class="list-group-item">History</a>
                                    <a href="/tv/hln-live-stream/" class="list-group-item">HLN</a>
                                    <a href="/tv/ifc-live-stream/" class="list-group-item">IFC</a>
                                    <a href="/tv/investigation-discovery-live-stream/" class="list-group-item">Investigation Discovery</a>
                                    <a href="/tv/ion-television-east-hd-live-stream/" class="list-group-item">ION Television East HD</a>
                                    <a href="/tv/lifetime-live-stream/" class="list-group-item">Lifetime</a>
                                    <a href="/tv/lmn-live-stream/" class="list-group-item">LMN</a>
                                    <a href="/tv/logo-live-stream/" class="list-group-item">Logo</a>
                                    <a href="/tv/metv-toons-live-stream/" class="list-group-item">MeTV Toons</a>
                                    <a href="/tv/mlb-network-live-stream/" class="list-group-item">MLB Network</a>
                                    <a href="/tv/moremax-live-stream/" class="list-group-item">MoreMAX</a>
                                    <a href="/tv/motortrend-hd-live-stream/" class="list-group-item">MotorTrend HD</a>
                                    <a href="/tv/moviemax-live-stream/" class="list-group-item">MovieMAX</a>
                                    <a href="/tv/msnbc-live-stream/" class="list-group-item">MSNBC</a>
                                    <a href="/tv/mtv-live-stream/" class="list-group-item">MTV</a>
                                    <a href="/tv/nat-geo-wild-live-stream/" class="list-group-item">Nat Geo WILD</a>
                                    <a href="/tv/national-geographic-live-stream/" class="list-group-item">National Geographic</a>
                                    <a href="/tv/nba-tv-live-stream/" class="list-group-item">NBA TV</a>
                                    <a href="/tv/newsmax-tv/" class="list-group-item">Newsmax TV</a>
                                    <a href="/tv/nfl-network-live-stream/" class="list-group-item">NFL Network</a>
                                    <a href="/tv/nfl-red-zone/" class="list-group-item">NFL Red Zone</a>
                                    <a href="/tv/nhl-network-live-stream/" class="list-group-item">NHL Network</a>
                                    <a href="/tv/nick-jr-live-stream/" class="list-group-item">Nick Jr.</a>
                                    <a href="/tv/nickelodeon-east-live-stream/" class="list-group-item">Nickelodeon East</a>
                                    <a href="/tv/nicktoons-live-stream/" class="list-group-item">Nicktoons</a>
                                    <a href="/tv/outdoor-channel-live-stream/" class="list-group-item">Outdoor Channel</a>
                                    <a href="/tv/own-live-stream/" class="list-group-item">OWN</a>
                                    <a href="/tv/oxygen-true-crime-live-stream/" class="list-group-item">Oxygen True Crime</a>
                                    <a href="/tv/pbs-13-wnet-new-york-live-stream/" class="list-group-item">PBS 13 (WNET) New York</a>
                                    <a href="/tv/reelzchannel-live-stream/" class="list-group-item">ReelzChannel</a>
                                    <a href="/tv/science-live-stream/" class="list-group-item">Science</a>
                                    <a href="/tv/sec-network-live-stream/" class="list-group-item">SEC Network</a>
                                    <a href="/tv/showtime-e-live-stream/" class="list-group-item">Showtime (E)</a>
                                    <a href="/tv/showtime-2-live-stream/" class="list-group-item">SHOWTIME 2</a>
                                    <a href="/tv/starz-east-live-stream/" class="list-group-item">STARZ East</a>
                                    <a href="/tv/sundancetv-hd-live-stream/" class="list-group-item">SundanceTV HD</a>
                                    <a href="/tv/syfy-live-stream/" class="list-group-item">SYFY</a>
                                    <a href="/tv/tbs-live-stream/" class="list-group-item">TBS</a>
                                    <a href="/tv/tcm-live-stream/" class="list-group-item">TCM</a>
                                    <a href="/tv/teennick-live-stream/" class="list-group-item">TeenNick</a>
                                    <a href="/tv/telemundo-east-live-stream/" class="list-group-item">Telemundo East</a>
                                    <a href="/tv/tennis-channel-live-stream/" class="list-group-item">Tennis Channel</a>
                                    <a href="/tv/the-cw-live-stream/" class="list-group-item">The CW (WPIX New York)</a>
                                    <a href="/tv/the-movie-channel-east-live-stream/" class="list-group-item">The Movie Channel East</a>
                                    <a href="/tv/the-weather-channel-live-stream/" class="list-group-item">The Weather Channel</a>
                                    <a href="/tv/tlc-live-stream/" class="list-group-item">TLC</a>
                                    <a href="/tv/tnt-live-stream/" class="list-group-item">TNT</a>
                                    <a href="/tv/travel-channel-live-stream/" class="list-group-item">Travel Channel</a>
                                    <a href="/tv/trutv-live-stream/" class="list-group-item">truTV</a>
                                    <a href="/tv/tv-one-hd-live-stream/" class="list-group-item">TV One HD</a>
                                    <a href="/tv/universal-kids-live-stream/" class="list-group-item">Universal Kids</a>
                                    <a href="/tv/univision-east-live-stream/" class="list-group-item">Univision East</a>
                                    <a href="/tv/usa-network-live-stream/" class="list-group-item">USA Network</a>
                                    <a href="/tv/vh1-live-stream/" class="list-group-item">VH1</a>
                                    <a href="/tv/vice-live-stream/" class="list-group-item">VICE</a>
                                    <a href="/tv/wabc-new-york-abc-east-live-stream/" class="list-group-item">WABC (New York) ABC East</a>
                                    <a href="/tv/wcbs-new-york-cbs-east-live-stream/" class="list-group-item">WCBS (New York) CBS East</a>
                                    <a href="/tv/we-tv-live-stream/" class="list-group-item">WE tv</a>
                                    <a href="/tv/wnbc-new-york-nbc-east-live-stream/" class="list-group-item">WNBC (New York) NBC East</a>
                                    <a href="/tv/wnyw-new-york-fox-east-live-stream/" class="list-group-item">WNYW (New York) FOX East</a>
                            </ol>
        </div>
    </div>

    </div>

	<!-- Bootstrap JS -->
	<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4" crossorigin="anonymous"></script>


   <script>
        $("span").each(function() {
            var startTime = new Date($(this).text());
            if (startTime) {
                $(this).text(startTime.toLocaleString("en-US", {
                    dateStyle: "short",
                    timeStyle: "long",
                    hour12: true
                })).css({
                    "background-color": "lightblue",
                    "font-size": "100%",
                    "color": "black",
                    "font-weight": "400"
                });
            }
        });
    </script>


    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-KDJRXWJ0P0"></script>
    <script>
        window.dataLayer = window.dataLayer || [];

        function gtag() {
            dataLayer.push(arguments);
        }
        gtag('js', new Date());

        gtag('config', 'G-KDJRXWJ0P0');
    </script>

</body>

</html>
```
</details>

In this page, we are only interested in one thing: the list of channels.

These pages can be in one of two possible states: channels available, or no channels available.

In both cases, we can identify the element (selector `body > div > div > div > ol`) that contains the list of elements.

In the case where no channels are available, this list will contain a single `p` element with the text `No Match Found`.

If there are channels, each channel is represented by an `a` element. The `href` of this element points to something called a **Streaming Page**

### 2. Streaming Pages

This page basically just bootstraps a JWPlayer instance with a video source that is encrypted. The video source is decrypted by the client-side JavaScript.

#### Included Data

For regular TV channels, the page contains three things:

<details>
<summary>TV Channel HTML</summary>

```html
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="description" content="Watch A&amp;E live and other popular cable networks, no cable required.">
    <meta name="keywords" content="A&amp;E live stream, thetvapp, thetvapp.to">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="7M2cR7uwhShXrTWinYYf2mKqgC8azyPyG8e9YOKg">
        <title>A&amp;E live stream - TheTVApp</title>
    <!-- Scripts -->
    <link rel="preload" as="style" href="https://thetvapp.to/build/assets/app-24fac727.css" /><link rel="modulepreload" href="https://thetvapp.to/build/assets/app-bcabbe84.js" /><link rel="stylesheet" href="https://thetvapp.to/build/assets/app-24fac727.css" /><script type="module" src="https://thetvapp.to/build/assets/app-bcabbe84.js"></script>
	<!-- Bootstrap CSS -->
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet">

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://thetvapp.to/player/v/8.29.0/jwplayer.js"></script>

    <script type="text/javascript">
        jwplayer.key = "uoW6qHjBL3KNudxKVnwa3rt5LlTakbko9e6aQ6VUyKQ=";
    </script>

</head>

<body>
    <!-- navbar -->
    <header class="p-3 text-bg-dark">
        <div class="container">
            <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                <a class="navbar-brand" href="/">
                    <img src="https://thetvapp.to/img/TheTVApp.svg" width="80%" class="d-inline-block align-top"
                        alt="">
                </a>

                <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                    <li><a href="/" class="nav-link px-2 text-white">Home</a></li>
                    <li><a href="https://thetvapp.to/tv" class="nav-link px-2 text-white">Live TV</a></li>
                    <li><a href="https://thetvapp.to/nba" class="nav-link px-2 text-white">NBA</a></li>
                    <li><a href="https://thetvapp.to/mlb" class="nav-link px-2 text-white">MLB</a></li>
                    <li><a href="https://thetvapp.to/nhl" class="nav-link px-2 text-white">NHL</a></li>
                    <li><a href="https://thetvapp.to/nfl" class="nav-link px-2 text-white">NFL</a></li>
                    <li><a href="https://thetvapp.to/ncaaf" class="nav-link px-2 text-white">NCAAF</a></li>
                    <li><a href="https://thetvapp.to/ncaab" class="nav-link px-2 text-white">NCAAB</a>
                    </li>
                    <li><a href="https://thetvapp.to/soccer" class="nav-link px-2 text-white">Soccer</a></li>
                    <li><a href="https://thetvapp.to/ppv" class="nav-link px-2 text-white">PPV</a></li>
                </ul>



                <div class="text-end">
                    <a href="https://thetvapp.to/contact"><button type="button"
                            class="btn btn-outline-light me-2">Contact Us</button></a>


					<a href="https://discord.gg/keZ3RwNq4b" target="_blank"><button type="button" class="btn btn-success">Join Our Discord</button></a>
                </div>

				<div class="m-2">
                    <a href="https://thetvsub.to" target="_blank" class="btn btn-primary">Subscribe Now</a>
                </div>
            </div>
        </div>
    </header>
    <!-- navbar -->

    <div class="container my-4">

            <div class="row">
        <div class="col-lg-8">

                            <div class="alert alert-primary" role="alert">
                    Subscribe now to install our app on smart TVs and streaming devices.
                </div>

            <div class="mb-3">
                <h2>A&amp;E</h2>
            </div>

                            <div id="encrypted-text" data="vGV0zLG6Db92XYNafNczyGDegBSxoL8wgIeuZ0AEVTJtvF9agnXnlX5hH3T4T3Ozu2NxAZ9JE3fCv3r3TlbpigFZKJSgX2YCMioWx1ZZwIaXcsj6BOTNLKGJwWQHf0m3ZmkvHBMJbIPzgk09"></div>
                <button id="loadVideoBtn" class="video-button"></button>
                <div id="my-jwplayer" class="mt-3"></div>

        </div>
        <div class="col-lg-4 pt-3">
                        <ul class="nav nav-pills navbar-inverse mb-3" id="myTab">
                <li class="nav-item"><a class="nav-link active" data-bs-toggle="tab" href="#schedule">TV Schedule</a></li>
                <li class="nav-item"><a class="nav-link" data-bs-toggle="tab" href="#channels">All Channels</a></li>
            </ul>
            <div class="tab-content">
                <div id="channels" class="tab-pane fade">
                    <ol class="list-group list-group-numbered">
                                                    <a href="/tv/ae-live-stream/"
                                class="list-group-item list-group-item-dark">A&amp;E</a>
                                                    <a href="/tv/acc-network-live-stream/"
                                class="list-group-item list-group-item-dark">ACC Network</a>
                                                    <a href="/tv/amc-live-stream/"
                                class="list-group-item list-group-item-dark">AMC</a>
                                                    <a href="/tv/american-heroes-channel-live-stream/"
                                class="list-group-item list-group-item-dark">American Heroes Channel</a>
                                                    <a href="/tv/animal-planet-live-stream/"
                                class="list-group-item list-group-item-dark">Animal Planet</a>
                                                    <a href="/tv/bbc-america-live-stream/"
                                class="list-group-item list-group-item-dark">BBC America</a>
                                                    <a href="/tv/bbc-world-news-hd-live-stream/"
                                class="list-group-item list-group-item-dark">BBC World News HD</a>
                                                    <a href="/tv/bet-live-stream/"
                                class="list-group-item list-group-item-dark">BET</a>
                                                    <a href="/tv/bet-her-live-stream/"
                                class="list-group-item list-group-item-dark">BET Her</a>
                                                    <a href="/tv/big-ten-network-live-stream/"
                                class="list-group-item list-group-item-dark">Big Ten Network</a>
                                                    <a href="/tv/bloomberg-tv-live-stream/"
                                class="list-group-item list-group-item-dark">Bloomberg TV</a>
                                                    <a href="/tv/boomerang-live-stream/"
                                class="list-group-item list-group-item-dark">Boomerang</a>
                                                    <a href="/tv/bravo-live-stream/"
                                class="list-group-item list-group-item-dark">Bravo</a>
                                                    <a href="/tv/cartoon-network-live-stream/"
                                class="list-group-item list-group-item-dark">Cartoon Network</a>
                                                    <a href="/tv/cbs-sports-network-live-stream/"
                                class="list-group-item list-group-item-dark">CBS Sports Network</a>
                                                    <a href="/tv/cinemax-live-stream/"
                                class="list-group-item list-group-item-dark">Cinemax</a>
                                                    <a href="/tv/cmt-live-stream/"
                                class="list-group-item list-group-item-dark">CMT</a>
                                                    <a href="/tv/cnbc-live-stream/"
                                class="list-group-item list-group-item-dark">CNBC</a>
                                                    <a href="/tv/cnn-live-stream/"
                                class="list-group-item list-group-item-dark">CNN</a>
                                                    <a href="/tv/comedy-central-live-stream/"
                                class="list-group-item list-group-item-dark">Comedy Central</a>
                                                    <a href="/tv/cooking-channel-live-stream/"
                                class="list-group-item list-group-item-dark">Cooking Channel</a>
                                                    <a href="/tv/crime-investigation-hd-live-stream/"
                                class="list-group-item list-group-item-dark">Crime &amp; Investigation HD</a>
                                                    <a href="/tv/cspan-live-stream/"
                                class="list-group-item list-group-item-dark">CSPAN</a>
                                                    <a href="/tv/cspan-2-live-stream/"
                                class="list-group-item list-group-item-dark">CSPAN 2</a>
                                                    <a href="/tv/destination-america-live-stream/"
                                class="list-group-item list-group-item-dark">Destination America</a>
                                                    <a href="/tv/discovery-live-stream/"
                                class="list-group-item list-group-item-dark">Discovery</a>
                                                    <a href="/tv/discovery-family-channel-live-stream/"
                                class="list-group-item list-group-item-dark">Discovery Family Channel</a>
                                                    <a href="/tv/discovery-life-live-stream/"
                                class="list-group-item list-group-item-dark">Discovery Life</a>
                                                    <a href="/tv/disney-channel-east-live-stream/"
                                class="list-group-item list-group-item-dark">Disney Channel (East)</a>
                                                    <a href="/tv/disney-junior-live-stream/"
                                class="list-group-item list-group-item-dark">Disney Junior</a>
                                                    <a href="/tv/disney-xd-live-stream/"
                                class="list-group-item list-group-item-dark">Disney XD</a>
                                                    <a href="/tv/e-live-stream/"
                                class="list-group-item list-group-item-dark">E!</a>
                                                    <a href="/tv/espn-live-stream/"
                                class="list-group-item list-group-item-dark">ESPN</a>
                                                    <a href="/tv/espn2-live-stream/"
                                class="list-group-item list-group-item-dark">ESPN2</a>
                                                    <a href="/tv/espnews-live-stream/"
                                class="list-group-item list-group-item-dark">ESPNews</a>
                                                    <a href="/tv/espnu-live-stream/"
                                class="list-group-item list-group-item-dark">ESPNU</a>
                                                    <a href="/tv/food-network-live-stream/"
                                class="list-group-item list-group-item-dark">Food Network</a>
                                                    <a href="/tv/fox-business-network-live-stream/"
                                class="list-group-item list-group-item-dark">Fox Business Network</a>
                                                    <a href="/tv/fox-news-channel-live-stream/"
                                class="list-group-item list-group-item-dark">FOX News Channel</a>
                                                    <a href="/tv/fox-sports-1-live-stream/"
                                class="list-group-item list-group-item-dark">FOX Sports 1</a>
                                                    <a href="/tv/fox-sports-2-live-stream/"
                                class="list-group-item list-group-item-dark">FOX Sports 2</a>
                                                    <a href="/tv/freeform-live-stream/"
                                class="list-group-item list-group-item-dark">Freeform</a>
                                                    <a href="/tv/fuse-hd-live-stream/"
                                class="list-group-item list-group-item-dark">Fuse HD</a>
                                                    <a href="/tv/fx-live-stream/"
                                class="list-group-item list-group-item-dark">FX</a>
                                                    <a href="/tv/fx-movie-live-stream/"
                                class="list-group-item list-group-item-dark">FX Movie</a>
                                                    <a href="/tv/fxx-live-stream/"
                                class="list-group-item list-group-item-dark">FXX</a>
                                                    <a href="/tv/fyi-live-stream/"
                                class="list-group-item list-group-item-dark">FYI</a>
                                                    <a href="/tv/golf-channel-live-stream/"
                                class="list-group-item list-group-item-dark">Golf Channel</a>
                                                    <a href="/tv/hallmark-live-stream/"
                                class="list-group-item list-group-item-dark">Hallmark</a>
                                                    <a href="/tv/hallmark-drama-hd-live-stream/"
                                class="list-group-item list-group-item-dark">Hallmark Drama HD</a>
                                                    <a href="/tv/hallmark-movies-mysteries-hd-live-stream/"
                                class="list-group-item list-group-item-dark">Hallmark Movies &amp; Mysteries HD</a>
                                                    <a href="/tv/hbo-2-east-live-stream/"
                                class="list-group-item list-group-item-dark">HBO 2 East</a>
                                                    <a href="/tv/hbo-comedy-hd-live-stream/"
                                class="list-group-item list-group-item-dark">HBO Comedy HD</a>
                                                    <a href="/tv/hbo-east-live-stream/"
                                class="list-group-item list-group-item-dark">HBO East</a>
                                                    <a href="/tv/hbo-family-east-live-stream/"
                                class="list-group-item list-group-item-dark">HBO Family East</a>
                                                    <a href="/tv/hbo-signature-live-stream/"
                                class="list-group-item list-group-item-dark">HBO Signature</a>
                                                    <a href="/tv/hbo-zone-hd-live-stream/"
                                class="list-group-item list-group-item-dark">HBO Zone HD</a>
                                                    <a href="/tv/hgtv-live-stream/"
                                class="list-group-item list-group-item-dark">HGTV</a>
                                                    <a href="/tv/history-live-stream/"
                                class="list-group-item list-group-item-dark">History</a>
                                                    <a href="/tv/hln-live-stream/"
                                class="list-group-item list-group-item-dark">HLN</a>
                                                    <a href="/tv/ifc-live-stream/"
                                class="list-group-item list-group-item-dark">IFC</a>
                                                    <a href="/tv/investigation-discovery-live-stream/"
                                class="list-group-item list-group-item-dark">Investigation Discovery</a>
                                                    <a href="/tv/ion-television-east-hd-live-stream/"
                                class="list-group-item list-group-item-dark">ION Television East HD</a>
                                                    <a href="/tv/lifetime-live-stream/"
                                class="list-group-item list-group-item-dark">Lifetime</a>
                                                    <a href="/tv/lmn-live-stream/"
                                class="list-group-item list-group-item-dark">LMN</a>
                                                    <a href="/tv/logo-live-stream/"
                                class="list-group-item list-group-item-dark">Logo</a>
                                                    <a href="/tv/metv-toons-live-stream/"
                                class="list-group-item list-group-item-dark">MeTV Toons</a>
                                                    <a href="/tv/mlb-network-live-stream/"
                                class="list-group-item list-group-item-dark">MLB Network</a>
                                                    <a href="/tv/moremax-live-stream/"
                                class="list-group-item list-group-item-dark">MoreMAX</a>
                                                    <a href="/tv/motortrend-hd-live-stream/"
                                class="list-group-item list-group-item-dark">MotorTrend HD</a>
                                                    <a href="/tv/moviemax-live-stream/"
                                class="list-group-item list-group-item-dark">MovieMAX</a>
                                                    <a href="/tv/msnbc-live-stream/"
                                class="list-group-item list-group-item-dark">MSNBC</a>
                                                    <a href="/tv/mtv-live-stream/"
                                class="list-group-item list-group-item-dark">MTV</a>
                                                    <a href="/tv/nat-geo-wild-live-stream/"
                                class="list-group-item list-group-item-dark">Nat Geo WILD</a>
                                                    <a href="/tv/national-geographic-live-stream/"
                                class="list-group-item list-group-item-dark">National Geographic</a>
                                                    <a href="/tv/nba-tv-live-stream/"
                                class="list-group-item list-group-item-dark">NBA TV</a>
                                                    <a href="/tv/newsmax-tv/"
                                class="list-group-item list-group-item-dark">Newsmax TV</a>
                                                    <a href="/tv/nfl-network-live-stream/"
                                class="list-group-item list-group-item-dark">NFL Network</a>
                                                    <a href="/tv/nfl-red-zone/"
                                class="list-group-item list-group-item-dark">NFL Red Zone</a>
                                                    <a href="/tv/nhl-network-live-stream/"
                                class="list-group-item list-group-item-dark">NHL Network</a>
                                                    <a href="/tv/nick-jr-live-stream/"
                                class="list-group-item list-group-item-dark">Nick Jr.</a>
                                                    <a href="/tv/nickelodeon-east-live-stream/"
                                class="list-group-item list-group-item-dark">Nickelodeon East</a>
                                                    <a href="/tv/nicktoons-live-stream/"
                                class="list-group-item list-group-item-dark">Nicktoons</a>
                                                    <a href="/tv/outdoor-channel-live-stream/"
                                class="list-group-item list-group-item-dark">Outdoor Channel</a>
                                                    <a href="/tv/own-live-stream/"
                                class="list-group-item list-group-item-dark">OWN</a>
                                                    <a href="/tv/oxygen-true-crime-live-stream/"
                                class="list-group-item list-group-item-dark">Oxygen True Crime</a>
                                                    <a href="/tv/pbs-13-wnet-new-york-live-stream/"
                                class="list-group-item list-group-item-dark">PBS 13 (WNET) New York</a>
                                                    <a href="/tv/reelzchannel-live-stream/"
                                class="list-group-item list-group-item-dark">ReelzChannel</a>
                                                    <a href="/tv/science-live-stream/"
                                class="list-group-item list-group-item-dark">Science</a>
                                                    <a href="/tv/sec-network-live-stream/"
                                class="list-group-item list-group-item-dark">SEC Network</a>
                                                    <a href="/tv/showtime-e-live-stream/"
                                class="list-group-item list-group-item-dark">Showtime (E)</a>
                                                    <a href="/tv/showtime-2-live-stream/"
                                class="list-group-item list-group-item-dark">SHOWTIME 2</a>
                                                    <a href="/tv/starz-east-live-stream/"
                                class="list-group-item list-group-item-dark">STARZ East</a>
                                                    <a href="/tv/sundancetv-hd-live-stream/"
                                class="list-group-item list-group-item-dark">SundanceTV HD</a>
                                                    <a href="/tv/syfy-live-stream/"
                                class="list-group-item list-group-item-dark">SYFY</a>
                                                    <a href="/tv/tbs-live-stream/"
                                class="list-group-item list-group-item-dark">TBS</a>
                                                    <a href="/tv/tcm-live-stream/"
                                class="list-group-item list-group-item-dark">TCM</a>
                                                    <a href="/tv/teennick-live-stream/"
                                class="list-group-item list-group-item-dark">TeenNick</a>
                                                    <a href="/tv/telemundo-east-live-stream/"
                                class="list-group-item list-group-item-dark">Telemundo East</a>
                                                    <a href="/tv/tennis-channel-live-stream/"
                                class="list-group-item list-group-item-dark">Tennis Channel</a>
                                                    <a href="/tv/the-cw-live-stream/"
                                class="list-group-item list-group-item-dark">The CW (WPIX New York)</a>
                                                    <a href="/tv/the-movie-channel-east-live-stream/"
                                class="list-group-item list-group-item-dark">The Movie Channel East</a>
                                                    <a href="/tv/the-weather-channel-live-stream/"
                                class="list-group-item list-group-item-dark">The Weather Channel</a>
                                                    <a href="/tv/tlc-live-stream/"
                                class="list-group-item list-group-item-dark">TLC</a>
                                                    <a href="/tv/tnt-live-stream/"
                                class="list-group-item list-group-item-dark">TNT</a>
                                                    <a href="/tv/travel-channel-live-stream/"
                                class="list-group-item list-group-item-dark">Travel Channel</a>
                                                    <a href="/tv/trutv-live-stream/"
                                class="list-group-item list-group-item-dark">truTV</a>
                                                    <a href="/tv/tv-one-hd-live-stream/"
                                class="list-group-item list-group-item-dark">TV One HD</a>
                                                    <a href="/tv/universal-kids-live-stream/"
                                class="list-group-item list-group-item-dark">Universal Kids</a>
                                                    <a href="/tv/univision-east-live-stream/"
                                class="list-group-item list-group-item-dark">Univision East</a>
                                                    <a href="/tv/usa-network-live-stream/"
                                class="list-group-item list-group-item-dark">USA Network</a>
                                                    <a href="/tv/vh1-live-stream/"
                                class="list-group-item list-group-item-dark">VH1</a>
                                                    <a href="/tv/vice-live-stream/"
                                class="list-group-item list-group-item-dark">VICE</a>
                                                    <a href="/tv/wabc-new-york-abc-east-live-stream/"
                                class="list-group-item list-group-item-dark">WABC (New York) ABC East</a>
                                                    <a href="/tv/wcbs-new-york-cbs-east-live-stream/"
                                class="list-group-item list-group-item-dark">WCBS (New York) CBS East</a>
                                                    <a href="/tv/we-tv-live-stream/"
                                class="list-group-item list-group-item-dark">WE tv</a>
                                                    <a href="/tv/wnbc-new-york-nbc-east-live-stream/"
                                class="list-group-item list-group-item-dark">WNBC (New York) NBC East</a>
                                                    <a href="/tv/wnyw-new-york-fox-east-live-stream/"
                                class="list-group-item list-group-item-dark">WNYW (New York) FOX East</a>
                                            </ol>
                </div>
                <div id="schedule" class="tab-pane fade show active">
                    <div class="list-group list-custom" id="listgroup">
                    </div>
                </div>
            </div>
            <script>
    $(document).ready(function () {
        const listgroup = $('#listgroup');
        const jsonUrl = "https://thetvapp.to/json/9200009045.json";

        $.getJSON(jsonUrl, function (mydata) {
            const now = new Date();
            let index = 0;

            // Find the first program that is currently airing
            for (let i = 0; i < mydata.length; i++) {
                const startTime = new Date(mydata[i].startTime * 1000);
                const endTime = new Date(mydata[i].endTime * 1000);

                if (startTime <= now && now <= endTime) {
                    index = i;
                    break;
                }
            }

            let currentDay = null;

            // Display programs from the current index onwards
            for (let i = index; i < mydata.length; i++) {
                const program = mydata[i];
                const startTime = new Date(program.startTime * 1000);
                const airTime = startTime.toLocaleString('en-US', {
                    hour: 'numeric',
                    minute: 'numeric',
                    hour12: true
                });

                // Check if the day has changed and insert a date header
                const programDay = startTime.toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
                if (currentDay !== programDay) {
                    currentDay = programDay;
                    const dateHeader = `
                        <div class="list-group-item list-group-item-action" style="background: linear-gradient(45deg, #6a11cb, #2575fc); color: white; font-weight: bold;">
                            ${currentDay}
                        </div>
                    `;
                    listgroup.append(dateHeader);
                }

                // Combine title and episodeTitle if available
                let title = program.title;
                if (program.episodeTitle) {
                    title += ` - ${program.episodeTitle}`;
                }

                const programItem = `
                    <div class="list-group-item list-group-item-action list-group-item-dark">
                        ${airTime}: ${title}
                    </div>
                `;

                listgroup.append(programItem);
            }
        }).fail(function () {
            console.error("Failed to load JSON data.");
        });
    });
</script>
        </div>
    </div>

    </div>

	<!-- Bootstrap JS -->
	<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4" crossorigin="anonymous"></script>


   <script>
        $("span").each(function() {
            var startTime = new Date($(this).text());
            if (startTime) {
                $(this).text(startTime.toLocaleString("en-US", {
                    dateStyle: "short",
                    timeStyle: "long",
                    hour12: true
                })).css({
                    "background-color": "lightblue",
                    "font-size": "100%",
                    "color": "black",
                    "font-weight": "400"
                });
            }
        });
    </script>


    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-KDJRXWJ0P0"></script>
    <script>
        window.dataLayer = window.dataLayer || [];

        function gtag() {
            dataLayer.push(arguments);
        }
        gtag('js', new Date());

        gtag('config', 'G-KDJRXWJ0P0');
    </script>

</body>

</html>

```
</details>

1. The stream title: `A&amp;E` (selector `body > div > div > div.col-lg-8 > div.mb-3 > h2`, inner text)
2. The encrypted text (selector `#encrypted-text`, attr `data`)
3. The TV guide JSON url: `https://thetvapp.to/json/9200009045.json` (regex `https:\/\/thetvapp\.to\/json\/\d+\.json`)

For live events like sporting events, the page contains three things:

<details>
<summary>Live Event HTML</summary>

```html
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="description" content="Watch Brooklyn Nets vs. Orlando Magic live stream - TheTVApp.to">
    <meta name="keywords" content="Brooklyn Nets vs. Orlando Magic, NBA">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="7M2cR7uwhShXrTWinYYf2mKqgC8azyPyG8e9YOKg">
        <title>Brooklyn Nets vs. Orlando Magic live stream - TheTVApp.to</title>
    <!-- Scripts -->
    <link rel="preload" as="style" href="https://thetvapp.to/build/assets/app-24fac727.css" /><link rel="modulepreload" href="https://thetvapp.to/build/assets/app-bcabbe84.js" /><link rel="stylesheet" href="https://thetvapp.to/build/assets/app-24fac727.css" /><script type="module" src="https://thetvapp.to/build/assets/app-bcabbe84.js"></script>
	<!-- Bootstrap CSS -->
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous">
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet">

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js"></script>
        <script type="text/javascript" src="https://thetvapp.to/player/v/8.29.0/jwplayer.js"></script>

    <script type="text/javascript">
        jwplayer.key = "uoW6qHjBL3KNudxKVnwa3rt5LlTakbko9e6aQ6VUyKQ=";
    </script>
</head>

<body>
    <!-- navbar -->
    <header class="p-3 text-bg-dark">
        <div class="container">
            <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                <a class="navbar-brand" href="/">
                    <img src="https://thetvapp.to/img/TheTVApp.svg" width="80%" class="d-inline-block align-top"
                        alt="">
                </a>

                <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                    <li><a href="/" class="nav-link px-2 text-white">Home</a></li>
                    <li><a href="https://thetvapp.to/tv" class="nav-link px-2 text-white">Live TV</a></li>
                    <li><a href="https://thetvapp.to/nba" class="nav-link px-2 text-white">NBA</a></li>
                    <li><a href="https://thetvapp.to/mlb" class="nav-link px-2 text-white">MLB</a></li>
                    <li><a href="https://thetvapp.to/nhl" class="nav-link px-2 text-white">NHL</a></li>
                    <li><a href="https://thetvapp.to/nfl" class="nav-link px-2 text-white">NFL</a></li>
                    <li><a href="https://thetvapp.to/ncaaf" class="nav-link px-2 text-white">NCAAF</a></li>
                    <li><a href="https://thetvapp.to/ncaab" class="nav-link px-2 text-white">NCAAB</a>
                    </li>
                    <li><a href="https://thetvapp.to/soccer" class="nav-link px-2 text-white">Soccer</a></li>
                    <li><a href="https://thetvapp.to/ppv" class="nav-link px-2 text-white">PPV</a></li>
                </ul>



                <div class="text-end">
                    <a href="https://thetvapp.to/contact"><button type="button"
                            class="btn btn-outline-light me-2">Contact Us</button></a>


					<a href="https://discord.gg/keZ3RwNq4b" target="_blank"><button type="button" class="btn btn-success">Join Our Discord</button></a>
                </div>

				<div class="m-2">
                    <a href="https://thetvsub.to" target="_blank" class="btn btn-primary">Subscribe Now</a>
                </div>
            </div>
        </div>
    </header>
    <!-- navbar -->

    <div class="container my-4">

            <div class="row">
        <div class="col-lg-8">

                            <div class="alert alert-primary" role="alert">
                    Subscribe now to install our app on smart TVs and streaming devices.
                </div>

            <div class="mb-3">
                <h1>Brooklyn Nets vs. Orlando Magic</h1>
                <div>
                    Start Time:
                    <span class="startTime"></span>
                </div>
            </div>

                            <div id="encrypted-text" data="vGV0zLG6Db92ZH50bMX0khTrbG50yc9itKXaUqLIHxPueT5oTPjfgUT1QK90w2hgan1MWAF3YHy3e2PuiCdeUqOIQdgvW2K6AoaQFSgeRq5iwoPcVpDpMLWCZjxDv05KHpypTEcTTWg9MU=="></div>
                <button id="loadVideoBtn" class="video-button"></button>
                <div id="my-jwplayer" class="mt-3"></div>

            <script>
                var startTime = new Date('2024-12-29T20:40:00Z');
                if (startTime) {
                    $(".startTime").text(startTime.toLocaleString("en-US", {
                        dateStyle: "short",
                        timeStyle: "long",
                        hour12: true
                    }));
                }
            </script>

        </div>
        <div class="col-lg-4">
                    </div>

    </div>

    </div>

	<!-- Bootstrap JS -->
	<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4" crossorigin="anonymous"></script>


   <script>
        $("span").each(function() {
            var startTime = new Date($(this).text());
            if (startTime) {
                $(this).text(startTime.toLocaleString("en-US", {
                    dateStyle: "short",
                    timeStyle: "long",
                    hour12: true
                })).css({
                    "background-color": "lightblue",
                    "font-size": "100%",
                    "color": "black",
                    "font-weight": "400"
                });
            }
        });
    </script>


    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-KDJRXWJ0P0"></script>
    <script>
        window.dataLayer = window.dataLayer || [];

        function gtag() {
            dataLayer.push(arguments);
        }
        gtag('js', new Date());

        gtag('config', 'G-KDJRXWJ0P0');
    </script>

</body>

</html>
```
</details>

1. The stream title: `Brooklyn Nets vs. Orlando Magic` (selector `body > div > div > div.col-lg-8 > div.mb-3 > h1`, inner text)
2. The encrypted text (selector `#encrypted-text`, attr `data`)
3. The start time in ISO 8601 format: `2024-12-29T20:40:00Z` (regex `/\d{4}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01])T(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d(?:\.\d+)?(?:Z|[+-](?:[01]\d|2[0-3]):[0-5]\d)/gm`)

**The two types of channels can be discriminated pretty easily by checking to see if the TV guide regex hits or if the start time regex hits.**

#### Analysis & Decryption

The "encrypted text" looks like this: `vGV0zLG6Db92XYNafNczyGDegBSxoL8wgIeuZ0AEVTJtvF9agnXnlX5hH3T4T3Ozu2NxAZ9JE3fCv3r3TlbpigFZKJSgX2YCMioWx1ZZwIaXcsj6BOTNLKGJwWQHf0m3ZmkvHBMJbIPzgk09`.

Using the `decryptText` function in `deobfuscated.js` yields a URL like the following: `https://v11.thetvapp.to/hls/WNBCDT1/index.m3u8?token=OHcwUkwwbllzcVdDZWdGbzRkUExZRnhzQ1R0bEpCVkIwNGZLcExEaw==`

This `m3u8` playlist can be pretty easily streamed by a client like VLC or transcoded by a tool like `ffmpeg`.

To get the TV guide, we can just make a GET request to the JSON URL and parse the response.

### 3. TV Guide JSON

The structure of the TV guide is pretty simple: it's just a JSON list with a 1D object.

```json
[
    ...
    {
        "title": "Rachael Ray's Rebuild",
        "startTime": 1735480800,
        "endTime": 1735482600,
        "episodeTitle": null
    },
    ...
]
```
The `startTime` and `endTime` are Unix timestamps.

## Key Rotation

Every day, the parent HTML page will be updated with a new JS file that contains a new decryption key.

Although it is obfuscated using `javascript-obfuscator` (a.k.a. `obfuscator.io), a well documented obfuscation engine, it is not possible to extract the key using exclusively static analysis (i.e. deobfuscation attempts).

However, we live in the era of AI! Any LLM with a wide enough context window can extract the key fairly easily. In the `llm/` folder, you will see the JSON schema and system prompt I use to extract the key from the JavaScript.
