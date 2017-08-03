import { AsyncActionCreators, ActionCreator, Success, Failure } from 'typescript-fsa'

export type SuccessPayload<P, S> = {
  type: string,
  payload: Success<P, S>
}

export type FailedPayload<P, S> = {
  type: string,
  payload: Failure<P, S>
}

export type SyncPayload<S> = {
  type: string,
  payload: S
}

export const syncDispatch = <S>(action: ActionCreator<S>, payload: S): SyncPayload<S> => {
  return {
    type: action.type,
    payload: payload
  }
}

export const successDispatch = <P, S>(action: AsyncActionCreators<P, S, {}>,
                                payload: S, params?: P): SuccessPayload<P | void, S> => {
  return {
    type: action.done.type,
    payload: {
      params: params,
      result: payload
    }
  }
}

export const failedDispatch = <P, S extends Error>(action: AsyncActionCreators<P, S, {}>,
                              error: S, params?: P): FailedPayload<P | void, S> => {
  return {
    type: action.failed.type,
    payload: {
      params: params,
      error: error
    }
  }
}
