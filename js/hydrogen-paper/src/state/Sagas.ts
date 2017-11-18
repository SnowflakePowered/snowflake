import { SagaIterator } from 'redux-saga'
import { takeEvery, call, put, select } from 'redux-saga/effects'
import Snowflake, {
  Platform,
  Game,
  ConfigurationCollection,
  ConfigurationValue
} from 'snowflake-remoting'

import * as Actions from './Actions'
import {
  successDispatch,
  failedDispatch,
  syncDispatch,
  SyncPayload
} from './ResultDispatches'

import * as Selectors from './Selectors'
import { ConfigurationKey } from 'support/ConfigurationKey'

function* refreshPlatformsWorker (snowflake: Snowflake): SagaIterator {
  try {
    const platforms: { [platformId: string]: Platform } = yield call(snowflake.stone.getPlatforms)
    yield put(successDispatch(Actions.refreshPlatforms, platforms))
  } catch (e) {
    yield put(failedDispatch(Actions.refreshPlatforms, e))
  }
}

function* refreshGamesWorker (snowflake: Snowflake): SagaIterator {
  try {
    const games: { [gameGuid: string]: Game } = yield call(snowflake.games.getGames)
    yield put(successDispatch(Actions.refreshGames, games))
  } catch (e) {
    yield put(failedDispatch(Actions.refreshGames, e))
  }
}

function* refreshActiveState (): SagaIterator {
  const query: { [key: string]: string } = yield select(Selectors.queryParamsSelector)
  yield put(syncDispatch(Actions.setActivePlatform, query['platform']))
  yield put(syncDispatch(Actions.setActiveGame, query['game']))
  yield put(syncDispatch(Actions.setActiveConfigurationProfile, query['profile']))
  yield put(syncDispatch(Actions.setActiveEmulator, query['emulator']))
  yield put(syncDispatch(Actions.retrieveGameConfiguration.started, { gameGuid: query['game'], profileName: query['profile'] }))
}

function* retrieveGameConfigurations (snowflake: Snowflake, action: SyncPayload<{gameGuid: string, profileName: string}>): SagaIterator {
  try {
    const { gameGuid, profileName } = action.payload
    const configs: { [emulatorName: string]: ConfigurationCollection } = yield call(snowflake.games.getConfigurations, gameGuid, profileName)
    yield put(successDispatch(Actions.retrieveGameConfiguration, configs, action.payload))
  } catch (e) {
    yield put (failedDispatch(Actions.retrieveGameConfiguration, e))
  }
}

function* refreshGameConfiguration (snowflake: Snowflake, action: SyncPayload<{configKey: ConfigurationKey, newValue: ConfigurationValue}>): SagaIterator {
  const elementId = action.payload.newValue.Guid
  try {
    const { gameGuid, profileName, emulatorName } = action.payload.configKey
    yield put(syncDispatch(Actions.setElementLoadingState, { elementId: elementId, loadingState: true}))
    const config: ConfigurationCollection = yield call(snowflake.games.setEmulatorConfigurationValue, gameGuid, profileName, emulatorName, action.payload.newValue)
    yield put(successDispatch(Actions.refreshGameConfiguration, config, action.payload))
    yield put(syncDispatch(Actions.setElementLoadingState, { elementId: elementId, loadingState: false}))
  } catch (e) {
    yield put (failedDispatch(Actions.refreshGameConfiguration, e))
    yield put(syncDispatch(Actions.setElementLoadingState, { elementId: elementId, loadingState: false}))
  }
}

function* refreshGameConfigurations (snowflake: Snowflake, action: SyncPayload<{configKey: ConfigurationKey, newValues: ConfigurationValue[]}>): SagaIterator {
  const elementIds = action.payload.newValues.map(v => v.Guid)
  for (const elementId of elementIds) {
    yield put(syncDispatch(Actions.setElementLoadingState, { elementId: elementId, loadingState: true}))
  }

  try {
    const { gameGuid, profileName, emulatorName } = action.payload.configKey
    const config: ConfigurationCollection = yield call(snowflake.games.setEmulatorConfigurationValues, gameGuid, profileName, emulatorName, action.payload.newValues)
    yield put(successDispatch(Actions.refreshGameConfigurations, config, action.payload))
  } catch (e) {
    yield put (failedDispatch(Actions.refreshGameConfigurations, e))
  } finally {
    for (const elementId of elementIds) {
      yield put(syncDispatch(Actions.setElementLoadingState, { elementId: elementId, loadingState: false}))
    }
  }
}

function* rootSaga (snowflake: Snowflake): SagaIterator {
  yield takeEvery(Actions.refreshPlatforms.type, refreshPlatformsWorker, snowflake)
  yield takeEvery(Actions.refreshGames.type, refreshGamesWorker, snowflake)
  yield takeEvery(Actions.retrieveGameConfiguration.started, retrieveGameConfigurations, snowflake)
  yield takeEvery(Actions.locationChange, refreshActiveState)
  yield takeEvery(Actions.refreshGameConfiguration.started, refreshGameConfiguration, snowflake)
  yield takeEvery(Actions.refreshGameConfigurations.started, refreshGameConfigurations, snowflake)

}

export default rootSaga
