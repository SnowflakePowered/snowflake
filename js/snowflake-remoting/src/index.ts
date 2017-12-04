import { Games } from './remoting/Games'
import { Stone } from './remoting/Stone'
import { Emulators } from './remoting/Emulators'

export default class Snowflake {
  private _games: Games
  private _stone: Stone
  private _emulators: Emulators
  constructor(rootUrl: string = 'http://localhost:9696') {
    this._games = new Games(rootUrl)
    this._stone = new Stone(rootUrl)
    this._emulators = new Emulators(rootUrl)

  }

  public get games(): Games {
    return this._games
  }

  public get stone(): Stone {
    return this._stone
  }

  public get emulators(): Emulators {
    return this._emulators
  }
}
