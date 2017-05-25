
const state  = {
  platforms: {
    ARCADE_MAME: {
      FriendlyName: 'Arcade',
      PlatformID: 'ARCADE_MAME',
      Metadata: {
        platform_type: 'Arcade'
      },
      FileTypes: {
        '.zip': 'application/x-romfile-arcade+zip'
      },
      BiosFiles: [],
      MaximumInputs: 8
    },
    ATARI_2600: {
      FriendlyName: 'Atari 2600',
      PlatformID: 'ATARI_2600',
      Metadata: {
        platform_releasedate: '11/9/1977',
        platform_shortname: '2600',
        platform_company: 'Atari',
        platform_type: 'Home'
      },
      FileTypes: {
        '.a26': 'application/x-romfile-2600',
        '.bin': 'application/x-romfile-2600',
        '.rom': 'application/x-romfile-2600'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    ATARI_5200: {
      FriendlyName: 'Atari 5200 SuperSystem',
      PlatformID: 'ATARI_5200',
      Metadata: {
        platform_releasedate: '1/3/1982',
        platform_shortname: '5200',
        platform_company: 'Atari',
        platform_type: 'Home'
      },
      FileTypes: {
        '.a52': 'application/x-romfile-5200',
        '.bin': 'application/x-romfile-5200'
      },
      BiosFiles: [
        '5200.rom'
      ],
      MaximumInputs: 4
    },
    ATARI_7800: {
      FriendlyName: 'Atari 7800 ProSystem',
      PlatformID: 'ATARI_7800',
      Metadata: {
        platform_releasedate: '1/6/1986',
        platform_shortname: '7800',
        platform_company: 'Atari',
        platform_type: 'Home'
      },
      FileTypes: {
        '.a78': 'application/x-romfile-7800',
        '.bin': 'application/x-romfile-7800'
      },
      BiosFiles: [
        '7800 BIOS (U).rom',
        '7800 BIOS (E).rom'
      ],
      MaximumInputs: 2
    },
    ATARI_JAGUAR: {
      FriendlyName: 'Atari Jaguar CD',
      PlatformID: 'ATARI_JAGUAR',
      Metadata: {
        platform_releasedate: '21/09/1995',
        platform_shortname: 'Jag CD',
        platform_company: 'Atari',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-jaguar-iso9660',
        '.cdi': 'application/x-romfile-jaguar-diskjuggler',
        '.ccd': 'application/x-romfile-jaguar-clonecd-control',
        '.img': 'application/x-romfile-jaguar-clonecd-rawimage',
        '.sub': 'application/x-romfile-jaguar-clonecd-subchannel',
        '.bin': 'application/x-romfile-jaguar-rawimage',
        '.cue': 'application/x-romfile-jaguar-cuesheet',
        '.nrg': 'application/x-romfile-jaguar-nero'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    ATARI_LYNX: {
      FriendlyName: 'Atari Lynx',
      PlatformID: 'ATARI_LYNX',
      Metadata: {
        platform_releasedate: '11/10/1989',
        platform_shortname: 'Lynx',
        platform_company: 'Atari',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.lnx': 'application/x-romfile-lynx',
        '.bin': 'application/x-romfile-lynx'
      },
      BiosFiles: [
        'lynxboot.img'
      ],
      MaximumInputs: 1
    },
    BANDAI_WS: {
      FriendlyName: 'Bandai WonderSwan',
      PlatformID: 'BANDAI_WS',
      Metadata: {
        platform_releasedate: '04/03/1999',
        platform_shortname: 'WS',
        platform_company: 'Bandai',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.ws': 'application/x-romfile-ws',
        '.bin': 'application/x-romfile-ws'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    BANDAI_WSC: {
      FriendlyName: 'Bandai WonderSwan Color',
      PlatformID: 'BANDAI_WSC',
      Metadata: {
        platform_releasedate: '09/12/2000',
        platform_shortname: 'WSC',
        platform_company: 'Bandai',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.wsc': 'application/x-romfile-wsc',
        '.bin': 'application/x-romfile-wsc'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    COLECO_CV: {
      FriendlyName: 'ColecoVision',
      PlatformID: 'COLECO_CV',
      Metadata: {
        platform_releasedate: '1/8/1982',
        platform_shortname: 'CV',
        platform_company: 'Coleco',
        platform_type: 'Home'
      },
      FileTypes: {
        '.rom': 'application/x-romfile-cv',
        '.col': 'application/x-romfile-cv'
      },
      BiosFiles: [
        'ColecoVision BIOS (1982).rom',
        'ColecoVision BIOS (1982).col'
      ],
      MaximumInputs: 4
    },
    GCE_VECTREX: {
      FriendlyName: 'Vectrex',
      PlatformID: 'GCE_VECTREX',
      Metadata: {
        platform_releasedate: '1/8/1982',
        platform_shortname: 'Vectrex',
        platform_company: 'General Computer',
        platform_type: 'Home'
      },
      FileTypes: {
        '.vec': 'application/x-romfile-vec',
        '.bin': 'application/x-romfile-vec',
        '.vol': 'application/x-romfile-vec-overlay-vol',
        '.pcx': 'application/x-romfile-vec-overlay-pcx'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    MATTEL_INT: {
      FriendlyName: 'Intellivision',
      PlatformID: 'MATTEL_INT',
      Metadata: {
        platform_releasedate: '1/8/1982',
        platform_shortname: 'INT',
        platform_company: 'Mattel Electronics',
        platform_type: 'Home'
      },
      FileTypes: {
        '.int': 'application/x-romfile-int',
        '.bin': 'application/x-romfile-int'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    NEC_PCFX: {
      FriendlyName: 'PC-FX',
      PlatformID: 'NEC_PCFX',
      Metadata: {
        platform_releasedate: '23/11/1994',
        platform_shortname: 'PC-FX',
        platform_company: 'NEC',
        platform_type: 'Home'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-pcfx-rawimage',
        '.iso': 'application/x-romfile-pcfx-iso9660',
        '.cue': 'application/x-romfile-pcfx-cuesheet',
        '.ccd': 'application/x-romfile-pcfx-clonecd-control',
        '.img': 'application/x-romfile-pcfx-clonecd-rawimage',
        '.sub': 'application/x-romfile-pcfx-clonecd-subchannel'
      },
      BiosFiles: [
        'pcfx.rom'
      ],
      MaximumInputs: 2
    },
    NEC_SGFX: {
      FriendlyName: 'PC Engine SuperGrafx',
      PlatformID: 'NEC_SGFX',
      Metadata: {
        platform_releasedate: '1/11/1989',
        platform_shortname: 'SuperGrafx',
        platform_company: 'NEC',
        platform_type: 'Home'
      },
      FileTypes: {
        '.pce': 'application/x-romfile-sgfx',
        '.bin': 'application/x-romfile-sgfx'
      },
      BiosFiles: [
        'syscard1.pce',
        'syscard2.pce',
        'syscard3.pce'
      ],
      MaximumInputs: 2
    },
    NEC_TG16: {
      FriendlyName: 'TurboGrafx-16',
      PlatformID: 'NEC_TG16',
      Metadata: {
        platform_releasedate: '30/10/1986',
        platform_shortname: 'TG16',
        platform_jp_name: 'PC Engine',
        platform_company: 'NEC',
        platform_type: 'Home'
      },
      FileTypes: {
        '.pce': 'application/x-romfile-tg16',
        '.bin': 'application/x-romfile-tg16'
      },
      BiosFiles: [
        'syscard1.pce',
        'syscard2.pce',
        'syscard3.pce'
      ],
      MaximumInputs: 2
    },
    NEC_TGCD: {
      FriendlyName: 'TurboGrafx-CD',
      PlatformID: 'NEC_TGCD',
      Metadata: {
        platform_releasedate: '30/10/1986',
        platform_shortname: 'TGCD',
        platform_jp_name: 'PC Engine CD',
        platform_company: 'NEC',
        platform_type: 'Home'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-tgcd-rawimage',
        '.iso': 'application/x-romfile-tgcd-iso9660',
        '.cue': 'application/x-romfile-tgcd-cuesheet'
      },
      BiosFiles: [
        'syscard1.pce',
        'syscard2.pce',
        'syscard3.pce'
      ],
      MaximumInputs: 2
    },
    NINTENDO_3DS: {
      FriendlyName: 'Nintendo 3DS',
      PlatformID: 'NINTENDO_3DS',
      Metadata: {
        platform_releasedate: '26/02/2011',
        platform_shortname: '3DS',
        platform_company: 'Nintendo',
        platform_type: 'Handheld',
        platform_emulator_dev: 'Citra'
      },
      FileTypes: {
        '.3ds': 'application/x-romfile-3ds-encrypted',
        '.3dsx': 'application/x-romfile-3ds-homebrew',
        '.cia': 'application/x-romfile-3ds-cia'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    NINTENDO_FDS: {
      FriendlyName: 'Family Computer Disk System',
      PlatformID: 'NINTENDO_FDS',
      Metadata: {
        platform_releasedate: '18/10/1985',
        platform_shortname: 'FDS',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.fds': 'application/x-romfile-fds',
        '.bin': 'application/x-romfile-fds'
      },
      BiosFiles: [
        'disksys.rom'
      ],
      MaximumInputs: 2
    },
    NINTENDO_GB: {
      FriendlyName: 'Game Boy',
      PlatformID: 'NINTENDO_GB',
      Metadata: {
        platform_releasedate: '21/04/1989',
        platform_shortname: 'GB',
        platform_company: 'Nintendo',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.gb': 'application/x-romfile-gb',
        '.bin': 'application/x-romfile-gb'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    NINTENDO_GBA: {
      FriendlyName: 'Game Boy Advanced',
      PlatformID: 'NINTENDO_GBA',
      Metadata: {
        platform_releasedate: '21/10/2001',
        platform_shortname: 'GBA',
        platform_company: 'Nintendo',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.gba': 'application/x-romfile-gba',
        '.bin': 'application/x-romfile-gba'
      },
      BiosFiles: [
        'gba_bios.bin'
      ],
      MaximumInputs: 1
    },
    NINTENDO_GBC: {
      FriendlyName: 'Game Boy Color',
      PlatformID: 'NINTENDO_GBC',
      Metadata: {
        platform_releasedate: '21/10/1998',
        platform_shortname: 'GBC',
        platform_company: 'Nintendo',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.gbc': 'application/x-romfile-gbc',
        '.bin': 'application/x-romfile-gbc'
      },
      BiosFiles: [
        'gbc_bios.bin'
      ],
      MaximumInputs: 1
    },
    NINTENDO_GCN: {
      FriendlyName: 'Nintendo GameCube',
      PlatformID: 'NINTENDO_GCN',
      Metadata: {
        platform_releasedate: '14/9/2001',
        platform_shortname: 'GCN',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-gcn-iso9660',
        '.gcm': 'application/x-romfile-gcn-iso9660',
        '.dol': 'application/x-romfile-gcn-dol',
        '.elf': 'application/x-romfile-gcn-homebrew'
      },
      BiosFiles: [
        'IPL.bin',
        'IPL_E.bin',
        'IPL_U.bin',
        'IPL_J.bin'
      ],
      MaximumInputs: 4
    },
    NINTENDO_N64: {
      FriendlyName: 'Nintendo 64',
      PlatformID: 'NINTENDO_N64',
      Metadata: {
        platform_releasedate: '23/06/1996',
        platform_shortname: 'N64',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.64': 'application/x-romfile-n64-indeterminate',
        '.n64': 'application/x-romfile-n64-littleendian',
        '.z64': 'application/x-romfile-n64-bigendian',
        '.u64': 'application/x-romfile-n64-indeterminate',
        '.v64': 'application/x-romfile-n64-byteswapped',
        '.rom': 'application/x-romfile-n64-indeterminate',
        '.bin': 'application/x-romfile-n64-indeterminate'
      },
      BiosFiles: [],
      MaximumInputs: 4
    },
    NINTENDO_N64DD: {
      FriendlyName: 'Nintendo 64 DD',
      PlatformID: 'NINTENDO_N64DD',
      Metadata: {
        platform_releasedate: '23/06/1996',
        platform_shortname: 'N64DD',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.d64': 'application/x-romfile-n64-dd',
        '.dd': 'application/x-romfile-n64-dd',
        '.bin': 'application/x-romfile-n64-dd'
      },
      BiosFiles: [],
      MaximumInputs: 4
    },
    NINTENDO_NDS: {
      FriendlyName: 'Nintendo DS',
      PlatformID: 'NINTENDO_NDS',
      Metadata: {
        platform_releasedate: '21/11/2004',
        platform_shortname: 'NDS',
        platform_company: 'Nintendo',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.nds': 'application/x-romfile-nds',
        '.gba': 'application/x-romfile-nds-slot2',
        '.ids': 'application/x-romfile-nds'
      },
      BiosFiles: [
        'firmware.bin',
        'biosnds7.rom',
        'biosnds9.rom'
      ],
      MaximumInputs: 1
    },
    NINTENDO_NES: {
      FriendlyName: 'Nintendo Entertainment System',
      PlatformID: 'NINTENDO_NES',
      Metadata: {
        platform_releasedate: '18/10/1985',
        platform_shortname: 'NES',
        platform_jp_name: 'Family Computer',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.nes': 'application/x-romfile-nes-ines',
        '.bin': 'application/x-romfile-nes-ines',
        '.unf': 'application/x-romfile-nes-unif'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    NINTENDO_SNES: {
      FriendlyName: 'Super Nintendo Entertainment System',
      PlatformID: 'NINTENDO_SNES',
      Metadata: {
        platform_releasedate: '21/11/1990',
        platform_shortname: 'SNES',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.sfc': 'application/x-romfile-snes-headerless',
        '.smc': 'application/x-romfile-snes-magiccard',
        '.swc': 'application/x-romfile-snes-wildcard',
        '.bin': 'application/x-romfile-snes-headerless',
        '.fig': 'application/x-romfile-snes-profighter'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    NINTENDO_VB: {
      FriendlyName: 'Virtual Boy',
      PlatformID: 'NINTENDO_VB',
      Metadata: {
        platform_releasedate: '21/07/1995',
        platform_shortname: 'VB',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.vb': 'application/x-romfile-vb',
        '.bin': 'application/x-romfile-vb'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    NINTENDO_WII: {
      FriendlyName: 'Wii',
      PlatformID: 'NINTENDO_WII',
      Metadata: {
        platform_releasedate: '2/12/2006',
        platform_shortname: 'Wii',
        platform_company: 'Nintendo',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-wii-iso9660',
        '.wbfs': 'application/x-romfile-wbfs',
        '.ciso': 'application/x-romfile-wii-iso9660-compressed',
        '.elf': 'application/x-romfile-wii-homebrew',
        '.dol': 'application/x-romfile-wii-dol',
        '.wad': 'application/x-romfile-wii-wad'
      },
      BiosFiles: [
        'dsp_rom.bin',
        'dsp_coef.bin'
      ],
      MaximumInputs: 8
    },
    NINTENDO_WIIU: {
      FriendlyName: 'WiiU',
      PlatformID: 'NINTENDO_WIIU',
      Metadata: {
        platform_releasedate: '18/11/2012',
        platform_shortname: 'WiiU',
        platform_company: 'Nintendo',
        platform_type: 'Home',
        platform_emulator_dev: 'cemu'
      },
      FileTypes: {
        '.wud': 'application/x-romfile-wiiu-wud'
      },
      BiosFiles: [],
      MaximumInputs: 5
    },
    PANASONIC_3DO: {
      FriendlyName: '3DO Interactive Multiplayer',
      PlatformID: 'PANASONIC_3DO',
      Metadata: {
        platform_releasedate: '04/10/1993',
        platform_shortname: '3DO',
        platform_company: 'Panasonic',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-3do-iso9660',
        '.ccd': 'application/x-romfile-3do-clonecd-control',
        '.img': 'application/x-romfile-3do-clonecd-rawimage',
        '.sub': 'application/x-romfile-3do-clonecd-subchannel',
        '.bin': 'application/x-romfile-3do-rawimage',
        '.cue': 'application/x-romfile-3do-cuesheet'
      },
      BiosFiles: [
        '3DO-FZ-10.bin'
      ],
      MaximumInputs: 8
    },
    SEGA_32X: {
      FriendlyName: 'Sega 32X',
      PlatformID: 'SEGA_32X',
      Metadata: {
        platform_releasedate: '21/11/1994',
        platform_shortname: '32X',
        platform_company: 'Sega',
        platform_type: 'Addon'
      },
      FileTypes: {
        '.rom': 'application/x-romfile-32x',
        '.32x': 'application/x-romfile-32x',
        '.ccd': 'application/x-romfile-32xcd-clonecd-control',
        '.img': 'application/x-romfile-32xcd-clonecd-rawimage',
        '.sub': 'application/x-romfile-32xcd-clonecd-subchannel',
        '.bin': 'application/x-romfile-32xcd-rawimage',
        '.cue': 'application/x-romfile-32xcd-cuesheet'
      },
      BiosFiles: [
        '32X_G_BIOS.BIN',
        '32X_S_BIOS.BIN',
        '32X_M_BIOS.BIN'
      ],
      MaximumInputs: 2
    },
    SEGA_CD: {
      FriendlyName: 'Sega CD',
      PlatformID: 'SEGA_CD',
      Metadata: {
        platform_releasedate: '12/12/1991',
        platform_shortname: 'SCD',
        platform_company: 'Sega',
        platform_type: 'Addon'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-scd-iso9660',
        '.ccd': 'application/x-romfile-scd-clonecd-control',
        '.img': 'application/x-romfile-scd-clonecd-rawimage',
        '.sub': 'application/x-romfile-scd-clonecd-subchannel',
        '.bin': 'application/x-romfile-scd-rawimage',
        '.cue': 'application/x-romfile-scd-cuesheet'
      },
      BiosFiles: [
        'bios_CD_U.bin',
        'bios_CD_E.bin',
        'bios_CD_J.bin',
        'us_scd1_9210.bin',
        'eu_mcd1_9210.bin',
        'jp_mcd1_9112.bin'
      ],
      MaximumInputs: 2
    },
    SEGA_DC: {
      FriendlyName: 'Sega Dreamcast',
      PlatformID: 'SEGA_DC',
      Metadata: {
        platform_releasedate: '27/11/1998',
        platform_shortname: 'DC',
        platform_company: 'Sega',
        platform_type: 'Home'
      },
      FileTypes: {
        '.dc': 'application/x-romfile-dc-rawimage',
        '.iso': 'application/x-romfile-dc-iso9660',
        '.bin': 'application/x-romfile-dc-rawimage',
        '.raw': 'application/x-romfile-dc-rawimage',
        '.elf': 'application/x-romfile-dc-elf',
        '.cdi': 'application/x-romfile-dc-diskjuggler',
        '.gdi': 'application/x-romfile-dc-rawimage',
        '.ccd': 'application/x-romfile-dc-clonecd-control',
        '.img': 'application/x-romfile-dc-clonecd-rawimage',
        '.sub': 'application/x-romfile-dc-clonecd-subchannel',
        '.nrg': 'application/x-romfile-dc-nero'
      },
      BiosFiles: [
        'dc_boot.bin',
        'dc_flash.bin'
      ],
      MaximumInputs: 4
    },
    SEGA_GEN: {
      FriendlyName: 'Sega Genesis',
      PlatformID: 'SEGA_GEN',
      Metadata: {
        platform_releasedate: '29/10/1988',
        platform_shortname: 'Genesis',
        platform_eu_name: 'Mega Drive',
        platform_jp_name: 'Mega Drive',
        platform_company: 'Sega',
        platform_type: 'Home'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-gen',
        '.smd': 'application/x-romfile-gen',
        '.rom': 'application/x-romfile-gen',
        '.md': 'application/x-romfile-gen',
        '.gen': 'application/x-romfile-gen'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    SEGA_GG: {
      FriendlyName: 'Sega Game Gear',
      PlatformID: 'SEGA_GG',
      Metadata: {
        platform_releasedate: '06/10/1990',
        platform_shortname: 'GG',
        platform_company: 'Sega',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.gg': 'application/x-romfile-gg',
        '.sms': 'application/x-romfile-gg',
        '.bin': 'application/x-romfile-gg'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    SEGA_SAT: {
      FriendlyName: 'Sega Saturn',
      PlatformID: 'SEGA_SAT',
      Metadata: {
        platform_releasedate: '29/10/1988',
        platform_shortname: 'Saturn',
        platform_company: 'Sega',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-sat-iso9660',
        '.ccd': 'application/x-romfile-sat-clonecd-control',
        '.img': 'application/x-romfile-sat-clonecd-rawimage',
        '.sub': 'application/x-romfile-sat-clonecd-subchannel',
        '.bin': 'application/x-romfile-sat-rawimage',
        '.cue': 'application/x-romfile-sat-cuesheet'
      },
      BiosFiles: [
        'saturn_bios.bin'
      ],
      MaximumInputs: 2
    },
    SEGA_SG1000: {
      FriendlyName: 'Sega SG-1000',
      PlatformID: 'SEGA_SG1000',
      Metadata: {
        platform_releasedate: '15/07/1983',
        platform_shortname: 'SG-1000',
        platform_company: 'Sega',
        platform_type: 'Home'
      },
      FileTypes: {
        '.sg': 'application/x-romfile-sg1000',
        '.sc': 'application/x-romfile-sc1000',
        '.bin': 'application/x-romfile-sg1000'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    SEGA_SMS: {
      FriendlyName: 'Sega Master System',
      PlatformID: 'SEGA_SMS',
      Metadata: {
        platform_releasedate: '1985',
        platform_shortname: 'SMS',
        platform_company: 'Sega',
        platform_type: 'Home'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-sms',
        '.sms': 'application/x-romfile-sms'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    SNK_NG: {
      FriendlyName: 'Neo Geo',
      PlatformID: 'SNK_NG',
      Metadata: {
        platform_releasedate: '26/04/1991',
        platform_shortname: 'NG',
        platform_company: 'SNK',
        platform_type: 'Home'
      },
      FileTypes: {
        '.zip': 'application/x-romfile-arcade-neogeo'
      },
      BiosFiles: [
        'neogeo.zip'
      ],
      MaximumInputs: 2
    },
    SNK_NGCD: {
      FriendlyName: 'Neo Geo CD',
      PlatformID: 'SNK_NGCD',
      Metadata: {
        platform_releasedate: '01/09/1994',
        platform_shortname: 'NGCD',
        platform_company: 'SNK',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-ngcd-iso9660',
        '.bin': 'application/x-romfile-ngcd-rawimage',
        '.elf': 'application/x-romfile-ngcd-elf',
        '.cdi': 'application/x-romfile-ngcd-diskjuggler',
        '.gdi': 'application/x-romfile-ngcd-rawimage',
        '.ccd': 'application/x-romfile-ngcd-clonecd-control',
        '.img': 'application/x-romfile-ngcd-clonecd-rawimage',
        '.sub': 'application/x-romfile-ngcd-clonecd-subchannel',
        '.nrg': 'application/x-romfile-ngcd-nero',
        '.zip': 'application/x-romfile-ngcd+zip'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    SNK_NGP: {
      FriendlyName: 'Neo Geo Pocket',
      PlatformID: 'SNK_NGP',
      Metadata: {
        platform_releasedate: '28/10/1998',
        platform_shortname: 'NGP',
        platform_company: 'SNK',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.ngp': 'application/x-romfile-ngp',
        '.bin': 'application/x-romfile-ngp'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    SNK_NGPC: {
      FriendlyName: 'Neo Geo Pocket Color',
      PlatformID: 'SNK_NGPC',
      Metadata: {
        platform_releasedate: '16/03/1999',
        platform_shortname: 'NGPC',
        platform_company: 'SNK',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.ngc': 'application/x-romfile-ngpc',
        '.bin': 'application/x-romfile-ngpc'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    SONY_PS2: {
      FriendlyName: 'PlayStation 2',
      PlatformID: 'SONY_PS2',
      Metadata: {
        platform_releasedate: '04/03/2000',
        platform_shortname: 'PS2',
        platform_company: 'Sony',
        platform_type: 'Home'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-ps2-rawimage',
        '.cue': 'application/x-romfile-ps2-cuesheet',
        '.iso': 'application/x-romfile-ps2-iso9660',
        '.ps2': 'application/x-romfile-ps2-rawimage',
        '.ccd': 'application/x-romfile-ps2-clonecd-control',
        '.img': 'application/x-romfile-ps2-clonecd-rawimage',
        '.sub': 'application/x-romfile-ps2-clonecd-subchannel',
        '.elf': 'application/x-romfile-ps2-elf'
      },
      BiosFiles: [
        'scph-10000.bin',
        'scph-30003.bin',
        'scph-30004.bin',
        'scph-39001.bin',
        'scph-39004.bin',
        'scph-50003.bin',
        'scph-70000.bin',
        'scph-70004.bin',
        'scph-70012.bin',
        'scph-90006.bin'
      ],
      MaximumInputs: 4
    },
    SONY_PS3: {
      FriendlyName: 'PlayStation 3',
      PlatformID: 'SONY_PS3',
      Metadata: {
        platform_releasedate: '11/11/2006',
        platform_shortname: 'PS3',
        platform_company: 'Sony',
        platform_emulator_dev: 'rpcs3',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-ps3',
        '.elf': 'application/x-romfile-ps3-elf',
        '.pbp': 'application/x-romfile-ps3-pbp'
      },
      BiosFiles: [],
      MaximumInputs: 4
    },
    SONY_PS4: {
      FriendlyName: 'PlayStation 4',
      PlatformID: 'SONY_PS4',
      Metadata: {
        platform_releasedate: '15/11/2013',
        platform_shortname: 'PS4',
        platform_company: 'Sony',
        platform_emulator_dev: 'None',
        platform_type: 'Home'
      },
      FileTypes: {
        '.iso': 'application/x-romfile-ps4',
        '.elf': 'application/x-romfile-ps4-elf',
        '.pbp': 'application/x-romfile-ps4-pbp'
      },
      BiosFiles: [],
      MaximumInputs: 4
    },
    SONY_PSP: {
      FriendlyName: 'Playstation Portable',
      PlatformID: 'SONY_PSP',
      Metadata: {
        platform_releasedate: '12/12/2004',
        platform_shortname: 'PSP',
        platform_company: 'Sony',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-psp-rawimage',
        '.iso': 'application/x-romfile-psp-iso9660',
        '.pbp': 'application/x-romfile-psp-eboot',
        '.cso': 'application/x-romfile-psp-iso9660-compressed',
        '.ciso': 'application/x-romfile-psp-iso9660-compressed',
        '.dax': 'application/x-romfile-psp-iso9660-compressed'
      },
      BiosFiles: [],
      MaximumInputs: 2
    },
    SONY_PSV: {
      FriendlyName: 'PlayStation Vita',
      PlatformID: 'SONY_PSV',
      Metadata: {
        platform_releasedate: '17/12/2011',
        platform_shortname: 'PSV',
        platform_company: 'Sony',
        platform_emulator_dev: 'None',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.bin': 'application/x-romfile-psv',
        '.iso': 'application/x-romfile-psv',
        '.pbp': 'application/x-romfile-psv-pbp',
        '.cso': 'application/x-romfile-psv-compressed',
        '.ciso': 'application/x-romfile-psv-compressed'
      },
      BiosFiles: [],
      MaximumInputs: 1
    },
    SONY_PSX: {
      FriendlyName: 'PlayStation',
      PlatformID: 'SONY_PSX',
      Metadata: {
        platform_releasedate: '03/12/1994',
        platform_shortname: 'psx',
        platform_company: 'Sony',
        platform_type: 'Handheld'
      },
      FileTypes: {
        '.m3u': 'application/x-romfile-psx-playlist',
        '.toc': 'application/x-romfile-psx-cdrdao',
        '.bin': 'application/x-romfile-psx-rawimage',
        '.cue': 'application/x-romfile-psx-cuesheet',
        '.iso': 'application/x-romfile-psx-iso9660',
        '.psx': 'application/x-romfile-psx-rawimage',
        '.ccd': 'application/x-romfile-psx-clonecd-control',
        '.img': 'application/x-romfile-psx-clonecd-rawimage',
        '.sub': 'application/x-romfile-psx-clonecd-subchannel',
        '.pbp': 'application/x-romfile-psx-eboot',
        '.znx': 'application/x-romfile-psx-iso9660-compressed',
        '.exe': 'application/x-romfile-psx-exe'
      },
      BiosFiles: [
        'scph1001.bin',
        'scph5500.bin',
        'scph5501.bin',
        'scph5502.bin'
      ],
      MaximumInputs: 2
    }
  },
  games: [
    {
      Metadata: {
        game_platform: {
          Key: 'game_platform',
          Value: 'NINTENDO_NES',
          Guid: '62af9a06-4deb-518a-80e5-fac94d556c5a',
          Record: 'bda629d3-2907-453b-8159-e9b17e056c15'
        },
        game_title: {
          Key: 'game_title',
          Value: 'Game',
          Guid: 'e715a056-fad7-55e1-8401-0aa459f0a5ea',
          Record: 'bda629d3-2907-453b-8159-e9b17e056c15'
        }
      },
      Guid: 'bda629d3-2907-453b-8159-e9b17e056c15',
      PlatformId: 'NINTENDO_NES',
      Title: 'Game',
      Files: [
        {
          Metadata: {
            file_linkedrecord: {
              Key: 'file_linkedrecord',
              Value: 'bda629d3-2907-453b-8159-e9b17e056c15',
              Guid: '8d282347-d73a-542d-a88a-dca799bad463',
              Record: '9fcd9379-c5a3-4607-80dd-4deacc38750a'
            }
          },
          Guid: '9fcd9379-c5a3-4607-80dd-4deacc38750a',
          MimeType: 'application/x-snes',
          FilePath: '::virtual\test.snes',
          Record: 'bda629d3-2907-453b-8159-e9b17e056c15'
        }
      ]
    },
    {
      Metadata: {
        game_platform: {
          Key: 'game_platform',
          Value: 'NINTENDO_SNES',
          Guid: '09faad8c-1453-5173-a0d8-fa2b265925e5',
          Record: '837515c1-346a-4a97-a1d3-dbdb4ad393f0'
        },
        game_title: {
          Key: 'game_title',
          Value: 'test',
          Guid: '07059a1d-5685-5e37-9126-543a36b13230',
          Record: '837515c1-346a-4a97-a1d3-dbdb4ad393f0'
        }
      },
      Guid: '837515c1-346a-4a97-a1d3-dbdb4ad393f0',
      PlatformId: 'NINTENDO_SNES',
      Title: 'test',
      Files: []
    }
  ],
  router: {
    location: {
      pathname: '/',
      search: '',
      hash: ''
    }
  }
}

export default state