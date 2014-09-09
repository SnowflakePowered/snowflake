window.snowflake = {
	api: {
		games: [
			{"PlatformId":"NINTENDO_NES","Name":"Super Mario Bros.","Images":{"Fanarts":[["fanart_0","http://thegamesdb.net/banners/fanart/original/140-1.jpg"],["fanart_1","http://thegamesdb.net/banners/fanart/original/140-10.jpg"],["fanart_2","http://thegamesdb.net/banners/fanart/original/140-11.jpg"],["fanart_3","http://thegamesdb.net/banners/fanart/original/140-12.jpg"],["fanart_4","http://thegamesdb.net/banners/fanart/original/140-13.jpg"],["fanart_5","http://thegamesdb.net/banners/fanart/original/140-14.jpg"],["fanart_6","http://thegamesdb.net/banners/fanart/original/140-2.jpg"],["fanart_7","http://thegamesdb.net/banners/fanart/original/140-3.jpg"],["fanart_8","http://thegamesdb.net/banners/fanart/original/140-4.jpg"],["fanart_9","http://thegamesdb.net/banners/fanart/original/140-5.jpg"],["fanart_10","http://thegamesdb.net/banners/fanart/original/140-7.jpg"],["fanart_11","http://thegamesdb.net/banners/fanart/original/140-8.jpg"],["fanart_12","http://thegamesdb.net/banners/fanart/original/140-9.jpg"]],"Screenshots":[["screenshot_0","http://thegamesdb.net/banners/screenshots/140-1.jpg"]],"Boxarts":{"img_boxart_front":["boxart_front","http://thegamesdb.net/banners/boxart/original/front/140-1.jpg"],"img_boxart_back":["boxart_back","http://thegamesdb.net/banners/boxart/original/back/140-2.jpg"]},"CachePath":"C:\\Users\\Ronny\\AppData\\Roaming\\Snowflake\\data\\imagescache","ImagesID":"Z1dEK6qCe0uW5Bj8nVSbKA"},"Metadata":{"game_description":"The player takes the role of Mario, or in the case of a second player, Mario's brother Luigi. The ultimate objective is to race through the worlds of the Mushroom Kingdom, evade or eliminate Bowser's forces, and save the Princess","game_title":"Super Mario Bros.","game_releasedate":"09/13/1985","game_publisher":"Nintendo","game_developer":"Nintendo"},"UUID":"mevWllPUq0uv6rkFIrqJ1A","FileName":"dummysmb.nes","Settings":{}},
			{"PlatformId":"NINTENDO_NES","Name":"Contra?","Images":{"Fanarts":[["fanart_0","http://thegamesdb.net/banners/fanart/original/140-1.jpg"],["fanart_1","http://thegamesdb.net/banners/fanart/original/140-10.jpg"],["fanart_2","http://thegamesdb.net/banners/fanart/original/140-11.jpg"],["fanart_3","http://thegamesdb.net/banners/fanart/original/140-12.jpg"],["fanart_4","http://thegamesdb.net/banners/fanart/original/140-13.jpg"],["fanart_5","http://thegamesdb.net/banners/fanart/original/140-14.jpg"],["fanart_6","http://thegamesdb.net/banners/fanart/original/140-2.jpg"],["fanart_7","http://thegamesdb.net/banners/fanart/original/140-3.jpg"],["fanart_8","http://thegamesdb.net/banners/fanart/original/140-4.jpg"],["fanart_9","http://thegamesdb.net/banners/fanart/original/140-5.jpg"],["fanart_10","http://thegamesdb.net/banners/fanart/original/140-7.jpg"],["fanart_11","http://thegamesdb.net/banners/fanart/original/140-8.jpg"],["fanart_12","http://thegamesdb.net/banners/fanart/original/140-9.jpg"]],"Screenshots":[["screenshot_0","http://thegamesdb.net/banners/screenshots/140-1.jpg"]],"Boxarts":{"img_boxart_front":["boxart_front","http://www.7hunters.com/blog/wp-content/uploads/2013/03/postercontranes_thumb.jpg"],"img_boxart_back":["boxart_back","http://thegamesdb.net/banners/boxart/original/back/140-2.jpg"]},"CachePath":"C:\\Users\\Ronny\\AppData\\Roaming\\Snowflake\\data\\imagescache","ImagesID":"Z1dEK6qCe0uW5Bj8nVSbKA"},"Metadata":{"game_description":"Not super Mario Bros","game_title":"Contra?","game_releasedate":"09/13/1985","game_publisher":"Nintendo","game_developer":"Nintendo"},"UUID":"_mevWllPUq0uv6rkFIrqJ1A","FileName":"dummysmb.nes","Settings":{}}
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