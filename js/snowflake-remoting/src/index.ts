import { Games } from './remoting/Games'
import { Stone } from './remoting/Stone'

export default class Snowflake {
  private _games: Games
  private _stone: Stone
  constructor(rootUrl: string = 'http://localhost:9696') {
    this._games = new Games(rootUrl)
    this._stone = new Stone(rootUrl)

  }

  public get games(): Games {
    return this._games
  }

  public get stone(): Stone {
    return this._stone
  }
}
