import * as Immutable from 'immutable'

class ConfigurationKeyProxyHandler implements ProxyHandler<ConfigurationKey> {
  private immutableBacking: Immutable.Map<string, any>
  constructor (gameGuid: string, emulatorName: string, profileName: string) {
    this.immutableBacking = Immutable.fromJS({ gameGuid: gameGuid, emulatorName: emulatorName, profileName: profileName })
  }

  public get (target, property, receiver) {
    return this.immutableBacking.get(property) || this.immutableBacking[property]
  }

  public set (target, property, value, receiver) {
    return false
  }
}

/**
 * Used to access a ConfigurationCollection object.
 */
export type ConfigurationKey = {
  gameGuid: string,
  emulatorName: string,
  profileName: string
}
/**
 * Used to create a ConfigurationKey.
 *
 * Do not manually create one from a plain JavaScript object, it will not have proper value semantics.
 *
 * This function creates a Proxy that is backed by Immutable for proper value semantics when using
 * as a Map key.
 *
 * @param gameGuid The GUID of the game.
 * @param emulatorName The name of the emulator this collection is for.
 * @param profileName The name of the collection profile.
 *
 */
export const ConfigurationKey = (gameGuid: string, emulatorName: string, profileName: string): ConfigurationKey => {
  const handler = new ConfigurationKeyProxyHandler(gameGuid, emulatorName, profileName)
  return new Proxy<ConfigurationKey>({gameGuid: gameGuid, emulatorName: emulatorName, profileName: profileName}, handler)
}
