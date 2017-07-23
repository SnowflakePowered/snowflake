import * as Immutable from 'immutable'

class ConfigurationKeyProxyHandler implements ProxyHandler<ConfigurationKey> {
  private immutableBacking: Immutable.Map<string, any>
  constructor (gameGuid: string, emulatorName: string, profileName: string) {
    this.immutableBacking = Immutable.fromJS({ gameGuid: gameGuid, emulatorName: emulatorName, profileName: profileName })
  }

  public get (target, property, receiver) {
    if (this.immutableBacking.contains(property)) return this.immutableBacking.get(property)
    return this.immutableBacking[property]
  }

  public set (target, property, value, receiver) {
    return false
  }
}

export interface ConfigurationKey {
  gameGuid: string,
  emulatorName: string,
  profileName: string
}

export const ConfigurationKey = (gameGuid: string, emulatorName: string, profileName: string): ConfigurationKey => {
  const handler = new ConfigurationKeyProxyHandler(gameGuid, emulatorName, profileName)
  return new Proxy<ConfigurationKey>({gameGuid: gameGuid, emulatorName: emulatorName, profileName: profileName}, handler)
}
