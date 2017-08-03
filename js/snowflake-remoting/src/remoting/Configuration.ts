import * as Immutable from 'seamless-immutable'
import { request, Response, Service } from './Remoting'

export type ConfigurationOptionType = 'integer' | 'boolean' | 'decimal' | 'selection' | 'string'

export interface ConfigurationValue {
  Value: number | boolean | string,
  Guid: string
}

export interface ConfigurationDescriptor {
  Default: number | boolean | string,
  Description: string,
  DisplayName: string,
  Simple: boolean,
  Type: ConfigurationOptionType,
  Min?: number,
  Max?: number,
  Increment?: number
}

export interface ConfigurationSelection {
  DisplayName: string,
  Private: boolean
}

export interface ConfigurationOption {
  Value: ConfigurationValue,
  Descriptor: ConfigurationDescriptor,
  Selection?: ConfigurationSelection
}

export interface ConfigurationSectionDescriptor {
  Description: string,
  DisplayName: string,
  SectionName: string
}

export interface ConfigurationSection {
  Configuration: { [OptionName: string]: ConfigurationOption }
  Descriptor: ConfigurationSectionDescriptor
}

export interface ConfigurationCollection {
  [SectionName: string]: ConfigurationSection
}

export class Configuration extends Service {
  constructor(rootUrl: string) {
    super(rootUrl, 'configuration')
  }
}
