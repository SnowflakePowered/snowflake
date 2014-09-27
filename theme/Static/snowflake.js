window.snowflake = {
	api: {
		games: [
    {
        "UUID": "klGNT8l9m0ypDI8IGr0TcA",
        "FileName": "dummysmb.nes",
        "Settings": {},
        "PlatformId": "NINTENDO_NES",
        "Name": "Super Mario Bros.",
        "MediaStore": {
            "Images": {
                "SectionName": "Images",
                "MediaStoreItems": {
                    "fanart_0": "140-1.jpg",
                    "fanart_1": "140-10.jpg",
                    "fanart_2": "140-11.jpg",
                    "fanart_3": "140-12.jpg",
                    "fanart_4": "140-13.jpg",
                    "fanart_5": "140-14.jpg",
                    "fanart_6": "140-2.jpg",
                    "fanart_7": "140-3.jpg",
                    "fanart_8": "140-4.jpg",
                    "fanart_9": "140-5.jpg",
                    "fanart_10": "140-7.jpg",
                    "fanart_11": "140-8.jpg",
                    "fanart_12": "140-9.jpg",
                    "screenshot_0": "140-1.jpg",
                    "boxart_front": "140-1.jpg",
                    "boxart_back": "140-2.jpg"
                }
            },
            "Audio": {
                "SectionName": "Audio",
                "MediaStoreItems": {}
            },
            "Video": {
                "SectionName": "Video",
                "MediaStoreItems": {}
            },
            "Resources": {
                "SectionName": "Resources",
                "MediaStoreItems": {}
            },
            "MediaStoreKey": "game.Super_Mario_Bros.klGNT8l9m0ypDI8IGr0TcA"
        },
        "Metadata": {
            "game_description": "The player takes the role of Mario, or in the case of a second player, Mario's brother Luigi. The ultimate objective is to race through the worlds of the Mushroom Kingdom, evade or eliminate Bowser's forces, and save the Princess",
            "game_title": "Super Mario Bros.",
            "game_releasedate": "09/13/1985",
            "game_publisher": "Nintendo",
            "game_developer": "Nintendo"
        }
    },
	 {
        "UUID": "klGNT8l9m0ypDI8IGr0TcA",
        "FileName": "dummysmb.nes",
        "Settings": {},
        "PlatformId": "NINTENDO_NES",
        "Name": "Super Luigi Bros.",
        "MediaStore": {
            "Images": {
                "SectionName": "Images",
                "MediaStoreItems": {
                    "fanart_0": "140-1.jpg",
                    "fanart_1": "140-10.jpg",
                    "fanart_2": "140-11.jpg",
                    "fanart_3": "140-12.jpg",
                    "fanart_4": "140-13.jpg",
                    "fanart_5": "140-14.jpg",
                    "fanart_6": "140-2.jpg",
                    "fanart_7": "140-3.jpg",
                    "fanart_8": "140-4.jpg",
                    "fanart_9": "140-5.jpg",
                    "fanart_10": "140-7.jpg",
                    "fanart_11": "140-8.jpg",
                    "fanart_12": "140-9.jpg",
                    "screenshot_0": "140-1.jpg",
                    "boxart_front": "140-1.jpg",
                    "boxart_back": "140-2.jpg"
                }
            },
            "Audio": {
                "SectionName": "Audio",
                "MediaStoreItems": {}
            },
            "Video": {
                "SectionName": "Video",
                "MediaStoreItems": {}
            },
            "Resources": {
                "SectionName": "Resources",
                "MediaStoreItems": {}
            },
            "MediaStoreKey": "game.Super_Mario_Bros.klGNT8l9m0ypDI8IGr0TcA"
        },
        "Metadata": {
            "game_description": "The player takes the role of Mario, or in the case of a second player, Mario's brother Luigi. The ultimate objective is to race through the worlds of the Mushroom Kingdom, evade or eliminate Bowser's forces, and save the Princess",
            "game_title": "Super Mario Bros.",
            "game_releasedate": "09/13/1985",
            "game_publisher": "Nintendo",
            "game_developer": "Nintendo"
        }
    }
],
		platforms: {
			"NINTENDO_NES": {
				"FileExtensions": [
					".nes"
				],
				"Defaults": {
					"Scraper": "Scraper.TheGamesDB",
					"Identifier": "Identifier.ClrMameProDat",
					"Emulator": "Emulator.RetroArch"
				},	
				"PlatformId": "NINTENDO_NES",
				"Name": "Nintendo Entertainment System",
				"Images": {
					"img_platform_logo": "http://upload.wikimedia.org/wikipedia/commons/0/0d/NES_logo.svg",
					"img_platform_productshot": "http://upload.wikimedia.org/wikipedia/commons/thumb/8/82/NES-Console-Set.jpg/1280px-NES-Console-Set.jpg"
				},
				"Metadata": {
					"platform_shortname": "NES",
					"platform_company": "Nintendo",
					"platform_releasedate": "18/10/1985"
					
				}
			},
			"NINTENDO_SNES": {
				"FileExtensions": [
					".sfc",
					".smc"
				],
				"Defaults": {
					"Scraper": "Scraper.TheGamesDB",
					"Identifier": "Identifier.ClrMameProDat",
					"Emulator": "Emulator.RetroArch"
				},
				"PlatformId": "NINTENDO_SNES",
				"Name": "Super Nintendo Entertainment System",
				"Images": {
					"img_platform_logo": "http://upload.wikimedia.org/wikipedia/commons/2/2c/SNES_logo.svg",
					"img_platform_productshot": "http://upload.wikimedia.org/wikipedia/commons/3/31/SNES-Mod1-Console-Set.jpg"
				},
				"Metadata": {
					"platform_shortname": "SNES",
					"platform_company": "Nintendo",
					"platform_releasedate": "21/11/1990"
				}
			}
		}
	},
	theme: {
		heap: {
			debug: {
				gameModel: new Array()
			}
		}
	}
};