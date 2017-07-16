import { Games } from "./remoting/Games";
import { Stone } from "./remoting/Stone";
export default class Snowflake {
    private _games;
    private _stone;
    constructor(rootUrl?: string);
    readonly games: Games;
    readonly stone: Stone;
}
