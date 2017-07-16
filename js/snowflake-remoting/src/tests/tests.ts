import { expect } from 'chai'
import Snowflake from '../index'
import { Games } from '../remoting/Games'
import { Stone } from '../remoting/Stone'

describe('Initialization', () => {
  it('Should initialize properly', () => {
    const snowflake = new Snowflake()
    expect(snowflake).to.not.be.undefined
    expect(snowflake.games).to.not.be.undefined
    expect(snowflake.stone).to.not.be.undefined
  })
})

describe('Stone', () => {
  it('Should get Platforms properly', async () => {
    const snowflake = new Snowflake()
    const platforms = await snowflake.stone.getPlatforms()
    expect(platforms).to.not.be.undefined
    expect(Array.from(platforms.entries())).to.not.be.empty
  })
  it('Platforms should contain NINTENDO_NES', async () => {
    const snowflake = new Snowflake()
    const platforms = await snowflake.stone.getPlatforms()
    const platformKeys = Array.from(platforms.keys())
    expect(platformKeys).to.contain('NINTENDO_NES')
  })
})

describe('Games', () => {
  it('Should get Games properly', async () => {
    const snowflake = new Snowflake()
    const games = await snowflake.games.getGames()
    expect(games).to.not.be.undefined
    expect(Array.from(games)).to.not.be.empty
  })
})
