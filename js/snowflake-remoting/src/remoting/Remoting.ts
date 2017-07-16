import 'whatwg-fetch'

export interface Response<T> {
  Response: T
  Error: Error
}

export interface Error {
  Message?: string
}

export class Service {
  protected rootUrl: string
  protected serviceName: string

  protected constructor (rootUrl: string, serviceName: string) {
    this.rootUrl = rootUrl
    this.serviceName = serviceName
  }

  protected getServiceUrl = (...path: string[]): string => {
    return [this.rootUrl, this.serviceName, ...path].join('/')
  }
}

const toHttpVerb = (verb: Verb): string => {
  switch (verb) {
    case 'Create':
      return 'POST'
    case 'Read':
      return 'GET'
    case 'Delete':
      return 'DELETE'
    case 'Update':
      return 'PUT'
    default:
      return 'GET'
  }
}

export type Verb = 'Create' | 'Read' | 'Delete' | 'Update'

export const request = async <T> (url: string, payload: any = '', verb: Verb = 'Read'): Promise<Response<T>> => {
  if (typeof payload !== 'string') { payload = JSON.stringify(payload) }
  if (verb === 'Read' || verb === 'Delete') { payload = undefined }
  const result = await fetch(url, {
    body: payload,
    method: toHttpVerb(verb),
    mode: 'cors'
  })
  if (result.ok) {
    let json = await result.json()
    return { Response: json.Response, Error: json.Error }
  }
  return { Response: null, Error: { Message: 'Unable to resolve promise.' } }
}
