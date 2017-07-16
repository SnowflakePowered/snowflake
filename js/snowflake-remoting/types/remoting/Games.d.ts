/// <reference types="seamless-immutable" />
import * as Immutable from "seamless-immutable";
import { Service } from "./Remoting";
import { Platform } from "./Stone";
export interface Game {
    Files: File[];
    Guid: string;
    Metadata: {
        [key: string]: Metadata;
    };
    PlatformId: string;
    Title: string;
}
export interface File {
    FilePath: string;
    Guid: string;
    Metadata: {
        [key: string]: Metadata;
    };
    MimeType: string;
    Record: string;
}
export interface Metadata {
    Guid: string;
    Key: string;
    Record: string;
    Value: string;
}
export declare class Games extends Service {
    constructor(rootUrl: string);
    getGames: () => Promise<Iterable<Game>>;
    getGame: (uuid: string) => Promise<Game>;
    createGame: (title: string, platform: Platform) => Promise<Game>;
    createFile: (game: Game, path: string, mimetype: string) => Promise<Game & Immutable.ImmutableObject<Game>>;
}
