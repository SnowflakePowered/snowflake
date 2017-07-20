import { AsyncActionCreators, Success, Failure } from 'typescript-fsa'

type SuccessPayload<P, S> = {
  type: string,
  payload: Success<P, S>
}

type FailedPayload<P, S> = {
  type: string,
  payload: Failure<P, S>
}

export const successDispatch = <P, S>(action: AsyncActionCreators<P, S, any>,
                                      payload: S, params?: P): SuccessPayload<P | void, S> => {
  return {
    type: action.done.type,
    payload: {
      params: params,
      result: payload
    }
  }
}

export const failedDispatch = <P, S extends Error>(action: AsyncActionCreators<P, S, any>,
                              error: S, params?: P): FailedPayload<P | void, S> => {
  return {
    type: action.failed.type,
    payload: {
      params: params,
      error: error
    }
  }
}
