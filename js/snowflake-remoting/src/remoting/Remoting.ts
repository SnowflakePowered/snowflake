export type ErrorCode = 400 | 401 | 403 | 404 | 405 | 415 | 500 | 501 | 502 | 503
export type OKCode = 200 | 201 | 202 | 204 | 205

export interface Response<T> {
  Response: T
  Status: {
    Message: string,
    Type: string,
    Code: OKCode | ErrorCode
  }
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
    return { Response: json.Response, Status: json.Status }
  }
  return { Response: null, Status: { Message: 'Unable to resolve promise.', Type: 'ErrorRequest', Code: 400 } }
}
